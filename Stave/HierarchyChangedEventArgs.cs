namespace Stave
{
    public class HierarchyChangedEventArgs<TBase, TContainer> : IHierarchyChangedEventArgs<TBase, TContainer>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
    {
        public HierarchyChangeType ChangeType { get; }
        public TContainer Parent { get; }
        public TBase Child { get; }

        static public HierarchyChangedEventArgs<TBase, TContainer> Link(TContainer parent, TBase child)
            => new HierarchyChangedEventArgs<TBase, TContainer>(HierarchyChangeType.Link, parent, child);
        static public HierarchyChangedEventArgs<TBase, TContainer> Unlink(TContainer parent, TBase child)
            => new HierarchyChangedEventArgs<TBase, TContainer>(HierarchyChangeType.Unlink, parent, child);

        private HierarchyChangedEventArgs(HierarchyChangeType changeType, TContainer linkedParent, TBase linkedChild)
        {
            ChangeType = changeType;
            Parent = linkedParent;
            Child = linkedChild;
        }

        TBase IHierarchyChangedEventArgs<TBase>.Parent => Parent;
        IComponent IHierarchyChangedEventArgs.Parent => Parent;
        IComponent IHierarchyChangedEventArgs.Child => Child;
    }
}