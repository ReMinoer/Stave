using System.Collections.Generic;
using Diese.Collections;
using Stave.Base;

namespace Stave
{
    public class SealedOrderedComposite<TBase, TContainer, TComponent> : ContainerBase<TBase, TContainer, TComponent>, IOrderedComposite<TBase, TContainer, TComponent>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
        where TComponent : class, TBase
    {
        public ComponentList<TBase, TContainer, TComponent> Components { get; }

        private readonly IWrappedList<TComponent> _wrappedComponents;
        IWrappedList<TComponent> IOrderedComposite<TBase, TContainer, TComponent>.Components => _wrappedComponents;

        internal override bool InternalOpened => true;
        internal override IEnumerable<TComponent> ReadOnlyComponents => _wrappedComponents;
        IEnumerable<TComponent> IContainer<TBase, TContainer, TComponent>.Components => _wrappedComponents;
        IWrappedCollection<TComponent> IComposite<TBase, TContainer, TComponent>.Components => _wrappedComponents;

        TComponent IOrderedComposite<TBase, TContainer, TComponent>.this[int index]
        {
            get => Components[index];
            set => Components[index] = value;
        }

        public SealedOrderedComposite(TContainer owner)
            : base(owner)
        {
            Components = new ComponentList<TBase, TContainer, TComponent>(Owner);
            Components.ComponentAdded += OnComponentAdded;

            _wrappedComponents = new WrappedList<TComponent>(Components);
        }

        public void Add(TComponent item) => Components.Add(item);
        public void Clear() => Components.Clear();
        public bool Contains(TComponent item) => Components.Contains(item);
        public bool Remove(TComponent item) => Components.Remove(item);
        public int IndexOf(TComponent item) => Components.IndexOf(item);
        public void Insert(int index, TComponent item) => Components.Insert(index, item);
        public void RemoveAt(int index) => Components.RemoveAt(index);
        protected override void AddChild(TComponent component) => Add(component);
        protected override void RemoveChild(TComponent component) => Remove(component);
    }
}