using System;
using System.Collections.Generic;
using System.Text;

namespace AMTSystem.Core.Exceptions;

public class TransactionAlreadyReturnedException : Exception
{
    public TransactionAlreadyReturnedException()
        : base("One or more items were already returned.") { }
}