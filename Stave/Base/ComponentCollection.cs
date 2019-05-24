using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Stave.Base
{
    public class ComponentCollection<TBase, TContainer, TComponent> : ICollection<TComponent>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
        where TComponent : class, TBase
    {
        protected readonly TContainer Owner;

        protected readonly List<TComponent> Components;
        public int Count => Components.Count;
        public bool IsReadOnly => false;

        public event EventHandler<TComponent> ComponentAdded;

        public ComponentCollection(TContainer owner)
        {
            Owner = owner;
            Components = new List<TComponent>();
        }

        public void Add(TComponent item) => CheckAndAdd(item, Components.Add);

        protected void CheckAndAdd(TComponent item, Action<TComponent> addAction)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (item == Owner)
                throw new InvalidOperationException("Item can't be a child of itself.");

            if (Contains(item))
                return;

            if (((IComponent<TBase>)Owner).ParentQueue().Contains(item))
                throw new InvalidOperationException("Item can't be a child of this because it already exist among its parents.");

            addAction(item);
            item.Parent = Owner;

            ComponentAdded?.Invoke(this, item);
        }

        public bool Remove(TComponent item)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (!Components.Remove(item))
                return false;

            item.Parent = null;
            return true;
        }

        public void Clear()
        {
            for (int i = Count; i > 0; i--)
            {
                TComponent item = Components[0];
                Components.Remove(item);
                item.Parent = null;
            }
        }

        public bool Contains(TComponent item)
        {
            if (item == null)
                throw new ArgumentNullException();

            return Components.Contains(item);
        }

        public IEnumerator<TComponent> GetEnumerator() => Components.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Components).GetEnumerator();
        void ICollection<TComponent>.CopyTo(TComponent[] array, int arrayIndex) => throw new InvalidOperationException();
    }
}