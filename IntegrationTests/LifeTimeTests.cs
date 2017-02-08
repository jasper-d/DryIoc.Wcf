using IntegrationTests.ServiceReference;
using System;
using System.Threading.Tasks;
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
            Assert.Equal(arg.ToString(), response + "1");
        }

        [Fact]
        public void SingletonHasSameHashCode() {
            var response1 = _service.GetHashCodeOfSingleton();
            var response2 = _service.GetHashCodeOfSingleton();

            Assert.Equal(response1, response2);
        }

        [Fact]
        public void TransientHasDifferentHashCode() {
            var response1 = _service.GetHashCodeOfTransient();
            var response2 = _service.GetHashCodeOfTransient();

            Assert.NotEqual(response1, response2);
        }

        [Fact]
        public void TransientServiceHasDifferentHashCode() {
            var response1 = _service.GetHashCodeOfSelf();
            var response2 = _service.GetHashCodeOfSelf();

            Assert.NotEqual(response1, response2);
        }

        [Fact]
        public void CurrentScopedDependencyIsCreatedOncePerScope() {
            Assert.True(_service.BarEqualsFooBar());
        }

        [Fact]
        public void CurrentScopeIsNew() {
            var response1 = _service.GetHashCodeOfFoo();
            var response2 = _service.GetHashCodeOfFoo();

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
