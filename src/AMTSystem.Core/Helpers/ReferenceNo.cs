using System;
using System.Collections.Generic;
using System.Text;

namespace AMTSystem.Core.Helpers;

public static class ReferenceNo
{
    public static string GenerateAssetRef(int sequence)
    {
        string allowedCharacters = "0123456789";
        int length = 8;

        string assetRefNum = string.Create(length, allowedCharacters, (buffer, chars) =>
        {
            Random.Shared.GetItems(chars, buffer);
        });

        //TODO: have this function generate an 8-number ref no where
        //the two leftmost digits represent the category of the asset registered
        // for now, just test that is generates assetRefNo.
        return $"{assetRefNum}";
    }
}
