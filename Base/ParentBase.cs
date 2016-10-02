using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stave.Exceptions;

namespace Stave.Base
{
    public abstract class ParentBase<TAbstract, TParent, TComponent> : ComponentBase<TAbstract, TParent>, IParent<TAbstract, TParent, TComponent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
        where TComponent : class, TAbstract
    {
        internal override sealed IEnumerable<TAbstract> InternalAbstracts => InternalComponents;
        IEnumerable<TComponent> IParent<TAbstract, TParent, TComponent>.Components => InternalComponents;
        internal abstract IEnumerable<TComponent> InternalComponents { get; }

        internal abstract void Link(TComponent component);
        internal abstract void Unlink(TComponent component);

        bool IParent<TAbstract, TParent, TComponent>.Link(TComponent component)
        {
            if (InternalComponents.Contains(component))
                return true;

            Link(component);
            return true;
        }

        bool IParent<TAbstract, TParent, TComponent>.Unlink(TComponent component)
        {
            if (!InternalComponents.Contains(component))
                return false;

            Unlink(component);
            return true;
        }

        bool IParent<TAbstract, TParent>.Link(TAbstract child)
        {
            var component = child as TComponent;
            if (component == null)
                throw new InvalidChildException("Component provided must be of type " + typeof(TComponent));

            if (InternalComponents.Contains(component))
                return true;

            Link(component);
            return true;
        }

        bool IParent<TAbstract, TParent>.Unlink(TAbstract child)
        {
            var component = child as TComponent;
            if (component == null)
                throw new InvalidChildException("Component provided must be of type " + typeof(TComponent));

            if (!InternalComponents.Contains(component))
                return false;

            Unlink(component);
            return true;
        }
    }
}