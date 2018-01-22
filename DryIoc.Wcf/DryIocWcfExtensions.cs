using System;
using System.Linq;
using System.ServiceModel;

namespace DryIoc.Wcf {
    public static partial class DryIocWcfExtensions {
        internal static ServiceBehaviorAttribute GetServiceBehaviorAttribute(this Type type) {

            return type.GetCustomAttributes(typeof(ServiceBehaviorAttribute), true)
                    .OfType<ServiceBehaviorAttribute>()
                    .FirstOrDefault();
        }
    }
}
