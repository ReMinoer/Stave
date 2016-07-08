namespace Stave
{
    public interface IParent<TAbstract, TParent> : IComponent<TAbstract, TParent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
    {
        bool Link(TAbstract child);
        bool Unlink(TAbstract child);
    }
}