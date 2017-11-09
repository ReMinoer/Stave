using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Stave
{
    static public class ComponentExtensions
    {
        static public IEnumerable<TBase> ChildrenQueue<TBase>(this IComponent<TBase> component)
            where TBase : class, IComponent<TBase>
        {
            foreach (TBase child in component.Components)
                yield return child;

            foreach (TBase child in component.Components.SelectMany(x => x.ChildrenQueue()))
                yield return child;
        }

        static public IEnumerable ParentQueue(this IComponent component)
        {
            for (IComponent parent = component.Parent; parent != null; parent = parent.Parent)
                yield return parent;
        }

        static public IEnumerable<TBase> ParentQueue<TBase>(this IComponent<TBase> component)
            where TBase : class, IComponent<TBase>
        {
            for (TBase parent = component.Parent; parent != null; parent = parent.Parent)
                yield return parent;
        }

        static public IEnumerable<TContainer> ParentQueue<TBase, TContainer>(this IComponent<TBase, TContainer> component)
            where TBase : class, IComponent<TBase, TContainer>
            where TContainer : class, TBase, IContainer<TBase, TContainer>
        {
            if (component.Parent == null)
                yield break;

            yield return component.Parent;

            foreach (TContainer parent in component.Parent.ParentQueue())
                yield return parent;
        }
    }
}