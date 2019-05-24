using System;
using System.Collections;
using System.Collections.Generic;

namespace Stave.Base
{
    public abstract class ComponentBase<TBase, TContainer> : IComponent<TBase, TContainer>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
    {
        protected readonly TBase Owner;

        private TContainer _parent;

        public TContainer Parent
        {
            get => _parent;
            set
            {
                if (value == _parent)
                    return;

                if (_parent != null)
                {
                    _parent.HierarchyComponentAdded -= OnParentHierarchyComponentAdded;
                    _parent.HierarchyChanged -= OnParentHierarchyChanged;
                    _parent.Unlink(Owner);
                }

                _parent = value;

                if (_parent != null)
                {
                    _parent.Link(Owner);
                    _parent.HierarchyChanged += OnParentHierarchyChanged;
                    _parent.HierarchyComponentAdded += OnParentHierarchyComponentAdded;
                }

                ParentChangedBase?.Invoke(Owner, value);
                ParentChangedBaseBis?.Invoke(Owner, value);
                ParentChanged?.Invoke(Owner, value);

                HierarchyChangedEventArgs<TBase, TContainer> eventArgs = null;
                HierarchyChangedEventArgs<TBase, TContainer> GetEventArgs()
                    => eventArgs ?? (eventArgs = new HierarchyChangedEventArgs<TBase, TContainer>(value, Owner));

                HierarchyChangedBase?.Invoke(Owner, GetEventArgs());
                HierarchyChangedBaseBis?.Invoke(Owner, GetEventArgs());
                HierarchyChanged?.Invoke(Owner, GetEventArgs());
            }
        }

        internal abstract IEnumerable<TBase> ReadOnlyBaseComponents { get; }
        IComponent IComponent.Parent => Parent;
        TBase IComponent<TBase>.Parent => Parent;
        IEnumerable IComponent.Components => ReadOnlyBaseComponents;
        IEnumerable<TBase> IComponent<TBase>.Components => ReadOnlyBaseComponents;

        public event Event<TContainer> ParentChanged;
        public event Event<IHierarchyChangedEventArgs<TBase, TContainer>> HierarchyChanged;
        public event Event<IHierarchyComponentAddedEventArgs<TBase, TContainer>> HierarchyComponentAdded;

        private event Event<IComponent> ParentChangedBase;
        private event Event<TBase> ParentChangedBaseBis;
        private event Event<IHierarchyChangedEventArgs> HierarchyChangedBase;
        private event Event<IHierarchyChangedEventArgs<TBase>> HierarchyChangedBaseBis;
        private event Event<IHierarchyComponentAddedEventArgs> HierarchyComponentAddedBase;
        private event Event<IHierarchyComponentAddedEventArgs<TBase>> HierarchyComponentAddedBaseBis;

        protected ComponentBase()
        {
            Owner = this as TBase;
            if (Owner == null)
                throw new InvalidOperationException();
        }

        protected ComponentBase(TBase owner)
        {
            Owner = owner;
        }

        private void OnParentHierarchyChanged(object sender, IHierarchyChangedEventArgs<TBase, TContainer> e)
        {
            HierarchyChangedBase?.Invoke(Owner, e);
            HierarchyChangedBaseBis?.Invoke(Owner, e);
            HierarchyChanged?.Invoke(Owner, e);
        }

        private void OnParentHierarchyComponentAdded(object sender, IHierarchyComponentAddedEventArgs<TBase, TContainer> e)
        {
            HierarchyComponentAddedBase?.Invoke(Owner, e);
            HierarchyComponentAddedBaseBis?.Invoke(Owner, e);
            HierarchyComponentAdded?.Invoke(Owner, e);
        }

        protected void OnComponentAddedBase(TContainer containerOwner, TBase addedComponent)
        {
            HierarchyComponentAddedEventArgs<TBase, TContainer> eventArgs = null;
            HierarchyComponentAddedEventArgs<TBase, TContainer> GetEventArgs()
                => eventArgs ?? (eventArgs = new HierarchyComponentAddedEventArgs<TBase, TContainer>(containerOwner, addedComponent));

            HierarchyComponentAddedBase?.Invoke(Owner, GetEventArgs());
            HierarchyComponentAddedBaseBis?.Invoke(Owner, GetEventArgs());
            HierarchyComponentAdded?.Invoke(Owner, GetEventArgs());
        }

        event Event<IComponent> IComponent.ParentChanged
        {
            add => ParentChangedBase += value;
            remove => ParentChangedBase -= value;
        }

        event Event<IHierarchyChangedEventArgs> IComponent.HierarchyChanged
        {
            add => HierarchyChangedBase += value;
            remove => HierarchyChangedBase -= value;
        }

        event Event<TBase> IComponent<TBase>.ParentChanged
        {
            add => ParentChangedBaseBis += value;
            remove => ParentChangedBaseBis -= value;
        }

        event Event<IHierarchyChangedEventArgs<TBase>> IComponent<TBase>.HierarchyChanged
        {
            add => HierarchyChangedBaseBis += value;
            remove => HierarchyChangedBaseBis -= value;
        }

        event Event<IHierarchyComponentAddedEventArgs> IComponent.HierarchyComponentAdded
        {
            add => HierarchyComponentAddedBase += value;
            remove => HierarchyComponentAddedBase -= value;
        }

        event Event<IHierarchyComponentAddedEventArgs<TBase>> IComponent<TBase>.HierarchyComponentAdded
        {
            add => HierarchyComponentAddedBaseBis += value;
            remove => HierarchyComponentAddedBaseBis -= value;
        }
    }
}