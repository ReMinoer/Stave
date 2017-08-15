using Diese.Collections;

namespace Stave
{
    public interface IOrderedComposite<TAbstract, TParent, TComponent> : IComposite<TAbstract, TParent, TComponent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
        where TComponent : class, TAbstract
    {
        new IWrappedList<TComponent> Components { get; }
        TComponent this[int index] { get; set; }
        int IndexOf(TComponent item);
        void Insert(int index, TComponent item);
        void RemoveAt(int index);
    }
}