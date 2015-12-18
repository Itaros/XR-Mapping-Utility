using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Itaros.XRebirth
{
    public sealed class ExtensionInfo
    {

        public ExtensionInfo(string name)
        {
            ExtensionName = name;
        }

        public string ExtensionName { get; private set; }

    }
}
