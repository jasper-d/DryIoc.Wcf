using System;
using WcfSample.Service.Dependencies;

namespace WcfSample.Service {
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class SampleService : ISampleService {
        private IFoo _foo;
        private IBar _bar;
        private ISingleton _singleton;
        private ITransient _transient;

        public SampleService(IFoo foo, IBar bar, ISingleton singleton, ITransient transient) {
            if(foo == null) {
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

            _foo = foo;
            _bar = bar;
            _singleton = singleton;
            _transient = transient;
        }

        public string GetData(int value) {
            return value.ToString();
        }

        public int GetHashCodeOfFoo() {
            return _foo.GetHashCode();
        }

        public int GetHashCodeOfBar() {
            return _bar.GetHashCode();
        }

        public int GetHashCodeOfSingleton() {
            return _singleton.GetHashCode();
        }

        public int GetHashCodeOfTransient() {
            return _transient.GetHashCode();
        }

        public int GetHashCodeOfSelf() {
            return GetHashCode();
        }

        public bool BarEqualsFooBar() {
            return _foo.Bar == _bar;
        }
    }
}
