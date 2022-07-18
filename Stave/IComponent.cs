using System.Collections;
using System.Collections.Generic;

namespace Stave
{
    public interface IComponent
    {
        IComponent Parent { get; }
        IEnumerable Components { get; }
        event Event<IComponent> ParentChanged;
        event Event<IHierarchyChangedEventArgs> HierarchyChanged;
        event Event<IComponentsChangedEventArgs> HierarchyComponentsChanged;
    }

    public interface IComponent<out TBase> : IComponent
        where TBase : class, IComponent<TBase>
    {
        new TBase Parent { get; }
        new IEnumerable<TBase> Components { get; }
        new event Event<TBase> ParentChanged;
        new event Event<IHierarchyChangedEventArgs<TBase>> HierarchyChanged;
        new event Event<IComponentsChangedEventArgs<TBase>> HierarchyComponentsChanged;
    }

    public interface IComponent<out TBase, TContainer> : IComponent<TBase>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
    {
        new TContainer Parent { get; set; }
        new event Event<TContainer> ParentChanged;
        new event Event<IHierarchyChangedEventArgs<TBase, TContainer>> HierarchyChanged;
        new event Event<IComponentsChangedEventArgs<TBase, TContainer>> HierarchyComponentsChanged;
    }
}