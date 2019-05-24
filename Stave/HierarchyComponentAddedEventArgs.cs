namespace Stave
{
    public class HierarchyComponentAddedEventArgs<TBase, TContainer> : IHierarchyComponentAddedEventArgs<TBase, TContainer>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
    {
        public TContainer Parent { get; }
        public TBase NewComponent { get; }

        public HierarchyComponentAddedEventArgs(TContainer parent, TBase newComponent)
        {
            Parent = parent;
            NewComponent = newComponent;
        }
        
        TContainer IHierarchyComponentAddedEventArgs<TBase, TContainer>.Parent => Parent;
        TBase IHierarchyComponentAddedEventArgs<TBase>.Parent => Parent;
        IComponent IHierarchyComponentAddedEventArgs.Parent => Parent;
        IComponent IHierarchyComponentAddedEventArgs.NewComponent => NewComponent;
        TBase IHierarchyComponentAddedEventArgs<TBase>.NewComponent => NewComponent;
    }
}