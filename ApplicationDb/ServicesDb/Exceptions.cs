using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesDb.Exceptions
{
    public class ClientUnderageException : Exception
    {
        public ClientUnderageException(string message) : base(message)
        {
        }
    }public class MissingContractDataException : Exception
    {
        public MissingContractDataException(string message) : base(message)
        {
        }
    }
    public class EmployeeUnderageException : Exception
    {
        public EmployeeUnderageException(string message) : base(message)
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
    public class EmployeeNotFoundException : Exception
    {
        public EmployeeNotFoundException(string message) : base(message)
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
