namespace Stave
{
    public interface IComponentsChangedEventArgs
    {
        ComponentsChangeType ChangeType { get; }
        IContainer Parent { get; }
        IComponent Component { get; }
    }

    public interface IComponentsChangedEventArgs<out TBase> : IComponentsChangedEventArgs
        where TBase : class, IComponent<TBase>
    {
        new TBase Parent { get; }
        new TBase Component { get; }
    }

    public interface IComponentsChangedEventArgs<out TBase, out TContainer> : IComponentsChangedEventArgs<TBase>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
    {
        new TContainer Parent { get; }
    }

    public interface IComponentsChangedEventArgs<out TBase, out TContainer, out TComponent> : IComponentsChangedEventArgs<TBase, TContainer>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
        where TComponent : class, TBase
    {
        new TComponent Component { get; }
    }

    public enum ComponentsChangeType
    {
        Add,
        Remove
    }
}