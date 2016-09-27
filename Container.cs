using System.Collections.Generic;
using Stave.Base;
using Stave.Exceptions;

namespace Stave
{
    public class Container<TAbstract, TParent, TComponent> : ComponentEnumerable<TAbstract, TParent, TComponent>, IContainer<TAbstract, TParent, TComponent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
        where TComponent : class, TAbstract
    {
        protected readonly ComponentCollection<TAbstract, TParent, TComponent> Components;

        protected Container()
        {
            Components = new ComponentCollection<TAbstract, TParent, TComponent>(this);
        }

        public override IEnumerator<TComponent> GetEnumerator()
        {
            return Components.GetEnumerator();
        }

        protected override sealed void Link(TComponent component)
        {
            if (component.Parent == this)
                return;

            throw new ReadOnlyParentException();
        }

        protected override sealed void Unlink(TComponent component)
        {
            if (component.Parent == this)
                return;

            throw new ReadOnlyParentException();
        }
    }
}