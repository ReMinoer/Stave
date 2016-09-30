using System.Collections.Generic;
using Stave.Base;
using Stave.Exceptions;

namespace Stave
{
    public class Container<TAbstract, TParent, TComponent> : ParentBase<TAbstract, TParent, TComponent>, IContainer<TAbstract, TParent, TComponent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
        where TComponent : class, TAbstract
    {
        protected readonly ComponentList<TAbstract, TParent, TComponent> Components;
        protected internal override IEnumerable<TComponent> ProtectedComponents2 => Components;

        protected Container()
        {
            Components = new ComponentList<TAbstract, TParent, TComponent>(this);
        }

        protected override sealed void Link(TComponent component)
        {
            if (component.Parent == this)
                return;

            throw new ReadOnlyParentException(ReadOnlyParent.New);
        }

        protected override sealed void Unlink(TComponent component)
        {
            if (component.Parent == this)
                return;

            throw new ReadOnlyParentException(ReadOnlyParent.Previous);
        }
    }
}