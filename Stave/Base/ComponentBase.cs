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
                    _parent.HierarchyComponentsChanged -= OnParentHierarchyComponentsChanged;
                    _parent.HierarchyChanged -= OnParentHierarchyChanged;
                    _parent.Unlink(Owner);

                    HierarchyChangedEventArgs<TBase, TContainer> eventArgs = null;
                    HierarchyChangedEventArgs<TBase, TContainer> GetEventArgs()
                        => eventArgs ?? (eventArgs = HierarchyChangedEventArgs<TBase, TContainer>.Unlink(_parent, Owner));

                    HierarchyChangedT0?.Invoke(Owner, GetEventArgs());
                    HierarchyChangedT1?.Invoke(Owner, GetEventArgs());
                    HierarchyChanged?.Invoke(Owner, GetEventArgs());
                }

                _parent = value;

                if (_parent != null)
                {
                    _parent.Link(Owner);
                    _parent.HierarchyChanged += OnParentHierarchyChanged;
                    _parent.HierarchyComponentsChanged += OnParentHierarchyComponentsChanged;

                    HierarchyChangedEventArgs<TBase, TContainer> eventArgs = null;
                    HierarchyChangedEventArgs<TBase, TContainer> GetEventArgs()
                        => eventArgs ?? (eventArgs = HierarchyChangedEventArgs<TBase, TContainer>.Link(_parent, Owner));

                    HierarchyChangedT0?.Invoke(Owner, GetEventArgs());
                    HierarchyChangedT1?.Invoke(Owner, GetEventArgs());
                    HierarchyChanged?.Invoke(Owner, GetEventArgs());
                }

                ParentChangedT0?.Invoke(Owner, value);
                ParentChangedT1?.Invoke(Owner, value);
                ParentChanged?.Invoke(Owner, value);
            }
        }

        internal abstract IEnumerable<TBase> ReadOnlyBaseComponents { get; }
        IComponent IComponent.Parent => Parent;
        TBase IComponent<TBase>.Parent => Parent;
        IEnumerable IComponent.Components => ReadOnlyBaseComponents;
        IEnumerable<TBase> IComponent<TBase>.Components => ReadOnlyBaseComponents;

        public event Event<TContainer> ParentChanged;
        public event Event<IHierarchyChangedEventArgs<TBase, TContainer>> HierarchyChanged;
        public event Event<IComponentsChangedEventArgs<TBase, TContainer>> HierarchyComponentsChanged;

        private event Event<IComponent> ParentChangedT0;
        private event Event<TBase> ParentChangedT1;
        private event Event<IHierarchyChangedEventArgs> HierarchyChangedT0;
        private event Event<IHierarchyChangedEventArgs<TBase>> HierarchyChangedT1;
        private event Event<IComponentsChangedEventArgs> HierarchyComponentsChangedT0;
        private event Event<IComponentsChangedEventArgs<TBase>> HierarchyComponentsChangedT1;

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
            HierarchyChangedT0?.Invoke(Owner, e);
            HierarchyChangedT1?.Invoke(Owner, e);
            HierarchyChanged?.Invoke(Owner, e);
        }

        private void OnParentHierarchyComponentsChanged(object sender, IComponentsChangedEventArgs<TBase, TContainer> e)
        {
            RaiseHierarchyComponentsChanged(e);
        }

        protected void RaiseHierarchyComponentsChanged(IComponentsChangedEventArgs<TBase, TContainer> e)
        {
            HierarchyComponentsChangedT0?.Invoke(Owner, e);
            HierarchyComponentsChangedT1?.Invoke(Owner, e);
            HierarchyComponentsChanged?.Invoke(Owner, e);
        }

        event Event<IComponent> IComponent.ParentChanged
        {
            add => ParentChangedT0 += value;
            remove => ParentChangedT0 -= value;
        }

        event Event<IHierarchyChangedEventArgs> IComponent.HierarchyChanged
        {
            add => HierarchyChangedT0 += value;
            remove => HierarchyChangedT0 -= value;
        }

        event Event<TBase> IComponent<TBase>.ParentChanged
        {
            add => ParentChangedT1 += value;
            remove => ParentChangedT1 -= value;
        }

        event Event<IHierarchyChangedEventArgs<TBase>> IComponent<TBase>.HierarchyChanged
        {
            add => HierarchyChangedT1 += value;
            remove => HierarchyChangedT1 -= value;
        }

        event Event<IComponentsChangedEventArgs> IComponent.HierarchyComponentsChanged
        {
            add => HierarchyComponentsChangedT0 += value;
            remove => HierarchyComponentsChangedT0 -= value;
        }

        event Event<IComponentsChangedEventArgs<TBase>> IComponent<TBase>.HierarchyComponentsChanged
        {
            add => HierarchyComponentsChangedT1 += value;
            remove => HierarchyComponentsChangedT1 -= value;
        }
    }
}