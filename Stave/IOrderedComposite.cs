using Diese.Collections;

namespace Stave
{
    public interface IOrderedComposite<TBase, TContainer, TComponent> : IComposite<TBase, TContainer, TComponent>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
        where TComponent : class, TBase
    {
        new IWrappedList<TComponent> Components { get; }
        TComponent this[int index] { get; set; }
        int IndexOf(TComponent item);
        void Insert(int index, TComponent item);
        void RemoveAt(int index);
    }
}