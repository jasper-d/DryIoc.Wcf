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
            if(container == null) {
                throw new ArgumentNullException(nameof(container));
            }

            _container = container;
        }

        public DryIocServiceHost(IContainer container, object singletonInstance, params Uri[] baseAddresses)
            : base(singletonInstance, baseAddresses)
        {
            if (container == null) {
                throw new ArgumentNullException(nameof(container));
            }
            if (container == null) {
                throw new ArgumentNullException(nameof(singletonInstance));
            }

            _container = container;
        }

        internal DryIocServiceHost(IContainer container, Type serviceAbstraction, Type implementationType, params Uri[] baseAddresses)
            : base(implementationType, baseAddresses) {
            if (container == null) {
                throw new ArgumentNullException(nameof(container));
            }
            if (serviceAbstraction == null) {
                throw new ArgumentNullException(nameof(container));
            }

            _container = container;
            _serviceAbstraction = serviceAbstraction;
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
