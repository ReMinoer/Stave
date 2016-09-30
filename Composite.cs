using System.Collections.Generic;
using Stave.Base;

namespace Stave
{
    public class Composite<TAbstract, TParent, TComponent> : ParentBase<TAbstract, TParent, TComponent>, IComposite<TAbstract, TParent, TComponent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
        where TComponent : class, TAbstract
    {
        private readonly ComponentCollection<TAbstract, TParent, TComponent> _componentCollection;
        public IReadOnlyCollection<TComponent> ReadOnlyComponents { get; }

        protected internal override IEnumerable<TComponent> ProtectedComponents2 => ReadOnlyComponents;
        int ICollection<TComponent>.Count => _componentCollection.Count;
        bool ICollection<TComponent>.IsReadOnly => _componentCollection.IsReadOnly;

        protected Composite()
        {
            _componentCollection = new ComponentCollection<TAbstract, TParent, TComponent>(this);
            ReadOnlyComponents = new Diese.Collections.ReadOnlyCollection<TComponent>(_componentCollection);
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

        protected override sealed void Link(TComponent component)
        {
            Add(component);
        }

        protected override sealed void Unlink(TComponent component)
        {
            Remove(component);
        }

        void ICollection<TComponent>.CopyTo(TComponent[] array, int arrayIndex)
        {
            ((ICollection<TComponent>)_componentCollection).CopyTo(array, arrayIndex);
        }
    }
}