using System;

namespace Stave.Exceptions
{
    public class ReadOnlyParentException : Exception
    {
        public override string Message => "Parent is read-only and can't use Link or Unlink methods.";
    }
}