using System.ServiceModel;

namespace DryIoc.Wcf {
    public class WcfOperationScopeContext : IScopeContext {
        private InstanceContext _instanceContext;

        public string RootScopeName {
            get { return WcfReuse.WcfOperationScopeName; }
        }

        internal WcfOperationScopeContext(InstanceContext instanceContext) {
            _instanceContext = instanceContext;
        }

        public void Dispose() {
            Dispose(true);
        }

        protected void Dispose(bool disposing) {
            if (_instanceContext != null) {
                try {
                    _instanceContext.RemoveScope();
                } finally {
                    _instanceContext = null;
                }
            }
        }

        public IScope GetCurrentOrDefault() {
            var operationContext = OperationContext.Current;
            var instanceContext = operationContext != null ? operationContext.InstanceContext : null;

            return instanceContext != null ? instanceContext.GetCurrentScope() : null;
        }

        public IScope SetCurrent(SetCurrentScopeHandler setCurrentScope) {
            return setCurrentScope(GetCurrentOrDefault());
        }
    }
}
