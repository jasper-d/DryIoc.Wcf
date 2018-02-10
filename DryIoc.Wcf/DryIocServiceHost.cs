using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace DryIoc.Wcf {
    public class DryIocServiceHost : ServiceHost {
        private readonly IContainer _container;
        private readonly Type _serviceAbstraction;

        public DryIocServiceHost(IContainer container, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses) {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public DryIocServiceHost(IContainer container, object singletonInstance, params Uri[] baseAddresses)
            : base(singletonInstance, baseAddresses)
        {
            if (singletonInstance == null) {
                throw new ArgumentNullException(nameof(singletonInstance));
            }

            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        internal DryIocServiceHost(IContainer container, Type serviceAbstraction, Type implementationType, params Uri[] baseAddresses)
            : base(implementationType, baseAddresses) {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _serviceAbstraction = serviceAbstraction ?? throw new ArgumentNullException(nameof(serviceAbstraction));
        }

        public IEnumerable<ContractDescription> GetImplementedContracts() => ImplementedContracts.Values;

        protected override void OnOpening() {
            Description.Behaviors.Add(new DryIocServiceBehavior(_container) {
                ServiceType = _serviceAbstraction
            });

            base.OnOpening();
        }
    }
}
