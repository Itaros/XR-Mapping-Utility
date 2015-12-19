using System;
using System.Collections.Generic;
using System.IO;
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


        public string[] GetExtensionNames()
        {
            DirectoryInfo info = new DirectoryInfo(PathToGame+Path.DirectorySeparatorChar+"extensions");
            if (info.Exists)
            {
                var folders = info.GetDirectories();
                return folders.Select(f => f.Name).ToArray();
            }
            else
            {
                return null;
            }
        }
    }
}
