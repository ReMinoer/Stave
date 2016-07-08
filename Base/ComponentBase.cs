using System;
using System.Collections.Generic;
using Stave.Utils;

namespace Stave.Base
{
    public abstract class ComponentBase<TAbstract, TParent> : IComponent<TAbstract, TParent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
    {
        private TParent _parent;
        public string Name { get; set; }

        public TParent Parent
        {
            get { return _parent; }
            set
            {
                if (value == _parent)
                    return;

                var baseComponent = this as TAbstract;

                _parent?.Unlink(baseComponent);
                _parent = value;
                _parent?.Link(baseComponent);
            }
        }

        protected ComponentBase()
        {
            Name = GetType().GetDisplayName();
        }

        public abstract TAbstract GetComponent(string name);
        public abstract TAbstract GetComponent(Type type);
        public abstract TAbstract GetComponent(Predicate<TAbstract> predicate);
        public abstract T GetComponent<T>() where T : class, TAbstract;
        public abstract T GetComponent<T>(Predicate<T> predicate) where T : class, TAbstract;
        public abstract TAbstract GetComponentInChildren(string name);
        public abstract TAbstract GetComponentInChildren(Type type);
        public abstract TAbstract GetComponentInChildren(Predicate<TAbstract> predicate);
        public abstract T GetComponentInChildren<T>() where T : class, TAbstract;
        public abstract T GetComponentInChildren<T>(Predicate<T> predicate) where T : class, TAbstract;
        public abstract IEnumerable<TAbstract> GetAllComponents(Type type);
        public abstract IEnumerable<TAbstract> GetAllComponents(Predicate<TAbstract> predicate);
        public abstract IEnumerable<T> GetAllComponents<T>() where T : class, TAbstract;
        public abstract IEnumerable<T> GetAllComponents<T>(Predicate<T> predicate) where T : class, TAbstract;
        public abstract IEnumerable<TAbstract> GetAllComponentsInChildren(Type type);
        public abstract IEnumerable<TAbstract> GetAllComponentsInChildren(Predicate<TAbstract> predicate);
        public abstract IEnumerable<T> GetAllComponentsInChildren<T>() where T : class, TAbstract;
        public abstract IEnumerable<T> GetAllComponentsInChildren<T>(Predicate<T> predicate) where T : class, TAbstract;
        public abstract bool Contains(TAbstract component);
        public abstract bool ContainsInChildren(TAbstract component);

        public TAbstract GetComponentAmongParents(string name)
        {
            return GetComponentAmongParents(component => component.Name == name);
        }

        public TAbstract GetComponentAmongParents(Type type)
        {
            return GetComponentAmongParents(type.IsInstanceOfType);
        }

        public TAbstract GetComponentAmongParents(Predicate<TAbstract> predicate)
        {
            if (Parent == null)
                return null;

            return predicate(Parent) ? Parent : Parent.GetComponentAmongParents(predicate);
        }

        public T GetComponentAmongParents<T>() where T : class, TAbstract
        {
            return GetComponentAmongParents<T>(component => true);
        }

        public T GetComponentAmongParents<T>(Predicate<T> predicate) where T : class, TAbstract
        {
            var parent = Parent as T;
            if (parent == null)
                return Parent.GetComponentAmongParents(predicate);

            return predicate(parent) ? parent : Parent.GetComponentAmongParents<T>();
        }

        public bool ContainsAmongParents(TAbstract component)
        {
            if (Parent == null)
                return false;

            return Parent.Equals(component) || Parent.ContainsAmongParents(component);
        }
    }
}