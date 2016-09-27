using System;
using System.Collections;
using System.Collections.Generic;
using Stave.Exceptions;

namespace Stave.Base
{
    public class ComponentCollection<TAbstract, TParent, TComponent> : IList<TComponent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
        where TComponent : class, TAbstract
    {
        private readonly IParent<TAbstract, TParent> _owner;
        private readonly List<TComponent> _components;
        public int Count => _components.Count;
        public bool IsReadOnly => false;

        public ComponentCollection(IParent<TAbstract, TParent> owner)
        {
            _owner = owner;
            _components = new List<TComponent>();
        }

        public TComponent this[int index]
        {
            get { return _components[index]; }
            set { _components[index] = value; }
        }

        public void Add(TComponent item)
        {
            if (Equals(item))
                throw new InvalidOperationException("Item can't be a child of itself.");
            if (_owner.ContainsAmongParents(item))
                throw new InvalidOperationException("Item can't be a child of this because it already exist among its parents.");

            if (!Contains(item))
                _components.Add(item);

            item.Parent = _owner as TParent;
        }

        public bool Remove(TComponent item)
        {
            if (!_components.Remove(item))
                return false;

            item.Parent = null;
            return true;
        }

        public void Replace(ref TComponent reference, TComponent newItem)
        {
            if (reference == newItem)
                return;

            if (reference != null)
                Remove(reference);

            reference = newItem;

            if (reference != null)
                Add(reference);
        }

        public void Clear()
        {
            for (int i = Count; i > 0; i--)
                Remove(_components[0]);
        }

        public bool Contains(TComponent item)
        {
            return _components.Contains(item);
        }

        public int IndexOf(TComponent item)
        {
            return _components.IndexOf(item);
        }

        public void Insert(int index, TComponent item)
        {
            _components.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _components.RemoveAt(index);
        }

        public IEnumerator<TComponent> GetEnumerator()
        {
            return _components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_components).GetEnumerator();
        }

        void ICollection<TComponent>.CopyTo(TComponent[] array, int arrayIndex)
        {
            _components.CopyTo(array, arrayIndex);
        }
    }
}