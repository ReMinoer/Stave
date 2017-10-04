using System.Collections.Generic;
using Diese.Collections;
using Stave.Base;

namespace Stave
{
    public class Composite<TBase, TContainer, TComponent> : ContainerBase<TBase, TContainer, TComponent>, IComposite<TBase, TContainer, TComponent>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
        where TComponent : class, TBase
    {
        private readonly ComponentCollection<TBase, TContainer, TComponent> _componentCollection;
        public IWrappedCollection<TComponent> Components { get; }

        internal override bool InternalOpened => true;
        internal override IEnumerable<TComponent> ReadOnlyComponents => Components;

        public Composite()
        {
            _componentCollection = new ComponentCollection<TBase, TContainer, TComponent>(Owner);
            Components = new WrappedCollection<TComponent>(_componentCollection);
        }

        public Composite(TContainer owner)
            : base(owner)
        {
            _componentCollection = new ComponentCollection<TBase, TContainer, TComponent>(Owner);
            Components = new WrappedCollection<TComponent>(_componentCollection);
        }

        public virtual void Add(TComponent item) => _componentCollection.Add(item);
        public virtual bool Remove(TComponent item) => _componentCollection.Remove(item);
        public virtual void Clear() => _componentCollection.Clear();
        public bool Contains(TComponent item) => _componentCollection.Contains(item);
        protected override sealed void AddChild(TComponent component) => Add(component);
        protected override sealed void RemoveChild(TComponent component) => Remove(component);
    }
}