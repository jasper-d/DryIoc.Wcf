using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace DryIoc.Wcf {
    public class DryIocServiceHostFactory : ServiceHostFactory {
        private static IContainer Container;

        public static void SetContainer(IContainer container) {
            if (Container != null) {
                throw new InvalidOperationException($"{nameof(SetContainer)} can only be called once. Container is already set.");
            }
            Container = container ?? throw new ArgumentNullException(nameof(container));
        }

        internal static bool HasInstanceContextModeSingle(Type serviceType) =>
            serviceType.GetServiceBehaviorAttribute()?.InstanceContextMode == InstanceContextMode.Single;

        protected override ServiceHost CreateServiceHost(Type contractType, Uri[] baseAddresses) {
            if (contractType == null) {
                throw new ArgumentNullException(nameof(contractType));
            }

            if (Container == null) {
                throw new InvalidOperationException($"The operation failed. Please call the {typeof(DryIocServiceHostFactory).FullName}.{nameof(SetContainer)} method " +
                        $"supplying the application's {typeof(IContainer).FullName} instance during " +
                        "application startup (for instance inside the Application_Start event of the Global.asax). " +
                        "Please note that protocols other than HTTP such as net.tcp and net.pipe " +
                        "(e.g. when using Windows Activation Service (WAS)) are not supported");
            }

            var registrationInfo = Container.GetServiceRegistrations().Single(x => x.ServiceType == contractType);
            var implementationType = registrationInfo.Factory.ImplementationType;

            if (HasInstanceContextModeSingle(implementationType) && AssertIsRegisteredAsSingleton(registrationInfo)) {
                return new DryIocServiceHost(Container, Container.Resolve(contractType), baseAddresses);
            }
            return new DryIocServiceHost(Container, contractType, implementationType, baseAddresses);
        }

        private static bool AssertIsRegisteredAsSingleton(ServiceRegistrationInfo registrationInfo) {
            if (registrationInfo.Factory.Reuse != Reuse.Singleton) {
                throw new InvalidOperationException(String.Format(
                    "Service type {0} has been flagged as 'Single' using the ServiceBehaviorAttribute, " +
                    "causing WCF to hold on to this instance indefinitely, while {1} has been registered " +
                    "with the {2} reuse in the container. Please make sure that {1} is registered " +
                    "as Singleton as well, or mark {0} with " +
                    "[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)] instead.",
                    registrationInfo.ServiceType.FullName,
                    registrationInfo.Factory.ImplementationType.FullName,
                    registrationInfo.Factory.Reuse.ToString()));
            }
            return true;
        }

    }
}
