using System;
using System.Collections.Generic;
using Stave.Base;

namespace Stave
{
    public class OpaqueContainer<TBase, TContainer, TComponent> : ContainerBase<TBase, TContainer, TComponent>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
        where TComponent : class, TBase
    {
        protected readonly ComponentList<TBase, TContainer, TComponent> Components;

        internal override bool InternalOpened => false;
        internal override IEnumerable<TComponent> ReadOnlyComponents
        {
            get { yield break; }
        }

        public OpaqueContainer()
        {
            Components = new ComponentList<TBase, TContainer, TComponent>(Owner);
        }

        public OpaqueContainer(TContainer owner)
            : base(owner)
        {
            Components = new ComponentList<TBase, TContainer, TComponent>(Owner);
        }

        protected override sealed void AddChild(TComponent component) => throw new InvalidOperationException();
        protected override sealed void RemoveChild(TComponent component) => throw new InvalidOperationException();
    }
}