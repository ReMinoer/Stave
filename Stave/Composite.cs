using System.Collections.Generic;
using Diese.Collections;
using Stave.Base;

namespace Stave
{
    public class Composite<TAbstract, TParent, TComponent> : ParentBase<TAbstract, TParent, TComponent>, IComposite<TAbstract, TParent, TComponent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
        where TComponent : class, TAbstract
    {
        private readonly ComponentCollection<TAbstract, TParent, TComponent> _componentCollection;
        public IWrappedCollection<TComponent> Components { get; }
        internal override IEnumerable<TComponent> InternalComponents => Components;

        protected Composite()
        {
            _componentCollection = new ComponentCollection<TAbstract, TParent, TComponent>(this);
            Components = new WrappedCollection<TComponent>(_componentCollection);
        }

        public virtual void Add(TComponent item)
        {
            _componentCollection.Add(item);
        }

        public virtual bool Remove(TComponent item)
        {
            return _componentCollection.Remove(item);
        }

        public virtual void Clear()
        {
            _componentCollection.Clear();
        }

        public bool Contains(TComponent item)
        {
            return _componentCollection.Contains(item);
        }

        internal override sealed void Link(TComponent component)
        {
            Add(component);
        }

        internal override sealed void Unlink(TComponent component)
        {
            Remove(component);
        }
    }
}