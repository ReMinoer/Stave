using Diese.Collections.Observables.ReadOnly;

namespace Stave
{
    public interface IComposite<TBase, TContainer, TComponent> : IContainer<TBase, TContainer, TComponent>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
        where TComponent : class, TBase
    {
        new IWrappedObservableCollection<TComponent> Components { get; }
        void Add(TComponent item);
        bool Remove(TComponent item);
        void Clear();
        bool Contains(TComponent item);
    }
}