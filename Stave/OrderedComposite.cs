using System.Collections.Generic;
using Diese.Collections;
using Stave.Base;

namespace Stave
{
    public class OrderedComposite<TBase, TContainer, TComponent> : ContainerBase<TBase, TContainer, TComponent>, IOrderedComposite<TBase, TContainer, TComponent>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
        where TComponent : class, TBase
    {
        private readonly ComponentList<TBase, TContainer, TComponent> _componentList;
        public IWrappedList<TComponent> Components { get; }

        internal override bool InternalOpened => true;
        internal override IEnumerable<TComponent> ReadOnlyComponents => Components;
        IEnumerable<TComponent> IContainer<TBase, TContainer, TComponent>.Components => Components;
        IWrappedCollection<TComponent> IComposite<TBase, TContainer, TComponent>.Components => Components;

        TComponent IOrderedComposite<TBase, TContainer, TComponent>.this[int index]
        {
            get => _componentList[index];
            set => _componentList[index] = value;
        }

        public OrderedComposite()
        {
            _componentList = new ComponentList<TBase, TContainer, TComponent>(Owner);
            Components = new WrappedList<TComponent>(_componentList);
        }

        public OrderedComposite(TContainer owner)
            : base(owner)
        {
            _componentList = new ComponentList<TBase, TContainer, TComponent>(Owner);
            Components = new WrappedList<TComponent>(_componentList);
        }

        public void Add(TComponent item) => _componentList.Add(item);
        public void Clear() => _componentList.Clear();
        public bool Contains(TComponent item) => _componentList.Contains(item);
        public bool Remove(TComponent item) => _componentList.Remove(item);
        public int IndexOf(TComponent item) => _componentList.IndexOf(item);
        public void Insert(int index, TComponent item) => _componentList.Insert(index, item);
        public void RemoveAt(int index) => _componentList.RemoveAt(index);
        protected override void AddChild(TComponent component) => Add(component);
        protected override void RemoveChild(TComponent component) => Remove(component);
    }
}