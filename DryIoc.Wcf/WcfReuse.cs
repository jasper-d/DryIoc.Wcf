namespace DryIoc.Wcf {
    public static class WcfReuse {
        internal static string WcfOperationScopeName = nameof(WcfOperationScopeName);

        public static readonly IReuse InWcfOperation = Reuse.InCurrentNamedScope(WcfOperationScopeName);
    }
}
