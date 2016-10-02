using System;
using System.Collections.Generic;
using System.Linq;
using Stave.Base;
using Stave.Exceptions;

namespace Stave
{
    public class Decorator<TAbstract, TParent, TComponent> : ParentBase<TAbstract, TParent, TComponent>, IDecorator<TAbstract, TParent, TComponent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
        where TComponent : class, TAbstract
    {
        private TComponent _component;

        public TComponent Component
        {
            get { return _component; }
            set
            {
                if (_component != null)
                    throw new InvalidChildException("You must unlink a decorator before assign a new component !");

                if (value != null)
                {
                    if (this == value)
                        throw new InvalidOperationException("Item can't be a child of itself.");

                    var valueAsParent = value as TParent;
                    if (valueAsParent != null && this.ParentQueue().Contains(valueAsParent))
                        throw new InvalidOperationException("Item can't be a child of this because it already exist among its parents.");
                }

                _component = value;

                if (value != null)
                    value.Parent = this as TParent;
            }
        }

        internal override IEnumerable<TComponent> InternalComponents
        {
            get
            {
                if (Component != null)
                    yield return Component;
            }
        }

        public TComponent Unlink()
        {
            TComponent component = Component;
            Unlink(Component);
            return component;
        }

        internal override void Link(TComponent component)
        {
            Component = component;
        }

        internal override void Unlink(TComponent component)
        {
            Component = null;
        }
    }
}