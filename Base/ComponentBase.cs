using System.Collections.Generic;

namespace Stave.Base
{
    public abstract class ComponentBase<TAbstract, TParent> : IComponent<TAbstract, TParent>
        where TAbstract : class, IComponent<TAbstract, TParent>
        where TParent : class, TAbstract, IParent<TAbstract, TParent>
    {
        private TParent _parent;
        IEnumerable<TAbstract> IComponent<TAbstract, TParent>.Components => ProtectedComponents;
        protected internal abstract IEnumerable<TAbstract> ProtectedComponents { get; }

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
    }
}