using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exceprtions
{
    public class ClientUnderageException : Exception
    {
        public ClientUnderageException(string message) : base(message)
        {
        }
    }

    public class MissingPassportDataException : Exception
    {
        public MissingPassportDataException(string message) : base(message)
        {
        }
    }

    public class ClientNotFoundException : Exception
    {
        public ClientNotFoundException(string message) : base(message)
        {
        }
    }

    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException(string message) : base(message)
        {
        }
    }
}
