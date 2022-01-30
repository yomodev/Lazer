using System;
using System.Drawing;
using System.Xml;

namespace Lazer
{
    class Wall : Block
    {
        private TextureBrush brush;


        // wall is an opaque object: do not return any ray
        public Wall()
        {
            brush = new TextureBrush(Texture.extract(TextureType.Wall));
        }
        

        public override void draw(Graphics g, Rectangle rect, bool showLaser = true)
        {
            g.FillRectangle(brush, rect);
        }


        /*
        public override int CompareTo(IBlock obj)
        {
            if (obj is Wall) return 0;
            return obj == null ? 1 : -1;
        }
        */


        public override string ToString()
        {
            return "Wall";
        }


        public override XmlNode serialize(XmlDocument xdocument, XmlElement parent)
        {
            parent.SetAttribute("type", GetType().FullName);
            return parent;
        }


        public static Block deserialize(XmlElement node)
        {
            Wall obj = new Wall();
            return obj;
        }
        
    }
}
