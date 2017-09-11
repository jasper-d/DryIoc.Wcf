using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WcfSample.Service.Dependencies {
    public interface IFoo {
        IBar Bar { get; }

        Guid GetHash();
    }

    public class Foo : IFoo {
        private readonly Guid _hash;
        private IBar _bar;

        public IBar Bar { get { return _bar; } }

        public Foo(IBar bar) {
            _hash = Guid.NewGuid();
            if (bar == null) {
                throw new ArgumentNullException(nameof(bar));
            }

            _bar = bar;
        }

        public Guid GetHash() {
            return _hash;
        }
    }

    public interface IBar {
        Guid GetHash();
    }

    public class Bar : IBar {
        private readonly Guid _hash;

        public Bar() {
            _hash = Guid.NewGuid();
        }

        public Guid GetHash() {
            return _hash;
        }
    }

    public interface ISingleton {
        Guid GetHash();
    }
    public class Singleton : ISingleton {
        private readonly Guid _hash;

        public Singleton() {
            _hash = Guid.NewGuid();
        }

        public Guid GetHash() {
            return _hash;
        }
    };

    public interface ITransient {
        Guid GetHash();
    }
    public class Transient : ITransient {
        private readonly Guid _hash;

        public Transient() {
            _hash = Guid.NewGuid();
        }

        public Guid GetHash() {
            return _hash;
        }
    }

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