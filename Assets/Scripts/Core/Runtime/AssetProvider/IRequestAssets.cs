using System;
using System.Collections.Generic;

namespace Core.AssetProvider
{
    public interface IRequestAssets
    {
        Dictionary<Type, string> RequestAssets();
    }
}