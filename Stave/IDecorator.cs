namespace Stave
{
    public interface IDecorator<TBase, TContainer, TComponent> : IContainer<TBase, TContainer, TComponent>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
        where TComponent : class, TBase
    {
        TComponent Component { get; set; }
        TComponent Unlink();
    }
}