using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using DryIoc;
using DryIoc.Wcf;
using IntegrationTests.ServiceReference;
using MoreLinq;
using Xunit;

namespace IntegrationTests {
    public class SelfHostTests {
        [Fact]
        public void SelfHostingWorks() {
            var container = new Container(rules => rules.WithDefaultReuse(Reuse.InCurrentScope), scopeContext: null);

            container.Register(typeof(SampleService), typeof(SampleService), Reuse.Transient);
            container.Register(typeof(IFoo), typeof(Foo));
            container.Register(typeof(IBar), typeof(Bar));
            container.Register(typeof(ISingleton), typeof(Singleton), Reuse.Singleton);
            container.Register(typeof(ITransient), typeof(Transient), Reuse.Transient);
            container.Register(typeof(IAsyncClass), typeof(AsyncClass));

            using(var serviceHost = new DryIocServiceHost(container, typeof(SampleService), new Uri("http://localhost:8080/foo"))) {
                var smb = new ServiceMetadataBehavior { HttpGetEnabled = true };
                serviceHost.Description.Behaviors.Add(smb);
                serviceHost.Open();
                var proxy = new ServiceProxy();
                var id = proxy.GetIdOfSingleton();
                var id2 = proxy.GetIdOfSingleton();
                Assert.Equal(id, id2);
            }
        }
    }


    public class ServiceProxy : ClientBase<ISampleService>, ISampleService {
        public bool BarEqualsFooBar() {
            return base.Channel.BarEqualsFooBar();

        }

        public string GetData(int value) {
            return base.Channel.GetData(42);
        }

        public Guid GetIdOfBar() {
            return Channel.GetIdOfBar();
        }

        public Guid GetIdOfFoo() {
            return Channel.GetIdOfBar();
        }

        public Guid GetIdOfSelf() {
            return Channel.GetIdOfSelf();
        }

        public Guid GetIdOfSingleton() {
            return Channel.GetIdOfSingleton();
        }

        public Guid GetIdOfTransient() {
            return Channel.GetIdOfTransient();
        }

        public Task<bool> ResolutionWorksForAsyncOperations() {
            return Channel.ResolutionWorksForAsyncOperations();
        }
    }

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

    [ServiceContract]
    public interface ISampleService {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        Guid GetIdOfFoo();

        [OperationContract]
        Guid GetIdOfBar();

        [OperationContract]
        Guid GetIdOfSingleton();

        [OperationContract]
        Guid GetIdOfTransient();

        [OperationContract]
        Guid GetIdOfSelf();

        [OperationContract]
        bool BarEqualsFooBar();

        [OperationContract]
        Task<bool> ResolutionWorksForAsyncOperations();
    }

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
