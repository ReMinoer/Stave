using System.Collections.Generic;
using Stave.Base;

namespace Stave
{
    public class Component<TAbstract, TParent> : ComponentBase<TAbstract, TParent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
    {
        internal override sealed IEnumerable<TAbstract> InternalAbstracts
        {
            get { yield break; }
        }
    }
}