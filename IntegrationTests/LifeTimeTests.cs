using IntegrationTests.ServiceReference;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

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

        [Fact]
        public void DoesNotFailUnderLoad() {
            var service = new SampleServiceClient();

            var singletons = new ConcurrentBag<Guid>();
            var transients = new ConcurrentBag<Guid>();
            var currentScopes = new ConcurrentBag<Guid>();
            var services = new ConcurrentBag<Guid>();

            var count = 100000;

            Parallel.For(0, count, (i) => {
                var currentScope = service.GetHashCodeOfFoo();//Unique for each request
                var singleton = service.GetHashCodeOfSingleton(); //identicall for each request
                var transient = service.GetHashCodeOfTransient(); //Unique for each request
                var serviceHashCode = service.GetHashCodeOfSelf(); //Unique for each request
                singletons.Add(singleton);
                transients.Add(transient);
                currentScopes.Add(currentScope);
                services.Add(serviceHashCode);
            });

            var distincSingletons = singletons.Distinct().Count();
            var distinctServices = services.Distinct().Count();
            var distinctTransients = transients.Distinct().Count();
            var distinctCurrentScopes = currentScopes.Distinct().Count();

            Assert.Equal(count, singletons.Count());
            Assert.Equal(1, distincSingletons);
            Assert.Equal(count, distinctServices);
            Assert.Equal(count, distinctTransients);
            Assert.Equal(count, distinctCurrentScopes);

        }

        public void Dispose() {
            _service?.Close();
            (_service as IDisposable)?.Dispose();
        }
    }
}
