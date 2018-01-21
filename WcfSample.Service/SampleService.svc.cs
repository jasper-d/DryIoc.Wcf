using System;
using System.Linq;
using System.Threading.Tasks;
using WcfSample.Service.Dependencies;
using MoreLinq;

namespace WcfSample.Service
{
    public class SampleService : ISampleService {
        private readonly Guid _hash;
        private IFoo _foo;
        private IBar _bar;
        private ISingleton _singleton;
        private ITransient _transient;
        private IAsyncClass _asyncClass;

        public SampleService(IFoo foo, IBar bar, ISingleton singleton, ITransient transient, IAsyncClass asyncClass) {
            _hash = Guid.NewGuid();
            if (foo == null) {
                throw new ArgumentNullException(nameof(foo));
            }
            if (bar == null) {
                throw new ArgumentNullException(nameof(bar));
            }
            if (singleton == null) {
                throw new ArgumentNullException(nameof(singleton));
            }
            if (transient == null) {
                throw new ArgumentNullException(nameof(transient));
            }

            if (asyncClass == null) {
                throw new ArgumentNullException(nameof(asyncClass));
            }

            _foo = foo;
            _bar = bar;
            _singleton = singleton;
            _transient = transient;
            _asyncClass = asyncClass;
        }

        public string GetData(int value) {
            return value.ToString();
        }

        public Guid GetHashCodeOfFoo() {
            return _foo.GetHash();
        }

        public Guid GetHashCodeOfBar() {
            return _bar.GetHash();
        }

        public Guid GetHashCodeOfSingleton() {
            return _singleton.GetHash();
        }

        public Guid GetHashCodeOfTransient() {
            return _transient.GetHash();
        }

        public Guid GetHashCodeOfSelf() {
            return _hash;
        }

        public bool BarEqualsFooBar() {
            return _foo.Bar == _bar;
        }

        public async Task<bool> ResolutionWorksForAsyncOperations() {
            var results = await Task.WhenAll(Enumerable.Range(0, 10).Select(i => _asyncClass.ResolveTransientAsync()));
            var first = results.First();
            var count = results.Count();
            var hashCodes = results.Select(r => r.GetHashCode());
            var distinctCount = results.DistinctBy(x => x.GetHashCode()).Count();

            return count == distinctCount && results.All(r => r != null);
        }
    }
}
