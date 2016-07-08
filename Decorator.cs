using System;
using System.Collections.Generic;
using System.Linq;
using Stave.Base;
using Stave.Exceptions;

namespace Stave
{
    public class Decorator<TAbstract, TParent, TComponent> : ComponentBase<TAbstract, TParent>, IDecorator<TAbstract, TParent, TComponent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
        where TComponent : class, TAbstract
    {
        private TComponent _component;

        public TComponent Component
        {
            get { return _component; }
            set
            {
                if (_component != null)
                    throw new InvalidChildException("You must unlink a decorator before assign a new component !");

                if (value != null)
                {
                    if (Equals(value))
                        throw new InvalidOperationException("Item can't be a child of itself.");
                    if (ContainsAmongParents(value))
                        throw new InvalidOperationException("Item can't be a child of this because it already exist among its parents.");
                }

                _component = value;

                if (value != null)
                    value.Parent = this as TParent;
            }
        }

        public override sealed TAbstract GetComponent(string name)
        {
            return GetComponent(component => component.Name == name);
        }

        public override sealed TAbstract GetComponent(Type type)
        {
            return GetComponent(type.IsInstanceOfType);
        }

        public override sealed TAbstract GetComponent(Predicate<TAbstract> predicate)
        {
            return Component != null && predicate(Component) ? Component : null;
        }

        public override sealed T GetComponent<T>()
        {
            return GetComponent<T>(component => true);
        }

        public override sealed T GetComponent<T>(Predicate<T> predicate)
        {
            var component = Component as T;
            if (component == null)
                return null;

            return predicate(component) ? component : null;
        }

        public override sealed TAbstract GetComponentInChildren(string name)
        {
            return GetComponentInChildren(component => component.Name == name);
        }

        public override sealed TAbstract GetComponentInChildren(Type type)
        {
            return GetComponentInChildren(type.IsInstanceOfType);
        }

        public override sealed TAbstract GetComponentInChildren(Predicate<TAbstract> predicate)
        {
            TAbstract component = GetComponent(predicate);
            return component ?? Component?.GetComponentInChildren(predicate);
        }

        public override sealed T GetComponentInChildren<T>()
        {
            return GetComponentInChildren<T>(component => true);
        }

        public override sealed T GetComponentInChildren<T>(Predicate<T> predicate)
        {
            T component = GetComponent(predicate);
            return component ?? Component?.GetComponentInChildren(predicate);
        }

        public override sealed IEnumerable<TAbstract> GetAllComponents(Type type)
        {
            return GetAllComponents(type.IsInstanceOfType);
        }

        public override sealed IEnumerable<TAbstract> GetAllComponents(Predicate<TAbstract> predicate)
        {
            var result = new List<TAbstract>();

            if (Component != null && predicate(Component))
                result.Add(Component);

            return result;
        }

        public override sealed IEnumerable<T> GetAllComponents<T>()
        {
            return GetAllComponents<T>(component => true);
        }

        public override sealed IEnumerable<T> GetAllComponents<T>(Predicate<T> predicate)
        {
            var result = new List<T>();

            var component = Component as T;
            if (component != null && predicate(component))
                result.Add(component);

            return result;
        }

        public override sealed IEnumerable<TAbstract> GetAllComponentsInChildren(Type type)
        {
            return GetAllComponentsInChildren(type.IsInstanceOfType);
        }

        public override sealed IEnumerable<TAbstract> GetAllComponentsInChildren(Predicate<TAbstract> predicate)
        {
            List<TAbstract> result = GetAllComponents(predicate).ToList();

            if (Component != null)
                result.AddRange(Component.GetAllComponentsInChildren(predicate));

            return result;
        }

        public override sealed IEnumerable<T> GetAllComponentsInChildren<T>()
        {
            return GetAllComponentsInChildren<T>(component => true);
        }

        public override sealed IEnumerable<T> GetAllComponentsInChildren<T>(Predicate<T> predicate)
        {
            List<T> result = GetAllComponents(predicate).ToList();

            if (Component != null)
                result.AddRange(Component.GetAllComponentsInChildren(predicate));

            return result;
        }

        public override sealed bool Contains(TAbstract component)
        {
            return Component != null && Component.Equals(component);
        }

        public override sealed bool ContainsInChildren(TAbstract component)
        {
            return Contains(component) || Component.ContainsInChildren(component);
        }

        public TComponent Unlink()
        {
            TComponent component = Component;
            Component = null;
            return component;
        }

        bool IParent<TAbstract, TParent>.Link(TAbstract child)
        {
            var component = child as TComponent;
            if (component == null)
                throw new InvalidChildException("Component provided must be of type " + typeof(TComponent) + " !");

            if (Component == child)
                return false;

            Component = component;
            return true;
        }

        bool IParent<TAbstract, TParent>.Unlink(TAbstract child)
        {
            var component = child as TComponent;
            if (component == null)
                throw new InvalidChildException("Component provided must be of type " + typeof(TComponent) + " !");

            if (Component != child)
                return false;

            Component = null;
            return true;
        }
    }
}