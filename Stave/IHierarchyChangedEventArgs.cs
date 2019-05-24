namespace Stave
{
    public interface IHierarchyChangedEventArgs
    {
        IComponent LinkedParent { get; }
        IComponent LinkedChild { get; }
    }

    public interface IHierarchyChangedEventArgs<out TBase> : IHierarchyChangedEventArgs
        where TBase : class, IComponent<TBase>
    {
        new TBase LinkedParent { get; }
        new TBase LinkedChild { get; }
    }

    public interface IHierarchyChangedEventArgs<out TBase, out TContainer> : IHierarchyChangedEventArgs<TBase>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
    {
        new TContainer LinkedParent { get; }
    }
}