using System.Collections;
using System.Collections.Generic;
using Diese.Collections;

namespace Stave
{
    static public class ComponentExtensions
    {
        static public IEnumerable<TBase> ChildrenQueue<TBase>(this IComponent<TBase> component)
            where TBase : class, IComponent<TBase>
        {
            return Tree.BreadthFirstExclusive(component, x => x.Components);
        }

        static public IEnumerable ParentQueue(this IComponent component)
        {
            return Sequence.AggregateExclusive(component, x => x.Parent);
        }

        static public IEnumerable<TBase> ParentQueue<TBase>(this IComponent<TBase> component)
            where TBase : class, IComponent<TBase>
        {
            return Sequence.AggregateExclusive(component, x => x.Parent);
        }

        static public IEnumerable<TContainer> ParentQueue<TBase, TContainer>(this IComponent<TBase, TContainer> component)
            where TBase : class, IComponent<TBase, TContainer>
            where TContainer : class, TBase, IContainer<TBase, TContainer>
        {
            return Sequence.AggregateExclusive(component, x => x.Parent);
        }
    }
}