using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Diese.Collections.Observables;

namespace Stave.Base
{
    public class ComponentCollection<TBase, TContainer, TComponent> : IObservableCollection<TComponent>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
        where TComponent : class, TBase
    {
        protected readonly TContainer Owner;

        protected readonly List<TComponent> Components;
        public int Count => Components.Count;

        public event EventHandler<TComponent> ComponentAdded;
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public ComponentCollection(TContainer owner)
        {
            Owner = owner;
            Components = new List<TComponent>();
        }

        public void Add(TComponent item) => CheckAndInsert(item, () => Components.Add(item), () => CollectionChangedEventArgs.Add(item, Count));

        protected void CheckAndInsert(TComponent item, Action addAction, Func<NotifyCollectionChangedEventArgs> eventArgsFunc)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (item == Owner)
                throw new InvalidOperationException("Item can't be a child of itself.");

            if (Contains(item))
                return;

            if (((IComponent<TBase>)Owner).ParentQueue().Contains(item))
                throw new InvalidOperationException("Item can't be a child of this because it already exist among its parents.");

            addAction();
            item.Parent = Owner;

            ComponentAdded?.Invoke(this, item);
            CollectionChanged?.Invoke(this, eventArgsFunc());
        }

        public bool Remove(TComponent item)
        {
            if (item == null)
                throw new ArgumentNullException();

            int index = Components.IndexOf(item);
            if (index == -1)
                return false;

            Components.RemoveAt(index);
            item.Parent = null;

            CollectionChanged?.Invoke(this, CollectionChangedEventArgs.Remove(item, index));
            return true;
        }

        public void Clear()
        {
            int count = Count;
            for (int i = 0; i < count; i++)
            {
                TComponent component = Components[0];
                Components.RemoveAt(0);
                component.Parent = null;
            }

            CollectionChanged?.Invoke(this, CollectionChangedEventArgs.Clear());
        }

        public bool Contains(TComponent item)
        {
            if (item == null)
                throw new ArgumentNullException();

            return Components.Contains(item);
        }
        
        bool ICollection<TComponent>.IsReadOnly => false;
        void ICollection<TComponent>.CopyTo(TComponent[] array, int arrayIndex) => throw new InvalidOperationException();
        public IEnumerator<TComponent> GetEnumerator() => Components.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Components).GetEnumerator();
    }
}