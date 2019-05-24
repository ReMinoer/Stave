namespace Stave
{
    public interface IHierarchyComponentAddedEventArgs
    {
        IComponent Parent { get; }
        IComponent NewComponent { get; }
    }

    public interface IHierarchyComponentAddedEventArgs<out TBase> : IHierarchyComponentAddedEventArgs
    {
        new TBase Parent { get; }
        new TBase NewComponent { get; }
    }

    public interface IHierarchyComponentAddedEventArgs<out TBase, out TContainer> : IHierarchyComponentAddedEventArgs<TBase>
    {
        new TContainer Parent { get; }
    }
}