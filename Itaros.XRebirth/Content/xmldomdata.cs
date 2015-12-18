using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Itaros.XRebirth.Content
{
    public class XMLDOMData
    {

        public bool IsValid { get; private set; }
        public string Filename { get; private set; }

        private string _fullpath;
        protected XmlDocument _dom;

        public XMLDOMData(string fullpath)
        {
            _fullpath = fullpath;

            IsValid = System.IO.File.Exists(_fullpath);
            Filename = System.IO.Path.GetFileName(_fullpath);

            if (IsValid)
            {
                _dom = new XmlDocument();
                _dom.Load(_fullpath);

                Load();
            }
        }

        protected virtual void Load()
        {

        }

        public void FlushTo(string path, Encoding encoding)
        {
            using (var writer = new StreamWriter(path, false, encoding))
            {
                _dom.Save(writer);
            }
        }

        public XmlNode CreateRawNode(string name)
        {
            var node = _dom.CreateNode(XmlNodeType.Element, name, null);
            return node;
        }
        public XmlAttribute CreateRawAttribute(string name, string content)
        {
            var attrib = _dom.CreateAttribute(name);
            attrib.InnerText = content;
            return attrib;
        }

    }
}
