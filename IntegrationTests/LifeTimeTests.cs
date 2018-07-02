using System;
using System.Threading.Tasks;
using IntegrationTests.ServiceReference;
using Xunit;

namespace IntegrationTests {
    public class LifeTimeTests : IDisposable {
        private SampleServiceClient _service;

        public LifeTimeTests() {
            _service = new SampleServiceClient();

        }

        [Fact]
        public void CanCommunicateWithService() {
            var arg = 42;
            var response = _service.GetData(arg);
            Assert.Equal(arg.ToString(), response);
        }

        [Fact]
        public void SingletonHasSameId() {
            var response1 = _service.GetIdOfSingleton();
            var response2 = _service.GetIdOfSingleton();

            Assert.Equal(response1, response2);
        }

        [Fact]
        public void TransientHasDifferentId() {
            var response1 = _service.GetIdOfTransient();
            var response2 = _service.GetIdOfTransient();

            Assert.NotEqual(response1, response2);
        }

        [Fact]
        public void TransientServiceHasDifferentId() {
            var response1 = _service.GetIdOfSelf();
            var response2 = _service.GetIdOfSelf();

            Assert.NotEqual(response1, response2);
        }

        [Fact]
        public void CurrentScopedDependencyIsCreatedOncePerScope() {
            Assert.True(_service.BarEqualsFooBar());
        }

        [Fact]
        public void CurrentScopeIsNew() {
            var response1 = _service.GetIdOfFoo();
            var response2 = _service.GetIdOfFoo();

            Assert.NotEqual(response1, response2);
        }

        [Fact]
        public async Task AsyncResolutionWorks() {
            Assert.True(await _service.ResolutionWorksForAsyncOperationsAsync());
        }

        public void Dispose() {
            _service?.Close();
            (_service as IDisposable)?.Dispose();
        }
    }
}
