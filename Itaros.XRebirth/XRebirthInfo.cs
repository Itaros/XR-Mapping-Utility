using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Itaros.XRebirth
{
    public class XRebirthInfo
    {

        public XRebirthInfo(string path)
        {
            PathToGame = path;
        }

        public string PathToGame { get; private set; }

    }
}
