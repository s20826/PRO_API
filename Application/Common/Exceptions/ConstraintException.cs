using System;

namespace Application.Common.Exceptions
{
    public class ConstraintException : Exception
    {
        public int ConstraintValue { set; get; }

        public ConstraintException(string message, int value) : base(message)
        {
            ConstraintValue = value;
        }
    }
}
