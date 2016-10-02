namespace Stave
{
    public interface IDecorator<TAbstract, TParent, TComponent> : IParent<TAbstract, TParent, TComponent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
        where TComponent : class, TAbstract
    {
        TComponent Component { get; set; }
        TComponent Unlink();
    }
}