using System;

namespace Stave
{
    public class OrderedComposite<TAbstract, TParent, TComponent> : Composite<TAbstract, TParent, TComponent>, IOrderedComposite<TAbstract, TParent, TComponent>
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
                    if (Equals(value))
                        throw new InvalidOperationException("Item can't be a child of itself.");
                    if (ContainsAmongParents(value))
                        throw new InvalidOperationException("Item can't be a child of this because it already exist among its parents.");

                    if (!Contains(value))
                        Components.Add(value);
                }

                Components[index] = value;
            }
        }

        public int IndexOf(TComponent item)
        {
            return Components.IndexOf(item);
        }

        public virtual void Insert(int index, TComponent item)
        {
            if (Equals(item))
                throw new InvalidOperationException("Item can't be a child of itself.");
            if (ContainsAmongParents(item))
                throw new InvalidOperationException("Item can't be a child of this because it is already among its parents.");

            if (!Contains(item))
                Components.Add(item);

            Components.Insert(index, item);

            if (item != null)
                item.Parent = this as TParent;
        }

        public virtual void RemoveAt(int index)
        {
            Components.RemoveAt(index);
            Components[index].Parent = null;
        }
    }
}