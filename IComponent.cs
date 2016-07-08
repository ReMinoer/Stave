using System;
using System.Collections.Generic;

namespace Stave
{
    public interface IComponent<TAbstract, TParent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
    {
        string Name { get; }
        TParent Parent { get; set; }
        TAbstract GetComponent(string name);
        TAbstract GetComponent(Type type);
        TAbstract GetComponent(Predicate<TAbstract> predicate);
        T GetComponent<T>() where T : class, TAbstract;
        T GetComponent<T>(Predicate<T> predicate) where T : class, TAbstract;
        TAbstract GetComponentInChildren(string name);
        TAbstract GetComponentInChildren(Type type);
        TAbstract GetComponentInChildren(Predicate<TAbstract> predicate);
        T GetComponentInChildren<T>() where T : class, TAbstract;
        T GetComponentInChildren<T>(Predicate<T> predicate) where T : class, TAbstract;
        TAbstract GetComponentAmongParents(string name);
        TAbstract GetComponentAmongParents(Type type);
        TAbstract GetComponentAmongParents(Predicate<TAbstract> predicate);
        T GetComponentAmongParents<T>() where T : class, TAbstract;
        T GetComponentAmongParents<T>(Predicate<T> predicate) where T : class, TAbstract;
        IEnumerable<TAbstract> GetAllComponents(Type type);
        IEnumerable<TAbstract> GetAllComponents(Predicate<TAbstract> predicate);
        IEnumerable<T> GetAllComponents<T>() where T : class, TAbstract;
        IEnumerable<T> GetAllComponents<T>(Predicate<T> predicate) where T : class, TAbstract;
        IEnumerable<TAbstract> GetAllComponentsInChildren(Type type);
        IEnumerable<TAbstract> GetAllComponentsInChildren(Predicate<TAbstract> predicate);
        IEnumerable<T> GetAllComponentsInChildren<T>() where T : class, TAbstract;
        IEnumerable<T> GetAllComponentsInChildren<T>(Predicate<T> predicate) where T : class, TAbstract;
        bool Contains(TAbstract component);
        bool ContainsInChildren(TAbstract component);
        bool ContainsAmongParents(TAbstract component);
    }
}