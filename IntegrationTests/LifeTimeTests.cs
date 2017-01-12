﻿using IntegrationTests.ServiceReference;
using Xunit;

namespace IntegrationTests {
    public class LifeTimeTests {
        private SampleServiceClient _service;

        public LifeTimeTests() {
            _service = new SampleServiceClient();
        }

        [Fact]
        public void CanCommunicateWithService() {
            var arg = 42;
            var service = new SampleServiceClient();
            var response = service.GetData(arg);

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
    }
}
