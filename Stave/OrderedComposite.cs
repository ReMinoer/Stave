using System.Collections.Generic;
using Diese.Collections;
using Stave.Base;

namespace Stave
{
    public class OrderedComposite<TAbstract, TParent, TComponent> : ParentBase<TAbstract, TParent, TComponent>, IOrderedComposite<TAbstract, TParent, TComponent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
        where TComponent : class, TAbstract
    {
        private readonly ComponentList<TAbstract, TParent, TComponent> _componentList;
        public IWrappedList<TComponent> Components { get; }
        internal override IEnumerable<TComponent> InternalComponents => Components;
        IEnumerable<TComponent> IParent<TAbstract, TParent, TComponent>.Components => Components;
        IWrappedCollection<TComponent> IComposite<TAbstract, TParent, TComponent>.Components => Components;

        TComponent IOrderedComposite<TAbstract, TParent, TComponent>.this[int index]
        {
            get { return _componentList[index]; }
            set { _componentList[index] = value; }
        }

        public OrderedComposite()
        {
            _componentList = new ComponentList<TAbstract, TParent, TComponent>(this);
            Components = new WrappedList<TComponent>(_componentList);
        }

        public void Add(TComponent item)
        {
            _componentList.Add(item);
        }

        public void Clear()
        {
            _componentList.Clear();
        }

        public bool Contains(TComponent item)
        {
            return _componentList.Contains(item);
        }

        public bool Remove(TComponent item)
        {
            return _componentList.Remove(item);
        }

        public int IndexOf(TComponent item)
        {
            return _componentList.IndexOf(item);
        }

        public void Insert(int index, TComponent item)
        {
            _componentList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _componentList.RemoveAt(index);
        }

        internal override sealed void Link(TComponent component)
        {
            Add(component);
        }

        internal override void Unlink(TComponent component)
        {
            Remove(component);
        }
    }
}