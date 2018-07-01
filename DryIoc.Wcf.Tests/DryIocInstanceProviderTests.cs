using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Moq;
using Xunit;

namespace DryIoc.Wcf.Tests
{
    public class DryIocInstanceProviderTests
    {
        private readonly Mock<IContainer> _containerMock;
        private readonly Mock<IResolverContext> _newContainerMock;
        private readonly Mock<ServiceHostBase> _serviceHostMock;
        private readonly InstanceContext _instanceContext;

        public DryIocInstanceProviderTests()
        {
            _containerMock = new Mock<IContainer>(MockBehavior.Strict);
            _newContainerMock = new Mock<IResolverContext>(MockBehavior.Strict);
            _serviceHostMock = new Mock<ServiceHostBase>(MockBehavior.Strict);
            _instanceContext = new InstanceContext(_serviceHostMock.Object);
        }

        [Fact]
        public void DryIocInstanceProviderCtorThrowsNullArgumentExceptionForNullArguments()
        {
            Assert.Throws<ArgumentNullException>("container", () => new DryIocInstanceProvider(null, typeof(DryIocInstanceProviderTests)));
            Assert.Throws<ArgumentNullException>("serviceType", () => new DryIocInstanceProvider(_containerMock.Object, null));
        }

        [Fact]
        public void GetInstanceThrowsNullArgumentExceptionForNullArgument()
        {
            var sut = new DryIocInstanceProvider(_containerMock.Object, typeof(DryIocInstanceProviderTests));
            Assert.Throws<ArgumentNullException>("instanceContext", () => sut.GetInstance(null));
        }

        [Fact]
        public void GetInstanceResolvesInstanceUsingDryIoc()
        {
            var serviceType = typeof(string);
            var resolvedObject = string.Empty;
            var message = new Mock<Message>(MockBehavior.Loose).Object;

            _containerMock.Setup(c => c.OpenScope(null, false)).Returns(_newContainerMock.Object);
            _newContainerMock.Setup(c => c.Resolve(serviceType, false)).Returns(resolvedObject);

            var sut = new DryIocInstanceProvider(_containerMock.Object, serviceType);
            var instance = sut.GetInstance(_instanceContext, message);
            Assert.Equal(resolvedObject, instance);
            Assert.IsType(serviceType, instance);

            _containerMock.Verify(c => c.OpenScope(null, false), Times.Once);
            _newContainerMock.Verify(c => c.Resolve(serviceType, false), Times.Once);
        }

        [Fact]
        public void ContainerIsDisposedIfResolveThrows()
        {
            var serviceType = typeof(string);
            var resolvedObject = string.Empty;

            _containerMock.Setup(c => c.OpenScope(null, false)).Returns(_newContainerMock.Object);
            _newContainerMock.Setup(c => c.Resolve(serviceType, false)).Throws<InvalidOperationException>();
            _newContainerMock.Setup(c => c.Dispose());

            var sut = new DryIocInstanceProvider(_containerMock.Object, serviceType);
            Assert.Throws<InvalidOperationException>(() => sut.GetInstance(_instanceContext));

            _containerMock.Verify(c => c.OpenScope(null, false), Times.Once);
            _newContainerMock.Verify(c => c.Resolve(serviceType, false), Times.Once);
            _newContainerMock.Verify(c => c.Dispose(), Times.Once);
        }

        [Fact]
        public void ReleaseInstanceThrowsIfInstanceContextIsNull()
        {
            var serviceType = typeof(string);
            var sut = new DryIocInstanceProvider(_containerMock.Object, serviceType);
            Assert.Throws<ArgumentNullException>("instanceContext", () => sut.ReleaseInstance(null, new object()));
        }

        [Fact]
        public void RealeaseInstanceDoesNotThrowIfCurrentScopeIsNull()
        {
            var instanceProvider = new DryIocInstanceProvider(_containerMock.Object, typeof(DryIocInstanceProviderTests));
            instanceProvider.ReleaseInstance(_instanceContext, new object());
        }
    }
}
