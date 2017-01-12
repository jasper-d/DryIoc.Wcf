using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.ServiceModel;

namespace DryIoc.Wcf {
    /// <summary>
    /// Extension methods for integrating Simple Injector with WCF services.
    /// </summary>
    public static partial class DryIocWcfExtensions {
        //        /// <summary>
        //        /// Registers the WCF services instances (public classes that implement an interface that
        //        /// is decorated with a <see cref="ServiceContractAttribute"/>) that are 
        //        /// declared as public non-abstract in the supplied set of <paramref name="assemblies"/>.
        //        /// </summary>
        //        /// <param name="container">The container the services should be registered in.</param>
        //        /// <param name="assemblies">The assemblies to search.</param>
        //        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="container"/> is 
        //        /// a null reference (Nothing in VB).</exception>
        //        public static void RegisterWcfServices(this IContainer container, params Assembly[] assemblies) {
        //            if (container == null) {
        //                throw new ArgumentNullException(nameof(container));
        //            }

        //            if (assemblies == null || assemblies.Length == 0) {
        //                assemblies = AppDomain.CurrentDomain.GetAssemblies();
        //            }

        //            var serviceTypes = (
        //                from assembly in assemblies
        //                where !assembly.IsDynamic
        //                from type in GetExportedTypes(assembly)
        //                where !type.IsAbstract
        //                where !type.IsGenericTypeDefinition
        //                where IsWcfServiceType(type)
        //                select type)
        //                .ToArray();

        //            VerifyConcurrencyMode(serviceTypes);

        //            foreach (Type serviceType in serviceTypes) {
        //                IReuse reuse = GetAppropriateLifestyle(serviceType);
        //                container.Register(serviceType, serviceType, reuse);
        //            }
        //        }

        internal static ServiceBehaviorAttribute GetServiceBehaviorAttribute(this Type type) =>
        type.GetCustomAttributes(typeof(ServiceBehaviorAttribute), true)
            .OfType<ServiceBehaviorAttribute>()
            .FirstOrDefault();

        //        private static bool IsWcfServiceType(Type type) {
        //            bool typeIsDecorated = type.GetCustomAttributes(typeof(ServiceContractAttribute), true).Any();

        //            bool typesInterfacesAreDecorated = (
        //                from @interface in type.GetInterfaces()
        //                where @interface.IsPublic
        //                where @interface.GetCustomAttributes(typeof(ServiceContractAttribute), true).Any()
        //                select @interface)
        //                .Any();

        //            return typeIsDecorated || typesInterfacesAreDecorated;
        //        }

        //        private static void VerifyConcurrencyMode(Type[] serviceTypes) {
        //            foreach (Type serviceType in serviceTypes) {
        //                VerifyConcurrencyMode(serviceType);
        //            }
        //        }

        //        private static void VerifyConcurrencyMode(Type wcfServiceType) {
        //            if (HasInvalidConcurrencyMode(wcfServiceType)) {
        //                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
        //                    "The WCF service class {0} is configured with ConcurrencyMode Multiple, but this is not " +
        //                    "supported by Simple Injector. Please change the ConcurrencyMode to Single.",
        //                    wcfServiceType.FullName));
        //            }
        //        }

        //        private static bool HasInvalidConcurrencyMode(Type wcfServiceType) {
        //            var attribute = GetServiceBehaviorAttribute(wcfServiceType);

        //            return attribute != null && attribute.ConcurrencyMode == ConcurrencyMode.Multiple;
        //        }

        //        private static IReuse GetAppropriateLifestyle(Type wcfServiceType) {
        //            var attribute = GetServiceBehaviorAttribute(wcfServiceType);

        //            bool singleton = attribute?.InstanceContextMode == InstanceContextMode.Single;

        //            return singleton ? Reuse.Singleton : WcfReuse.InWcfOperation;
        //        }

        //        private static IEnumerable<Type> GetExportedTypes(Assembly assembly) {
        //            try {
        //                return assembly.GetExportedTypes();
        //            } catch (NotSupportedException) {
        //                // A type load exception would typically happen on an Anonymously Hosted DynamicMethods 
        //                // Assembly and it would be safe to skip this exception.
        //                return Type.EmptyTypes;
        //            } catch (ReflectionTypeLoadException ex) {
        //                // Return the types that could be loaded. Types can contain null values.
        //                return ex.Types.Where(type => type != null);
        //            } catch (Exception ex) {
        //                // Throw a more descriptive message containing the name of the assembly.
        //                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
        //                    "Unable to load types from assembly {0}. {1}", assembly.FullName, ex.Message), ex);
        //            }
        //        }
    }
}
