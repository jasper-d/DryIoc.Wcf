using System;
using System.Reflection;
using System.ServiceModel;
using FastExpressionCompiler;
using FastExpressionCompiler.LightExpression;
using Moq;
using Xunit;

namespace DryIoc.Wcf.Tests {
    public class DryIocServiceHostFactoryTests {
        private readonly Mock<IContainer> _container;

        public DryIocServiceHostFactoryTests() {
            _container = new Mock<IContainer>(MockBehavior.Strict);
        }

        [Fact]
        public void SetContainerThrowsIfContainerIsAlreadySet() {
            SetContainer(null);

            DryIocServiceHostFactory.SetContainer(_container.Object);
            Assert.Throws<InvalidOperationException>(() => DryIocServiceHostFactory.SetContainer(_container.Object));
        }

        [Fact]
        public void SetContainerThrowsIfContainerIsNull() {
            SetContainer(null);
            Assert.Throws<ArgumentNullException>("container", () => DryIocServiceHostFactory.SetContainer(null));
        }

        [Fact]
        public void CreateServiceHostThrowsIfContainerIsNull() {
            SetContainer(null);
            var fake = new ServiceHostFactoryStub();
            Assert.Throws<InvalidOperationException>(() => fake.CallCreateServiceHost(typeof(ServiceHostFactoryStub), null));
        }

        [Fact]
        public void CreateServiceHostThrowsIfContractTypeIsNull() {
            SetContainer(_container.Object);
            var fake = new ServiceHostFactoryStub();
            Assert.Throws<ArgumentNullException>("contractType", () => fake.CallCreateServiceHost(null, null));
        }

        [Fact]
        public void SingletonServicesAreCreatedAsSingletonAndSetInServiceHost() {
            var contractType = typeof(IService);
            var stub = new ServiceHostFactoryStub();
            var addresses = new Uri[0];
            var (instance, factory) = SetupFactoryForSingletonResolution<SingletonService>(contractType, typeof(SingletonService));
            factory.SetupGet(f => f.Reuse).Returns(Reuse.Singleton);

            var createdServiceHost = stub.CallCreateServiceHost(contractType, addresses);
            _container.Verify(c => c.Resolve(contractType, IfUnresolved.Throw), Times.Once);
            Assert.Same(instance, createdServiceHost.SingletonInstance);
        }

        [Fact]
        public void ServiceHostFactoryThrowsIfSingletonServiceIsNotRegisteredAsSingleton() {
            var contractType = typeof(IService);
            var stub = new ServiceHostFactoryStub();
            var addresses = new Uri[0];
            var (instance, _) = SetupFactoryForSingletonResolution<SingletonService>(contractType, typeof(SingletonService));

            Assert.Throws<InvalidOperationException>(() => stub.CallCreateServiceHost(contractType, addresses));

            _container.Verify(c => c.Resolve(contractType, IfUnresolved.Throw), Times.Never);
        }

        [Fact]
        public void NonSingletonServicesAreNotCreatedAsSingletonAndNotSetInServiceHost() {
            var contractType = typeof(IService);
            var addresses = new Uri[0];
            var stub = new ServiceHostFactoryStub();
            var (instance, _) = SetupFactoryForSingletonResolution<NonSingletonService>(contractType, typeof(NonSingletonService));

            var createdServiceHost = stub.CallCreateServiceHost(contractType, addresses);

            _container.Verify(c => c.Resolve(contractType, IfUnresolved.Throw), Times.Never);
            Assert.Null(createdServiceHost.SingletonInstance);
        }

        private (T instance, Mock<FactoryStub> factory) SetupFactoryForSingletonResolution<T>(Type contractType, Type implType) where T : new() {
            SetContainer(_container.Object);
            var instance = new T();
            var factory = new Mock<FactoryStub>();
            factory.SetupGet(f => f.ImplementationType).Returns(implType);
            factory.SetupGet(f => f.Reuse).Returns(Reuse.InThread);
            var serviceRegistration = new ServiceRegistrationInfo() { Factory = factory.Object, ServiceType = contractType };
            _container.Setup(c => c.GetServiceRegistrations()).Returns(new[] { serviceRegistration });
            _container.Setup(c => c.Resolve(contractType, IfUnresolved.Throw)).Returns(instance);

            return (instance, factory);
        }

        public class ServiceHostFactoryStub : DryIocServiceHostFactory {
            public ServiceHost CallCreateServiceHost(Type contractType, Uri[] baseAddresses) {
                return base.CreateServiceHost(contractType, baseAddresses);
            }
        }

        public class FactoryStub : Factory {
            public override Expression CreateExpressionOrDefault(Request request) {
                return null;
            }
        }

        private void SetContainer(IContainer container) {
            var fieldInfo = typeof(DryIocServiceHostFactory).GetField("Container", BindingFlags.NonPublic | BindingFlags.Static);
            fieldInfo.SetValue(null, container);
        }

        [ServiceContract]
        private interface IService { }

        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        private class SingletonService : IService { }

        private class NonSingletonService : IService { }
    }
}
