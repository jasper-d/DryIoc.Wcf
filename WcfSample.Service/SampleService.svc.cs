using System;
using System.Linq;
using System.Threading.Tasks;
using WcfSample.Service.Dependencies;
using MoreLinq;

namespace WcfSample.Service
{
    public class SampleService : ISampleService {
        private readonly Guid _id;
        private IFoo _foo;
        private IBar _bar;
        private ISingleton _singleton;
        private ITransient _transient;
        private IAsyncClass _asyncClass;

        public SampleService(IFoo foo, IBar bar, ISingleton singleton, ITransient transient, IAsyncClass asyncClass) {
            _id = Guid.NewGuid();
            _foo = foo ?? throw new ArgumentNullException(nameof(foo));
            _bar = bar ?? throw new ArgumentNullException(nameof(bar));
            _singleton = singleton ?? throw new ArgumentNullException(nameof(singleton));
            _transient = transient ?? throw new ArgumentNullException(nameof(transient));
            _asyncClass = asyncClass ?? throw new ArgumentNullException(nameof(asyncClass));
        }

        public string GetData(int value) {
            return value.ToString();
        }

        public Guid GetIdOfFoo() {
            return _foo.GetId();
        }

        public Guid GetIdOfBar() {
            return _bar.GetId();
        }

        public Guid GetIdOfSingleton() {
            return _singleton.GetId();
        }

        public Guid GetIdOfTransient() {
            return _transient.GetId();
        }

        public Guid GetIdOfSelf() {
            return _id;
        }

        public bool BarEqualsFooBar() {
            return _foo.Bar == _bar;
        }

        public async Task<bool> ResolutionWorksForAsyncOperations() {
            var results = await Task.WhenAll(Enumerable.Range(0, 10).Select(i => _asyncClass.ResolveTransientAsync()));
            var first = results.First();
            var count = results.Count();
            var ids = results.Select(r => r.GetId());
            var distinctCount = results.DistinctBy(x => x.GetId()).Count();

            return count == distinctCount && results.All(r => r != null);
        }
    }
}
