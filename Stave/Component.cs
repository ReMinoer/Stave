using System.Collections.Generic;
using Stave.Base;

namespace Stave
{
    public class Component<TBase, TContainer> : ComponentBase<TBase, TContainer>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
    {
        internal override sealed IEnumerable<TBase> ReadOnlyBaseComponents
        {
            get { yield break; }
        }

        public Component()
        {
        }

        public Component(TBase owner)
            : base(owner)
        {
        }
    }
}