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

                _parent?.Unlink(Owner);
                _parent = value;
                _parent?.Link(Owner);
            }
        }
        
        internal abstract IEnumerable<TBase> ReadOnlyBaseComponents { get; }
        IComponent IComponent.Parent => Parent;
        TBase IComponent<TBase>.Parent => Parent;
        IEnumerable IComponent.Components => ReadOnlyBaseComponents;
        IEnumerable<TBase> IComponent<TBase>.Components => ReadOnlyBaseComponents;

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
    }
}