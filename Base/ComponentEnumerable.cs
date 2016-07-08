using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stave.Exceptions;

namespace Stave.Base
{
    public abstract class ComponentEnumerable<TAbstract, TParent, TComponent> : ComponentBase<TAbstract, TParent>, IParent<TAbstract, TParent>, IEnumerable<TComponent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
        where TComponent : class, TAbstract
    {
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
            return this.FirstOrDefault(component => predicate(component));
        }

        public override sealed T GetComponent<T>()
        {
            return GetComponent<T>(component => true);
        }

        public override sealed T GetComponent<T>(Predicate<T> predicate)
        {
            return this.OfType<T>().FirstOrDefault(component => predicate(component));
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
            if (component != null)
                return component;

            foreach (TComponent child in this)
            {
                component = child.GetComponentInChildren(predicate);
                if (component != null)
                    return component;
            }

            return null;
        }

        public override sealed T GetComponentInChildren<T>()
        {
            return GetComponentInChildren<T>(component => true);
        }

        public override sealed T GetComponentInChildren<T>(Predicate<T> predicate)
        {
            T component = GetComponent(predicate);
            if (component != null)
                return component;

            foreach (TComponent child in this)
            {
                component = child.GetComponentInChildren(predicate);
                if (component != null)
                    return component;
            }

            return null;
        }

        public override sealed IEnumerable<TAbstract> GetAllComponents(Type type)
        {
            return GetAllComponents(type.IsInstanceOfType);
        }

        public override IEnumerable<TAbstract> GetAllComponents(Predicate<TAbstract> predicate)
        {
            return this.Where(component => predicate(component));
        }

        public override sealed IEnumerable<T> GetAllComponents<T>()
        {
            return GetAllComponents<T>(component => true);
        }

        public override sealed IEnumerable<T> GetAllComponents<T>(Predicate<T> predicate)
        {
            return this.OfType<T>().Where(component => predicate(component));
        }

        public override sealed IEnumerable<TAbstract> GetAllComponentsInChildren(Type type)
        {
            return GetAllComponentsInChildren(type.IsInstanceOfType);
        }

        public override sealed IEnumerable<TAbstract> GetAllComponentsInChildren(Predicate<TAbstract> predicate)
        {
            List<TAbstract> result = GetAllComponents(predicate).ToList();

            foreach (TComponent child in this)
                result.AddRange(child.GetAllComponentsInChildren(predicate));

            return result;
        }

        public override sealed IEnumerable<T> GetAllComponentsInChildren<T>()
        {
            return GetAllComponentsInChildren<T>(component => true);
        }

        public override sealed IEnumerable<T> GetAllComponentsInChildren<T>(Predicate<T> predicate)
        {
            List<T> result = GetAllComponents(predicate).ToList();

            foreach (TComponent child in this)
                result.AddRange(child.GetAllComponentsInChildren(predicate));

            return result;
        }

        public override sealed bool Contains(TAbstract component)
        {
            return this.Any(child => child == component);
        }

        public override sealed bool ContainsInChildren(TAbstract component)
        {
            return Contains(component) || this.Any(child => child.ContainsInChildren(component));
        }

        public abstract IEnumerator<TComponent> GetEnumerator();
        protected abstract void Link(TComponent component);
        protected abstract void Unlink(TComponent component);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        bool IParent<TAbstract, TParent>.Link(TAbstract child)
        {
            var component = child as TComponent;
            if (component == null)
                throw new InvalidChildException("Component provided must be of type " + typeof(TComponent));

            if (Contains(component))
                return false;

            Link(component);
            return true;
        }

        bool IParent<TAbstract, TParent>.Unlink(TAbstract child)
        {
            var component = child as TComponent;
            if (component == null)
                throw new InvalidChildException("Component provided must be of type " + typeof(TComponent));

            if (!Contains(component))
                return false;

            Unlink(component);
            return true;
        }
    }
}