using System;
using System.Collections.Generic;
using System.Linq;
using Stave.Base;
using Stave.Exceptions;

namespace Stave
{
    public class Decorator<TBase, TContainer, TComponent> : ContainerBase<TBase, TContainer, TComponent>, IDecorator<TBase, TContainer, TComponent>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
        where TComponent : class, TBase
    {
        private TComponent _component;

        public TComponent Component
        {
            get => _component;
            set
            {
                if (_component != null)
                    throw new InvalidChildException("You must unlink a decorator before assign a new component !");

                if (value == null)
                    return;

                if (value == Owner)
                    throw new InvalidOperationException("Item can't be a child of itself.");

                if (((IComponent<TBase>)Owner).AllParents().Contains(value))
                    throw new InvalidOperationException("Item can't be a child of this because it already exist among its parents.");

                _component = value;
                _component.Parent = Owner;
                OnComponentAdded(this, _component);
            }
        }

        internal override bool InternalOpened => true;

        internal override IEnumerable<TComponent> ReadOnlyComponents
        {
            get
            {
                if (Component != null)
                    yield return Component;
            }
        }

        public Decorator()
        {
        }

        public Decorator(TContainer owner)
            : base(owner)
        {
        }

        public TComponent Unlink()
        {
            TComponent component = Component;
            RemoveChild(Component);
            return component;
        }

        protected override sealed void AddChild(TComponent component)
        {
            Component = component;
        }

        protected override sealed void RemoveChild(TComponent component)
        {
            Component = null;
        }
    }
}