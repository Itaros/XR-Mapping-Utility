using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Itaros.XRebirth.Content
{
    public class DataContainers
    {

        public DataContainers()
        {

        }

        private string _currentMapName = string.Empty;
        public string CurrentMapName
        {
            get { return _currentMapName; }
            set
            {
                _currentMapName = value;
                CurrentMap = new MapInformation(_currentMapName);
            }
        }
        public MapInformation CurrentMap { get; private set; }

        private string[] _presentMapGroups = null;

        public IEnumerable<string> PresentMapGroups { 
            get { return _presentMapGroups; }
            set
            {
                if (value != null)
                {
                    _presentMapGroups = value.ToArray();
                }
                else
                {
                    _presentMapGroups = null;
                }
            }
        }



    }
}
