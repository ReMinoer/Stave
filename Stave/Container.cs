using System;
using System.Collections.Generic;
using Diese.Collections.Observables.ReadOnly;
using Stave.Base;

namespace Stave
{
    public class Container<TBase, TContainer, TComponent> : ContainerBase<TBase, TContainer, TComponent>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
        where TComponent : class, TBase
    {
        protected readonly ComponentList<TBase, TContainer, TComponent> Components;
        private readonly IReadOnlyCollection<TComponent> _readOnlyComponents;

        internal override bool InternalOpened => false;
        internal override IEnumerable<TComponent> ReadOnlyComponents => _readOnlyComponents;

        public Container()
        {
            Components = new ComponentList<TBase, TContainer, TComponent>(Owner, RaiseComponentsChanged);
            _readOnlyComponents = new ReadOnlyObservableCollection<TComponent>(Components);
        }

        public Container(TContainer owner)
            : base(owner)
        {
            Components = new ComponentList<TBase, TContainer, TComponent>(Owner, RaiseComponentsChanged);
            _readOnlyComponents = new ReadOnlyObservableCollection<TComponent>(Components);
        }

        protected override sealed void AddChild(TComponent component) => throw new InvalidOperationException();
        protected override sealed void RemoveChild(TComponent component) => throw new InvalidOperationException();
    }
}