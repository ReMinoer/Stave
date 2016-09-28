using System.Collections.Generic;
using Stave.Base;

namespace Stave
{
    public class Component<TAbstract, TParent> : ComponentBase<TAbstract, TParent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
    {
        protected override sealed IEnumerable<TAbstract> ProtectedComponents
        {
            get { yield break; }
        }
    }
}