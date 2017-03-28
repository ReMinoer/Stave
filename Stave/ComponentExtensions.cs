using System.Collections.Generic;
using System.Linq;

namespace Stave
{
    static public class ComponentExtensions
    {
        static public IEnumerable<TAbstract> ChildrenQueue<TAbstract, TParent>(this IComponent<TAbstract, TParent> component)
            where TAbstract : class, IComponent<TAbstract, TParent>
            where TParent : class, TAbstract, IParent<TAbstract, TParent>
        {
            IEnumerable<TAbstract> children = Enumerable.Empty<TAbstract>();
            foreach (TAbstract child in component.Components)
            {
                yield return child;
                children = children.Concat(child.ChildrenQueue());
            }

            foreach (TAbstract child in children)
                yield return child;
        }

        static public IEnumerable<TParent> ParentQueue<TAbstract, TParent>(this IComponent<TAbstract, TParent> component)
            where TAbstract : class, IComponent<TAbstract, TParent>
            where TParent : class, TAbstract, IParent<TAbstract, TParent>
        {
            if (component.Parent == null)
                yield break;

            yield return component.Parent;

            foreach (TParent parent in component.Parent.ParentQueue())
                yield return parent;
        }
    }
}