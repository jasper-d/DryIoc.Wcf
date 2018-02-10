using System.ServiceModel;

namespace DryIoc.Wcf {
    internal static class InstanceContextExtension {

        internal static IContainer OpenScope(this InstanceContext instanceContext, IContainer container) {
            var extension = instanceContext.Extensions.Find<DryIocInstanceContextExtension>();

            if (extension == null) {
                instanceContext.Extensions.Add(extension = new DryIocInstanceContextExtension());
            }

            if (extension.CurrentScopedContainer == null) {
                extension.CurrentScopedContainer = container.OpenScope(null, null);
                return extension.CurrentScopedContainer;
            }

            return extension.CurrentScopedContainer.OpenScope(null, null);
        }

        internal static IScope GetCurrentScope(this InstanceContext instanceContext) {
            if (instanceContext == null) {
                return null;
            }

            var extension = instanceContext.Extensions.Find<DryIocInstanceContextExtension>();

            return extension?.CurrentScopedContainer.GetCurrentScope();
        }

        private sealed class DryIocInstanceContextExtension : IExtension<InstanceContext> {
            internal IContainer CurrentScopedContainer { get; set; }

            public void Attach(InstanceContext owner) {
            }

            public void Detach(InstanceContext owner) {
            }
        }
    }
}
