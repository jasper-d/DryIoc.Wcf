using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Moq;
using Xunit;

namespace DryIoc.Wcf.Tests {
    public class DryIocServiceBehaviorTests {
        private readonly Mock<IContainer> _containerMock;
        private readonly Mock<ServiceHostBase> _serviceHostMock;

        public DryIocServiceBehaviorTests() {
            _containerMock = new Mock<IContainer>(MockBehavior.Strict);
            _serviceHostMock = new Mock<ServiceHostBase>(MockBehavior.Strict);
        }

        [Fact]
        public void DryIocServiceBehaviorCtorThrowsNullArgumentExceptionForNullArguments() {
            Assert.Throws<ArgumentNullException>("container", () => new DryIocServiceBehavior(null));
        }

        [Fact]
        public void ApplyDispatchBehaviorThrowsNullArgumentExceptionForNullArguments() {
            var sut = new DryIocServiceBehavior(_containerMock.Object);
            var serviceDescription = new ServiceDescription();

            Assert.Throws<ArgumentNullException>("serviceDescription", () => sut.ApplyDispatchBehavior(null, _serviceHostMock.Object));
            Assert.Throws<ArgumentNullException>("serviceHostBase", () => sut.ApplyDispatchBehavior(serviceDescription, null));
        }

        [Fact]
        public void InstanceProviderIsSetForImplementedContracts() {
            var sut = new DryIocServiceBehavior(_containerMock.Object);
            var serviceEndpoints = new[] {
                new ServiceEndpoint(new ContractDescription(nameof(String)) { ContractType = typeof(string)})
            };

            var serviceDescription = new ServiceDescription(serviceEndpoints) { ServiceType = typeof(string) };

            var channelListener = new Mock<IChannelListener>();
            var channelDispatcher = CreateChannelDispatchers();

            _serviceHostMock.Object.ChannelDispatchers.Add(channelDispatcher);

            sut.ApplyDispatchBehavior(serviceDescription, _serviceHostMock.Object);

            Assert.Equal(3, channelDispatcher.Endpoints.Count);
            Assert.True(channelDispatcher.Endpoints.Single(e => e.ContractName == nameof(String)).DispatchRuntime.InstanceProvider.GetType() == typeof(DryIocInstanceProvider));
            Assert.True(channelDispatcher.Endpoints.Where(e => e.ContractName != nameof(String)).Select(e => e.DispatchRuntime).All(dr => dr.InstanceProvider == null));
        }

        private static ChannelDispatcher CreateChannelDispatchers() {
            var channelListener = new Mock<IChannelListener>();
            var channelDispatcher = new ChannelDispatcher(channelListener.Object);

            channelDispatcher.Endpoints.Add(new EndpointDispatcher(new EndpointAddress("http://foo"), nameof(Int32), "ns"));
            channelDispatcher.Endpoints.Add(new EndpointDispatcher(new EndpointAddress("http://foo"), nameof(String), "ns"));
            channelDispatcher.Endpoints.Add(new EndpointDispatcher(new EndpointAddress("http://foo"), nameof(Int64), "ns"));

            return channelDispatcher;
        }
    }
}
