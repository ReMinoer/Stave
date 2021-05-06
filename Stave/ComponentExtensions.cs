using System.Collections.Generic;
using System.Linq;
using Diese.Collections;

namespace Stave
{
    static public class ComponentExtensions
    {
        static public IEnumerable<TBase> AllChildren<TBase>(this IComponent<TBase> component)
            where TBase : class, IComponent<TBase>
        {
            if (component == null)
                return Enumerable.Empty<TBase>();

            return Tree.BreadthFirstExclusive(component, x => x.Components);
        }

        static public IEnumerable<TBase> AndAllChildren<TBase>(this TBase component)
            where TBase : class, IComponent<TBase>
        {
            if (component == null)
                return Enumerable.Empty<TBase>();

            return Tree.BreadthFirst(component, x => x.Components);
        }

        static public IEnumerable<TBase> AllParents<TBase>(this IComponent<TBase> component)
            where TBase : class, IComponent<TBase>
        {
            if (component == null)
                return Enumerable.Empty<TBase>();

            return Sequence.AggregateExclusive(component, x => x.Parent);
        }

        static public IEnumerable<TBase> AndAllParents<TBase>(this TBase component)
            where TBase : class, IComponent<TBase>
        {
            if (component == null)
                return Enumerable.Empty<TBase>();

            return Sequence.Aggregate(component, x => x.Parent);
        }

        static public IEnumerable<TContainer> AllParents<TBase, TContainer>(this IComponent<TBase, TContainer> component)
            where TBase : class, IComponent<TBase, TContainer>
            where TContainer : class, TBase, IContainer<TBase, TContainer>
        {
            if (component == null)
                return Enumerable.Empty<TContainer>();

            return Sequence.AggregateExclusive(component, x => x.Parent);
        }

        static public IEnumerable<TContainer> AndAllParents<TBase, TContainer>(this TContainer component)
            where TBase : class, IComponent<TBase, TContainer>
            where TContainer : class, TBase, IContainer<TBase, TContainer>
        {
            if (component == null)
                return Enumerable.Empty<TContainer>();

            return Sequence.Aggregate(component, x => x.Parent);
        }
    }
}