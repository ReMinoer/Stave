using System.Collections;
using System.Collections.Generic;

namespace Stave
{
    public interface IComponent
    {
        IComponent Parent { get; }
        IEnumerable Components { get; }
    }

    public interface IComponent<out TBase> : IComponent
        where TBase : class, IComponent<TBase>
    {
        new TBase Parent { get; }
        new IEnumerable<TBase> Components { get; }
    }

    public interface IComponent<out TBase, TContainer> : IComponent<TBase>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
    {
        new TContainer Parent { get; set; }
    }
}