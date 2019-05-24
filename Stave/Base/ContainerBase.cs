using System;
using System.Collections.Generic;
using System.Linq;
using Stave.Exceptions;

namespace Stave.Base
{
    public abstract class ContainerBase<TBase, TContainer, TComponent> : ComponentBase<TBase, TContainer>, IContainer<TBase, TContainer, TComponent>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
        where TComponent : class, TBase
    {
        new protected readonly TContainer Owner;

        bool IContainer.Opened => InternalOpened;
        internal abstract bool InternalOpened { get; }

        internal abstract IEnumerable<TComponent> ReadOnlyComponents { get; }
        internal override sealed IEnumerable<TBase> ReadOnlyBaseComponents => ReadOnlyComponents;
        IEnumerable<TComponent> IContainer<TBase, TContainer, TComponent>.Components => ReadOnlyComponents;

        public event Event<TComponent> ComponentAdded;

        private event Event<IComponent> ComponentAddedBase;
        private event Event<TBase> ComponentAddedBaseBis;

        protected ContainerBase()
        {
            Owner = this as TContainer;
            if (Owner == null)
                throw new InvalidOperationException();
        }

        protected ContainerBase(TContainer owner)
            : base(owner)
        {
            Owner = owner;
        }

        private void Link(TComponent child) 
        {
            if (child == null)
                throw new NullReferenceException();

            if (ReadOnlyComponents.Contains(child))
                return;

            if (!InternalOpened)
                throw new ReadOnlyParentException(ReadOnlyParent.New);

            AddChild(child);
        }

        private void Unlink(TComponent child)
        {
            if (child == null)
                throw new NullReferenceException();

            if (!InternalOpened)
                throw new ReadOnlyParentException(ReadOnlyParent.Previous);

            if (!ReadOnlyComponents.Contains(child))
                return;

            RemoveChild(child);
        }

        private bool TryLink(TComponent child)
        {
            if (child == null)
                return false;

            if (!InternalOpened)
                return false;

            if (ReadOnlyComponents.Contains(child))
                return true;

            AddChild(child);
            return true;
        }

        private bool TryUnlink(TComponent child)
        {
            if (child == null)
                return false;

            if (!InternalOpened)
                return false;

            if (!ReadOnlyComponents.Contains(child))
                return true;

            RemoveChild(child);
            return true;
        }

        protected void OnComponentAdded(object sender, TComponent e)
        {
            ComponentAddedBase?.Invoke(Owner, e);
            ComponentAddedBaseBis?.Invoke(Owner, e);
            ComponentAdded?.Invoke(Owner, e);

            OnComponentAddedBase(Owner, e);
        }

        protected abstract void AddChild(TComponent child);
        protected abstract void RemoveChild(TComponent child);

        void IContainer<TBase>.Link(TBase child)
        {
            if (!(child is TComponent component))
                throw new InvalidChildException("Component provided must be of type " + typeof(TComponent));

            Link(component);
        }

        void IContainer<TBase>.Unlink(TBase child)
        {
            if (!(child is TComponent component))
                throw new InvalidChildException("Component provided must be of type " + typeof(TComponent));

            Unlink(component);
        }

        void IContainer<TBase, TContainer, TComponent>.Unlink(TComponent child) => Unlink(child);
        void IContainer<TBase, TContainer, TComponent>.Link(TComponent child) => Link(child);

        bool IContainer<TBase>.TryLink(TBase child) => child is TComponent component && TryLink(component);
        bool IContainer<TBase>.TryUnlink(TBase child) => child is TComponent component && TryUnlink(component);
        bool IContainer<TBase, TContainer, TComponent>.TryLink(TComponent child) => TryLink(child);
        bool IContainer<TBase, TContainer, TComponent>.TryUnlink(TComponent child) => TryUnlink(child);

        event Event<IComponent> IContainer.ComponentAdded
        {
            add => ComponentAddedBase += value;
            remove => ComponentAddedBase -= value;
        }

        event Event<TBase> IContainer<TBase>.ComponentAdded
        {
            add => ComponentAddedBaseBis += value;
            remove => ComponentAddedBaseBis -= value;
        }
    }
}