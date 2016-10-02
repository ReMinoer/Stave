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
        internal override IEnumerable<TComponent> InternalComponents => Components;

        protected Container()
        {
            Components = new ComponentList<TAbstract, TParent, TComponent>(this);
        }

        internal override sealed void Link(TComponent component)
        {
            if (component.Parent == this)
                return;

            throw new ReadOnlyParentException(ReadOnlyParent.New);
        }

        internal override sealed void Unlink(TComponent component)
        {
            if (component.Parent == this)
                return;

            throw new ReadOnlyParentException(ReadOnlyParent.Previous);
        }
    }
}