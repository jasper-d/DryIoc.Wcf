using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfSample.Service.Dependencies {
    public interface IFoo {
        IBar Bar { get; }
    }

    public class Foo : IFoo {
        private IBar _bar;

        public IBar Bar { get { return _bar; } }

        public Foo(IBar bar) {
            if(bar == null) {
                throw new ArgumentNullException(nameof(bar));
            }

            _bar = bar;
        }
    }

    public interface IBar { }

    public class Bar : IBar { }

    public interface ISingleton { }
    public class Singleton : ISingleton { };

    public interface ITransient { }
    public class Transient : ITransient {}
}