using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stave.Exceptions;

namespace Stave.Base
{
    public abstract class ComponentEnumerable<TAbstract, TParent, TComponent> : ComponentBase<TAbstract, TParent>, IParent<TAbstract, TParent>, IEnumerable<TComponent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
        where TComponent : class, TAbstract
    {
        protected override sealed IEnumerable<TAbstract> ProtectedComponents => this;
        public abstract IEnumerator<TComponent> GetEnumerator();
        protected abstract void Link(TComponent component);
        protected abstract void Unlink(TComponent component);

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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}