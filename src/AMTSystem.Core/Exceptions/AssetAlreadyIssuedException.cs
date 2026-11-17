using System;
using System.Collections.Generic;
using System.Text;

namespace AMTSystem.Core.Exceptions;

public class AssetAlreadyIssuedException : Exception
{
    public AssetAlreadyIssuedException()
        : base("One or more assets are already issued or lent.") { }

    public AssetAlreadyIssuedException(Exception inner)
        : base("One or more assets are already issued or lent.", inner) { }
}
