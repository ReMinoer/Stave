using System;
using System.Collections.Generic;
using System.Linq;

namespace Stave.Base
{
    public class ComponentList<TAbstract, TParent, TComponent> : ComponentCollection<TAbstract, TParent, TComponent>, IList<TComponent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
        where TComponent : class, TAbstract
    {
        public virtual TComponent this[int index]
        {
            get { return Components[index]; }
            set
            {
                if (value != null)
                {
                    if (Owner == value)
                        throw new InvalidOperationException("Item can't be a child of itself.");

                    var valueAsParent = value as TParent;
                    if (valueAsParent != null && Owner.ParentQueue().Contains(valueAsParent))
                        throw new InvalidOperationException("Item can't be a child of this because it already exist among its parents.");

                    if (!Contains(value))
                        Components.Add(value);
                }

                Components[index] = value;
            }
        }

        public ComponentList(IParent<TAbstract, TParent> owner)
            : base(owner)
        {
        }

        public int IndexOf(TComponent item)
        {
            return Components.IndexOf(item);
        }

        public virtual void Insert(int index, TComponent item)
        {
            if (Owner == item)
                throw new InvalidOperationException("Item can't be a child of itself.");

            var itemAsParent = item as TParent;
            if (itemAsParent != null && Owner.ParentQueue().Contains(itemAsParent))
                throw new InvalidOperationException("Item can't be a child of this because it is already among its parents.");

            if (!Contains(item))
                Components.Add(item);

            Components.Insert(index, item);

            if (item != null)
                item.Parent = Owner as TParent;
        }

        public virtual void RemoveAt(int index)
        {
            Components.RemoveAt(index);
            Components[index].Parent = null;
        }
    }
}