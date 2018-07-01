using System.ServiceModel;

namespace DryIoc.Wcf {
    internal static class InstanceContextExtension {

        internal static IResolverContext OpenScope(this InstanceContext instanceContext, IContainer container) {
            var extension = instanceContext.Extensions.Find<DryIocInstanceContextExtension>();

            if (extension == null) {
                instanceContext.Extensions.Add(extension = new DryIocInstanceContextExtension());
            }

            if (extension.ResolverContext == null) {
                extension.ResolverContext = container.OpenScope(null, false);
                return extension.ResolverContext;
            }

            return extension.ResolverContext.OpenScope(null, false);
        }

        internal static IScope GetCurrentScope(this InstanceContext instanceContext) {
            if (instanceContext == null) {
                return null;
            }

            var extension = instanceContext.Extensions.Find<DryIocInstanceContextExtension>();

            return extension?.ResolverContext.CurrentScope;
        }

        private sealed class DryIocInstanceContextExtension : IExtension<InstanceContext> {
            internal IResolverContext ResolverContext { get; set; }

            public void Attach(InstanceContext owner) {
            }

            public void Detach(InstanceContext owner) {
            }
        }
    }
}
