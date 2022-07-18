namespace Stave
{
    public class ComponentsChangedEventArgs<TBase, TContainer, TComponent> : IComponentsChangedEventArgs<TBase, TContainer, TComponent>
        where TBase : class, IComponent<TBase, TContainer>
        where TContainer : class, TBase, IContainer<TBase, TContainer>
        where TComponent : class, TBase
    {
        public ComponentsChangeType ChangeType { get; }
        public TContainer Parent { get; }
        public TComponent Component { get; }

        static public ComponentsChangedEventArgs<TBase, TContainer, TComponent> Add(TContainer parent, TComponent component)
            => new ComponentsChangedEventArgs<TBase, TContainer, TComponent>(ComponentsChangeType.Add, parent, component);
        static public ComponentsChangedEventArgs<TBase, TContainer, TComponent> Remove(TContainer parent, TComponent component)
            => new ComponentsChangedEventArgs<TBase, TContainer, TComponent>(ComponentsChangeType.Remove, parent, component);

        private ComponentsChangedEventArgs(ComponentsChangeType changeType, TContainer parent, TComponent component)
        {
            ChangeType = changeType;
            Parent = parent;
            Component = component;
        }

        IContainer IComponentsChangedEventArgs.Parent => Parent;
        TBase IComponentsChangedEventArgs<TBase>.Parent => Parent;
        TContainer IComponentsChangedEventArgs<TBase, TContainer>.Parent => Parent;

        IComponent IComponentsChangedEventArgs.Component => Component;
        TBase IComponentsChangedEventArgs<TBase>.Component => Component;
        TComponent IComponentsChangedEventArgs<TBase, TContainer, TComponent>.Component => Component;
    }
}