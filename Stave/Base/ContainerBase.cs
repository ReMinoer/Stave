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

        public event Event<IComponentsChangedEventArgs<TBase, TContainer, TComponent>> ComponentsChanged;

        private event Event<IComponentsChangedEventArgs> ComponentsChangedT0;
        private event Event<IComponentsChangedEventArgs<TBase>> ComponentsChangedT1;
        private event Event<IComponentsChangedEventArgs<TBase, TContainer>> ComponentsChangedT2;

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

        internal void RaiseComponentsChanged(IComponentsChangedEventArgs<TBase, TContainer, TComponent> e)
        { 
            ComponentsChangedT0?.Invoke(Owner, e);
            ComponentsChangedT1?.Invoke(Owner, e);
            ComponentsChangedT2?.Invoke(Owner, e);
            ComponentsChanged?.Invoke(Owner, e);

            RaiseHierarchyComponentsChanged(e);
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

        event Event<IComponentsChangedEventArgs> IContainer.ComponentsChanged
        {
            add => ComponentsChangedT0 += value;
            remove => ComponentsChangedT0 -= value;
        }

        event Event<IComponentsChangedEventArgs<TBase>> IContainer<TBase>.ComponentsChanged
        {
            add => ComponentsChangedT1 += value;
            remove => ComponentsChangedT1 -= value;
        }

        event Event<IComponentsChangedEventArgs<TBase, TContainer>> IContainer<TBase, TContainer>.ComponentsChanged
        {
            add => ComponentsChangedT2 += value;
            remove => ComponentsChangedT2 -= value;
        }
    }
}