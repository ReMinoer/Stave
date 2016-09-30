using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stave.Exceptions;

namespace Stave.Base
{
    public abstract class ParentBase<TAbstract, TParent, TComponent> : ComponentBase<TAbstract, TParent>, IParent<TAbstract, TParent, TComponent>, IEnumerable<TComponent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
        where TComponent : class, TAbstract
    {
        protected internal override sealed IEnumerable<TAbstract> ProtectedComponents => ProtectedComponents2;
        IEnumerable<TComponent> IParent<TAbstract, TParent, TComponent>.Components => ProtectedComponents2;
        protected internal abstract IEnumerable<TComponent> ProtectedComponents2 { get; }

        protected abstract void Link(TComponent component);
        protected abstract void Unlink(TComponent component);

        bool IParent<TAbstract, TParent, TComponent>.Link(TComponent component)
        {
            if (this.Contains(component))
                return true;

            Link(component);
            return true;
        }

        bool IParent<TAbstract, TParent, TComponent>.Unlink(TComponent component)
        {
            if (!this.Contains(component))
                return false;

            Unlink(component);
            return true;
        }

        bool IParent<TAbstract, TParent>.Link(TAbstract child)
        {
            var component = child as TComponent;
            if (component == null)
                throw new InvalidChildException("Component provided must be of type " + typeof(TComponent));

            if (this.Contains(component))
                return true;

            Link(component);
            return true;
        }

        bool IParent<TAbstract, TParent>.Unlink(TAbstract child)
        {
            var component = child as TComponent;
            if (component == null)
                throw new InvalidChildException("Component provided must be of type " + typeof(TComponent));

            if (!this.Contains(component))
                return false;

            Unlink(component);
            return true;
        }

        public IEnumerator<TComponent> GetEnumerator()
        {
            return ProtectedComponents2.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}