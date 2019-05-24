namespace Stave
{
    public class HierarchyChangedEventArgs<TBase, TContainer> : IHierarchyChangedEventArgs<TBase, TContainer>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
    {
        public TContainer LinkedParent { get; }
        public TBase LinkedChild { get; }

        public HierarchyChangedEventArgs(TContainer linkedParent, TBase linkedChild)
        {
            LinkedParent = linkedParent;
            LinkedChild = linkedChild;
        }

        TBase IHierarchyChangedEventArgs<TBase>.LinkedParent => LinkedParent;
        IComponent IHierarchyChangedEventArgs.LinkedParent => LinkedParent;
        IComponent IHierarchyChangedEventArgs.LinkedChild => LinkedChild;
    }
}