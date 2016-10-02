using System.Collections.Generic;

namespace Stave
{
    public interface IParent<TAbstract, TParent> : IComponent<TAbstract, TParent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
    {
        bool Link(TAbstract child);
        bool Unlink(TAbstract child);
    }

    public interface IParent<TAbstract, TParent, TComponent> : IParent<TAbstract, TParent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
        where TComponent : class, TAbstract
    {
        new IEnumerable<TComponent> Components { get; }
        bool Link(TComponent child);
        bool Unlink(TComponent child);
    }
}