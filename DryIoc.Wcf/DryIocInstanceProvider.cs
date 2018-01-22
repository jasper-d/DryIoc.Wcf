using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace DryIoc.Wcf {
    public class DryIocInstanceProvider : IInstanceProvider {
        private readonly IContainer _container;
        private readonly Type _serviceType;

        public DryIocInstanceProvider(IContainer container, Type serviceType) {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _serviceType = serviceType ?? throw new ArgumentNullException(nameof(serviceType));
        }

        public object GetInstance(InstanceContext instanceContext) {
            if (instanceContext == null) {
                throw new ArgumentNullException(nameof(instanceContext));
            }

            var scopedContainer = instanceContext.OpenScope(_container);

            try {
                return scopedContainer.Resolve(_serviceType, false);
            } catch {
                scopedContainer.Dispose();
                throw;
            }
            
        }

        public object GetInstance(InstanceContext instanceContext, Message message) => GetInstance(instanceContext);

        public void ReleaseInstance(InstanceContext instanceContext, object instance) {
            if(instanceContext == null) {
                throw new ArgumentNullException(nameof(instanceContext));
            }

            instanceContext.GetCurrentScope()?.Dispose();
        }
    }
}
