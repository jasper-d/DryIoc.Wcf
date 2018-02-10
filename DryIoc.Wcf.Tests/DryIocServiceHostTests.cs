using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;

namespace DryIoc.Wcf.Tests
{
    public class DryIocServiceHostTests
    {
        private readonly Mock<IContainer> _containerMock;
        private readonly Mock<IContainer> _newContainerMock;
        private readonly Mock<ServiceHostBase> _serviceHostMock;
        private readonly InstanceContext _instanceContext;

        public DryIocServiceHostTests()
        {
            _containerMock = new Mock<IContainer>(MockBehavior.Strict);
            _newContainerMock = new Mock<IContainer>(MockBehavior.Strict);
            _serviceHostMock = new Mock<ServiceHostBase>(MockBehavior.Strict);
            _instanceContext = new InstanceContext(_serviceHostMock.Object);
        }

        [Fact]
        public void DryIocServiceHostCtorsThrowForNullArguments()
        {
            Assert.Throws<ArgumentNullException>("container", () => new DryIocServiceHost(null, typeof(DryIocServiceHostTests)));
            Assert.Throws<ArgumentNullException>("container", () => new DryIocServiceHost(null, new object()));
            Assert.Throws<ArgumentNullException>("singletonInstance", () => new DryIocServiceHost(_containerMock.Object, default(object)));
        }

        [Fact]
        public void OnOpeningAddsDryIocServiceBehavior()
        {
            var serviceHost = new ServiceHostStub(_containerMock.Object, typeof(DryIocServiceHostTests), new Uri[0]);
            var behaviorCount = serviceHost.Description.Behaviors.Count;

            serviceHost.CallOnOpening();

            Assert.Equal(behaviorCount + 1, serviceHost.Description.Behaviors.Count);
            var dryIocBehaviors = serviceHost.Description.Behaviors.OfType<DryIocServiceBehavior>();
            Assert.Single(dryIocBehaviors);
        }

        private class ServiceHostStub : DryIocServiceHost
        {
            public ServiceHostStub(IContainer container, Type serviceType, params Uri[] baseAddresses) : base(container, serviceType, baseAddresses)
            {
            }

            public void CallOnOpening()
            {
                base.OnOpening();
            }
        }
    }
}
