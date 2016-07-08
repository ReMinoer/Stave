using System.Collections.Generic;
using Stave.Base;

namespace Stave
{
    public class Composite<TAbstract, TParent, TComponent> : ComponentEnumerable<TAbstract, TParent, TComponent>, IComposite<TAbstract, TParent, TComponent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
        where TComponent : class, TAbstract
    {
        protected readonly ComponentCollection<TAbstract, TParent, TComponent> Components;
        public int Count => Components.Count;
        public bool IsReadOnly => Components.IsReadOnly;

        protected Composite()
        {
            Components = new ComponentCollection<TAbstract, TParent, TComponent>(this);
        }

        public virtual void Add(TComponent item)
        {
            Components.Add(item);
        }

        public virtual void Remove(TComponent item)
        {
            Components.Remove(item);
        }

        public virtual void Clear()
        {
            Components.Clear();
        }

        public bool Contains(TComponent item)
        {
            return Components.Contains(item);
        }

        protected override sealed void Link(TComponent component)
        {
            Components.Add(component);
        }

        protected override sealed void Unlink(TComponent component)
        {
            Components.Remove(component);
        }

        public override IEnumerator<TComponent> GetEnumerator()
        {
            return Components.GetEnumerator();
        }

        void ICollection<TComponent>.CopyTo(TComponent[] array, int arrayIndex)
        {
            ((ICollection<TComponent>)Components).CopyTo(array, arrayIndex);
        }

        bool ICollection<TComponent>.Remove(TComponent item)
        {
            return ((ICollection<TComponent>)Components).Remove(item);
        }
    }
}