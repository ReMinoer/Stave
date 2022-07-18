namespace Stave
{
    public interface IHierarchyChangedEventArgs
    {
        HierarchyChangeType ChangeType { get; }
        IComponent Parent { get; }
        IComponent Child { get; }
    }

    public interface IHierarchyChangedEventArgs<out TBase> : IHierarchyChangedEventArgs
        where TBase : class, IComponent<TBase>
    {
        new TBase Parent { get; }
        new TBase Child { get; }
    }

    public interface IHierarchyChangedEventArgs<out TBase, out TContainer> : IHierarchyChangedEventArgs<TBase>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
    {
        new TContainer Parent { get; }
    }

    public enum HierarchyChangeType
    {
        Link,
        Unlink
    }
}