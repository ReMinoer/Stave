using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Stave
{
    static public class ComponentExtensions
    {
        static public IEnumerable ChildrenQueue<TBase>(this IComponent component)
            where TBase : class, IComponent<TBase>
        {
            IEnumerable<TBase> children = Enumerable.Empty<TBase>();
            foreach (TBase child in component.Components)
            {
                yield return child;
                children = children.Concat(child.ChildrenQueue());
            }

            foreach (TBase child in children)
                yield return child;
        }

        static public IEnumerable<TBase> ChildrenQueue<TBase>(this IComponent<TBase> component)
            where TBase : class, IComponent<TBase>
        {
            IEnumerable<TBase> children = Enumerable.Empty<TBase>();
            foreach (TBase child in component.Components)
            {
                yield return child;
                children = children.Concat(child.ChildrenQueue());
            }

            foreach (TBase child in children)
                yield return child;
        }

        static public IEnumerable ParentQueue(this IComponent component)
        {
            if (component.Parent == null)
                yield break;

            yield return component.Parent;

            foreach (object parent in component.Parent.ParentQueue())
                yield return parent;
        }

        static public IEnumerable<TBase> ParentQueue<TBase>(this IComponent<TBase> component)
            where TBase : class, IComponent<TBase>
        {
            if (component.Parent == null)
                yield break;

            yield return component.Parent;

            foreach (TBase parent in component.Parent.ParentQueue())
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