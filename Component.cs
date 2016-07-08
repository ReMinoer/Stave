using System;
using System.Collections.Generic;
using System.Linq;
using Stave.Base;

namespace Stave
{
    public class Component<TAbstract, TParent> : ComponentBase<TAbstract, TParent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
    {
        public override sealed TAbstract GetComponent(string name)
        {
            return null;
        }

        public override sealed TAbstract GetComponent(Type type)
        {
            return null;
        }

        public override TAbstract GetComponent(Predicate<TAbstract> predicate)
        {
            return null;
        }

        public override sealed T GetComponent<T>()
        {
            return null;
        }

        public override T GetComponent<T>(Predicate<T> predicate)
        {
            return null;
        }

        public override sealed TAbstract GetComponentInChildren(string name)
        {
            return null;
        }

        public override sealed TAbstract GetComponentInChildren(Type type)
        {
            return null;
        }

        public override TAbstract GetComponentInChildren(Predicate<TAbstract> predicate)
        {
            return null;
        }

        public override sealed T GetComponentInChildren<T>()
        {
            return null;
        }

        public override T GetComponentInChildren<T>(Predicate<T> predicate)
        {
            return null;
        }

        public override sealed IEnumerable<TAbstract> GetAllComponents(Type type)
        {
            return Enumerable.Empty<TAbstract>();
        }

        public override IEnumerable<TAbstract> GetAllComponents(Predicate<TAbstract> predicate)
        {
            return Enumerable.Empty<TAbstract>();
        }

        public override sealed IEnumerable<T> GetAllComponents<T>()
        {
            return Enumerable.Empty<T>();
        }

        public override IEnumerable<T> GetAllComponents<T>(Predicate<T> predicate)
        {
            return Enumerable.Empty<T>();
        }

        public override sealed IEnumerable<TAbstract> GetAllComponentsInChildren(Type type)
        {
            return Enumerable.Empty<TAbstract>();
        }

        public override IEnumerable<TAbstract> GetAllComponentsInChildren(Predicate<TAbstract> predicate)
        {
            return Enumerable.Empty<TAbstract>();
        }

        public override sealed IEnumerable<T> GetAllComponentsInChildren<T>()
        {
            return Enumerable.Empty<T>();
        }

        public override IEnumerable<T> GetAllComponentsInChildren<T>(Predicate<T> predicate)
        {
            return Enumerable.Empty<T>();
        }

        public override sealed bool Contains(TAbstract component)
        {
            return false;
        }

        public override sealed bool ContainsInChildren(TAbstract component)
        {
            return false;
        }
    }
}