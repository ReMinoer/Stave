using System;

namespace Stave.Exceptions
{
    public enum ReadOnlyParent
    {
        Previous,
        New
    }

    public class ReadOnlyParentException : Exception
    {
        public ReadOnlyParent ReadOnlyParent { get; }

        public override string Message
        {
            get
            {
                switch (ReadOnlyParent)
                {
                    case ReadOnlyParent.Previous:
                        return "Previous parent is read-only and can't unlink this component.";
                    case ReadOnlyParent.New:
                        return "New parent is read-only and can't link this component.";
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public ReadOnlyParentException(ReadOnlyParent readOnlyParent)
        {
            ReadOnlyParent = readOnlyParent;
        }
    }
}