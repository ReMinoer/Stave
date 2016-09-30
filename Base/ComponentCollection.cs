using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Stave.Base
{
    public class ComponentCollection<TAbstract, TParent, TComponent> : ICollection<TComponent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
        where TComponent : class, TAbstract
    {
        protected readonly IParent<TAbstract, TParent> Owner;
        protected readonly List<TComponent> Components;
        public int Count => Components.Count;
        public bool IsReadOnly => false;

        public ComponentCollection(IParent<TAbstract, TParent> owner)
        {
            Owner = owner;
            Components = new List<TComponent>();
        }

        public void Add(TComponent item)
        {
            if (Owner == item)
                throw new InvalidOperationException("Item can't be a child of itself.");

            var itemParent = item as TParent;
            if (itemParent != null && Owner.ParentQueue().Contains(itemParent))
                throw new InvalidOperationException("Item can't be a child of this because it already exist among its parents.");

            if (!Contains(item))
                Components.Add(item);

            item.Parent = Owner as TParent;
        }

        public bool Remove(TComponent item)
        {
            if (!Components.Remove(item))
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
                Remove(Components[0]);
        }

        public bool Contains(TComponent item)
        {
            return Components.Contains(item);
        }

        public IEnumerator<TComponent> GetEnumerator()
        {
            return Components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Components).GetEnumerator();
        }

        void ICollection<TComponent>.CopyTo(TComponent[] array, int arrayIndex)
        {
            Components.CopyTo(array, arrayIndex);
        }
    }
}