using DryIoc;
using System;
using System.Threading.Tasks;

namespace WcfSample.Service.Dependencies {
    public interface IFoo {
        IBar Bar { get; }

        Guid GetId();
    }

    public class Foo : IFoo {
        private readonly Guid _id;

        public IBar Bar { get; }


        public Foo(IBar bar) {
            _id = Guid.NewGuid();
            Bar = bar ?? throw new ArgumentNullException(nameof(bar));
        }

        public Guid GetId() {
            return _id;
        }
    }

    public interface IBar {
        Guid GetId();
    }

    public class Bar : IBar {
        private readonly Guid _id;

        public Bar() {
            _id = Guid.NewGuid();
        }

        public Guid GetId() {
            return _id;
        }
    }

    public interface ISingleton {
        Guid GetId();
    }
    public class Singleton : ISingleton {
        private readonly Guid _id;

        public Singleton() {
            _id = Guid.NewGuid();
        }

        public Guid GetId() {
            return _id;
        }
    };

    public interface ITransient {
        Guid GetId();
    }
    public class Transient : ITransient {
        private readonly Guid _id;

        public Transient() {
            _id = Guid.NewGuid();
        }

        public Guid GetId() {
            return _id;
        }
    }

    public interface IAsyncClass {
        Task<IBar> ResolveAsync();
        Task<ITransient> ResolveTransientAsync();
    }

    public class AsyncClass : IAsyncClass {
        private IContainer _container;

        public AsyncClass(IContainer container) {
            _container = container ?? throw new ArgumentNullException(nameof(container));
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