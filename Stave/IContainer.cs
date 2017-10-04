using System.Collections.Generic;

namespace Stave
{
    public interface IContainer<TBase> : IComponent<TBase>
        where TBase : class, IComponent<TBase>
    {
        bool Opened { get; }
        void Link(TBase child);
        void Unlink(TBase child);
        bool TryLink(TBase child);
        bool TryUnlink(TBase child);
    }

    public interface IContainer<TBase, TContainer> : IContainer<TBase>, IComponent<TBase, TContainer>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
    {
    }

    public interface IContainer<TBase, TContainer, TComponent> : IContainer<TBase, TContainer>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
        where TComponent : class, TBase
    {
        new IEnumerable<TComponent> Components { get; }
        void Link(TComponent child);
        void Unlink(TComponent child);
        bool TryLink(TComponent child);
        bool TryUnlink(TComponent child);
    }
}