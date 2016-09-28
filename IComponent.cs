using System.Collections.Generic;

namespace Stave
{
    public interface IComponent<out TAbstract, TParent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
    {
        TParent Parent { get; set; }
        IEnumerable<TAbstract> Components { get; }
    }
}