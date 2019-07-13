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

        public ComponentList(TContainer owner)
            : base(owner)
        {
        }

        public int IndexOf(TComponent item)
        {
            if (item == null)
                throw new ArgumentNullException();

            return Components.IndexOf(item);
        }

        public virtual void Insert(int index, TComponent item)
            => CheckAndInsert(item,
                () => Components.Insert(index, item),
                () => CollectionChangedEventArgs.Insert(item, index));

        public virtual void RemoveAt(int index)
        {
            Components.RemoveAt(index);
            Components[index].Parent = null;
        }
    }
}