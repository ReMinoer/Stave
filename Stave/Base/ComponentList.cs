using System;
using System.Linq;
using Diese.Collections.Observables;

namespace Stave.Base
{
    public class ComponentList<TBase, TContainer, TComponent> : ComponentCollection<TBase, TContainer, TComponent>, IObservableList<TComponent>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
        where TComponent : class, TBase
    {
        public virtual TComponent this[int index]
        {
            get => Components[index];
            set
            {
                int oldCount = Count;
                CheckAndInsert(value,
                    () =>
                    {
                        while (index >= Count)
                            Components.Add(null);
                        Components[index] = value;
                    },
                    () => CollectionChangedEventArgs.InsertRange(
                        Components.Skip(index).Take(Math.Min(1, Count - oldCount)).ToArray(),
                        Math.Min(index, oldCount)));
            }
        }

        public ComponentList(TContainer owner, Action<IComponentsChangedEventArgs<TBase, TContainer, TComponent>> raiseComponentsChanged)
            : base(owner, raiseComponentsChanged)
        {
        }

        public int IndexOf(TComponent item)
        {
            if (item == null)
                throw new ArgumentNullException();

            return Components.IndexOf(item);
        }

        public virtual void Insert(int index, TComponent item)
        {
            CheckAndInsert(
                item,
                () =>
                {
                    if (index == Count)
                        Components.Add(item);
                    else
                        Components.Insert(index, item);
                },
                () => CollectionChangedEventArgs.Insert(item, index)
            );
        }

        public virtual void RemoveAt(int index)
        {
            TComponent item = Components[index];
            Remove(item, index);
        }

        public void Move(int oldIndex, int newIndex)
        {
            TComponent item = Components[oldIndex];

            Components.RemoveAt(oldIndex);
            Components.Insert(newIndex, item);

            NotifyPropertyChanged(CollectionChangedEventArgs.Move(item, oldIndex, newIndex));
            _raiseComponentsChanged(ComponentsChangedEventArgs<TBase, TContainer, TComponent>.Remove(_owner, item));
            _raiseComponentsChanged(ComponentsChangedEventArgs<TBase, TContainer, TComponent>.Add(_owner, item));
        }
    }
}