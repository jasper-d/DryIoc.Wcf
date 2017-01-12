using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public interface IAsyncClass {
        Task<IBar> ResolveAsync();
        Task<ITransient> ResolveTransientAsync();
    }

    public class AsyncClass : IAsyncClass {
        private IContainer _container;

        public AsyncClass(IContainer container) {
            if (container == null) {
                throw new ArgumentNullException(nameof(container));
            }

            _container = container;
        }

        public async Task<IBar> ResolveAsync() {
            await Task.Delay(50);
            return _container.Resolve<IBar>(IfUnresolved.Throw);
        }

        public async Task<ITransient> ResolveTransientAsync() {
            await Task.Delay(50);
            return _container.Resolve<ITransient>(IfUnresolved.Throw);
        }
    }
}