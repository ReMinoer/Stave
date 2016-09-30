using System.Collections.Generic;

namespace Stave
{
    public interface IComposite<TAbstract, TParent, TComponent> : IParent<TAbstract, TParent, TComponent>, ICollection<TComponent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
        where TComponent : class, TAbstract
    {
    }
}