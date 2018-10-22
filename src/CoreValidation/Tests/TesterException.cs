using System;

namespace CoreValidation.Tests
{
    public class TesterException : Exception
    {
        public TesterException(string message)
            : base(message)
        {
        }
    }
}