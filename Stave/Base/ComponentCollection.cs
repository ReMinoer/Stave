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
        protected readonly TContainer _owner;
        protected readonly Action<IComponentsChangedEventArgs<TBase, TContainer, TComponent>> _raiseComponentsChanged;

        protected readonly List<TComponent> Components;
        public int Count => Components.Count;
        
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public ComponentCollection(TContainer owner, Action<IComponentsChangedEventArgs<TBase, TContainer, TComponent>> raiseComponentsChanged)
        {
            _owner = owner;
            _raiseComponentsChanged = raiseComponentsChanged;

            Components = new List<TComponent>();
        }

        public void Add(TComponent item)
        {
            CheckAndInsert(item,
                () => Components.Add(item),
                () => CollectionChangedEventArgs.Add(item, Count));
        }

        protected void CheckAndInsert(TComponent item,
            Action addAction,
            Func<NotifyCollectionChangedEventArgs> collectionEventArgsFunc)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (item == _owner)
                throw new InvalidOperationException("Item can't be a child of itself.");

            if (Contains(item))
                return;

            if (((IComponent<TBase>)_owner).AllParents().Contains(item))
                throw new InvalidOperationException("Item can't be a child of this because it already exist among its parents.");

            addAction();
            item.Parent = _owner;

            NotifyPropertyChanged(collectionEventArgsFunc());
            _raiseComponentsChanged(ComponentsChangedEventArgs<TBase, TContainer, TComponent>.Add(_owner, item));
        }

        public bool Remove(TComponent item)
        {
            if (item == null)
                throw new ArgumentNullException();

            int index = Components.IndexOf(item);
            if (index == -1)
                return false;

            Remove(item, index);
            return true;
        }

        protected void Remove(TComponent item, int index)
        {
            Components.RemoveAt(index);
            item.Parent = null;

            NotifyPropertyChanged(CollectionChangedEventArgs.Remove(item, index));
            _raiseComponentsChanged(ComponentsChangedEventArgs<TBase, TContainer, TComponent>.Remove(_owner, item));
        }

        public void Clear()
        {
            var removedComponents = new List<TComponent>();

            int count = Count;
            for (int i = 0; i < count; i++)
            {
                TComponent component = Components[0];
                Components.RemoveAt(0);
                component.Parent = null;

                removedComponents.Add(component);
            }

            NotifyPropertyChanged(CollectionChangedEventArgs.Clear());

            foreach (TComponent removedComponent in removedComponents)
                _raiseComponentsChanged(ComponentsChangedEventArgs<TBase, TContainer, TComponent>.Remove(_owner, removedComponent));
        }

        public bool Contains(TComponent item)
        {
            if (item == null)
                throw new ArgumentNullException();

            return Components.Contains(item);
        }

        protected void NotifyPropertyChanged(NotifyCollectionChangedEventArgs eventArgs) => CollectionChanged?.Invoke(this, eventArgs);
        
        bool ICollection<TComponent>.IsReadOnly => false;
        void ICollection<TComponent>.CopyTo(TComponent[] array, int arrayIndex) => throw new InvalidOperationException();
        public IEnumerator<TComponent> GetEnumerator() => Components.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Components).GetEnumerator();
    }
}