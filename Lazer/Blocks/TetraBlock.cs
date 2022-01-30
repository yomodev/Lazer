using System;
using System.Drawing;
using System.Xml;

namespace Lazer
{
    class TetraBlock : Prism
    {
        public Color Color1
        {
            get { return _color[0]; }
            set
            {
                if (_color[0] != value)
                {
                    _color[0] = value;
                    needRefresh = true;
                }
            }
        }

        public Color Color2
        {
            get { return _color[1]; }
            set
            {
                if (_color[1] != value)
                {
                    _color[1] = value;
                    needRefresh = true;
                }
            }
        }

        public Color Color3
        {
            get { return _color[2]; }
            set
            {
                if (_color[2] != value)
                {
                    _color[2] = value;
                    needRefresh = true;
                }
            }
        }

        public Color Color4
        {
            get { return _color[3]; }
            set
            {
                if (_color[3] != value)
                {
                    _color[3] = value;
                    needRefresh = true;
                }
            }
        }

        public TetraBlock() : base()
        {
            _color = new Color[4] { Color.White, Color.Red, Color.Lime, Color.Blue };
        }

        public TetraBlock(Direction dir, Color[] c) : base(dir)
        {
            _color = c != null && c.Length == 4 ? c : new Color[4] { Color.White, Color.Red, Color.Lime, Color.Blue };
        }

        public override string ToString()
        {
            string s = "TetraBlock(";
            switch (_direction)
            {
                case Direction.Up: s += "^"; break;
                case Direction.Down: s += "v"; break;
                case Direction.Left: s += "<"; break;
                case Direction.Right: s += ">"; break;
            }

            return s + ", " + Color1 + ", " + Color2 + ", " + Color3 + ", " + Color4 + ")";
        }

        public override XmlNode Serialize(XmlDocument xdocument, XmlElement parent)
        {
            parent.SetAttribute("type", GetType().FullName);
            parent.SetAttribute("direction", Direction.ToString());
            parent.SetAttribute("color1", ColorTranslator.ToHtml(Color1));
            parent.SetAttribute("color2", ColorTranslator.ToHtml(Color2));
            parent.SetAttribute("color3", ColorTranslator.ToHtml(Color3));
            parent.SetAttribute("color4", ColorTranslator.ToHtml(Color4));
            return parent;
        }

        public static new TetraBlock Deserialize(XmlElement node)
        {
            TetraBlock obj = new TetraBlock();

            if (node.HasAttribute("direction"))
            {
                obj.Direction = (Direction)Enum.Parse(typeof(Direction), node.GetAttribute("direction"), true);
            }

            if (node.HasAttribute("color1"))
            {
                obj.Color1 = ColorTranslator.FromHtml(node.GetAttribute("color1"));
            }
            if (node.HasAttribute("color2"))
            {
                obj.Color2 = ColorTranslator.FromHtml(node.GetAttribute("color2"));
            }
            if (node.HasAttribute("color3"))
            {
                obj.Color3 = ColorTranslator.FromHtml(node.GetAttribute("color3"));
            }
            if (node.HasAttribute("color4"))
            {
                obj.Color4 = ColorTranslator.FromHtml(node.GetAttribute("color4"));
            }

            return obj;
        }
    }
}