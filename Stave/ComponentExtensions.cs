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
            TAbstract[] components = component.Components.ToArray();
            if (components.Length == 0)
                yield break;

            foreach (TAbstract child in components)
                yield return child;

            foreach (TAbstract child in components.SelectMany(x => x.ChildrenQueue()))
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