using System;
using System.Collections.Generic;

namespace Stave.Base
{
    public class ComponentList<TBase, TContainer, TComponent> : ComponentCollection<TBase, TContainer, TComponent>, IList<TComponent>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
        where TComponent : class, TBase
    {
        public virtual TComponent this[int index]
        {
            get => Components[index];
            set => CheckAndAdd(value, x =>
            {
                while (index >= Count)
                    Components.Add(null);
                Components[index] = x;
            });
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

        public virtual void Insert(int index, TComponent item) => CheckAndAdd(item, x => Components.Insert(index, x));

        public virtual void RemoveAt(int index)
        {
            Components.RemoveAt(index);
            Components[index].Parent = null;
        }
    }
}