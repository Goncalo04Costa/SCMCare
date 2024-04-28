using System;

namespace WebApplication1.Excecoes
{
    public class DomainExceptionValidation : Exception
    {
        public DomainExceptionValidation(string message) : base(message)
        {
        }

        public DomainExceptionValidation(string message, Exception innerException) : base(message, innerException)
        {
        }

        public static void When(bool condition, string message)
        {
            if (condition)
            {
                throw new DomainExceptionValidation(message);
            }
        }
    }
}

