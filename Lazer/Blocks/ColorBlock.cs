using System;
using System.Drawing;
using System.Xml;

namespace Lazer
{
    class ColorBlock : Block
    {
        private Color _color = Color.Yellow;
        private TextureBrush brush;


        public Color Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    needRefresh = true;
                }
            }
        }


        public ColorBlock() :base()
        { 
            isMovable = true;
        }


        public ColorBlock(Color c) : this()
        {
            Color = c;
        }


        public override Ray process(Ray input)
        {
            return new Ray(
                input.hasDown ? _color : Color.Transparent,
                input.hasUp ? _color : Color.Transparent,
                input.hasRight ? _color : Color.Transparent,
                input.hasLeft ? _color : Color.Transparent
                );
        }



        public override void draw(Graphics g, Rectangle rect, bool showLaser = true)
        {
            if (needRefresh)
            {
                Bitmap b = Texture.extract(TextureType.ColorBlock);
                b = Texture.colorize(b, _color);
                brush = new TextureBrush(b);
                needRefresh = false;
            }

            g.FillRectangle(brush, rect);
        }


        /*
        public override int CompareTo(IBlock obj)
        {
            if (obj is ColorBlock && ((ColorBlock)obj).color == color)
            {
                return 0;
            }
            return obj == null ? 1 : -1;
        }*/



        public override string ToString()
        {
            return "ColorBlock(" + _color + ")";
        }



        public override XmlNode serialize(XmlDocument xdocument, XmlElement parent)
        {
            parent.SetAttribute("type", GetType().FullName);
            parent.SetAttribute("color", ColorTranslator.ToHtml(Color));
            return parent;
        }


        public static ColorBlock deserialize(XmlElement node)
        {
            ColorBlock obj = new ColorBlock();

            if (node.HasAttribute("color"))
            {
                obj.Color = ColorTranslator.FromHtml(node.GetAttribute("color"));
            }

            return obj;
        }
    }
}
