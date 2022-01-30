using System.Drawing;
using System.Xml;

namespace Lazer
{
    class Empty : Block
    {
        private TextureBrush laserH;
        private TextureBrush laserV;

        public Empty()
        {
            isMovable = true;
        }

        // return flipped input ie to propagate the ray
        public override Ray Process(Ray input)
        {
            output = new Ray(input.down, input.up, input.right, input.left);
            needRefresh = true;

            return output;
        }

        public override void Draw(Graphics g, Rectangle rect, bool showLaser = true)
        {
            if (needRefresh)
            {
                laserH = output.HasLeft || output.HasRight ?
                        new TextureBrush(Texture.colorize(Texture.extract(TextureType.Ray), output.HasLeft ? output.left : output.right)) : null;
                laserV = output.HasUp || output.HasDown ?
                    new TextureBrush(Texture.colorize(Texture.extract(TextureType.Ray, 90), output.HasUp ? output.up : output.down)) : null;

                needRefresh = false;
            }

            if (showLaser)
            {
                if (laserH != null) g.FillRectangle(laserH, rect);
                if (laserV != null) g.FillRectangle(laserV, rect);
            }
        }

        /*
        public override int CompareTo(IBlock obj)
        {
            if (obj is Empty) return 0;
            return obj == null ? 1 : -1;
        }
        */

        public override string ToString()
        {
            return "Empty";
        }

        public override XmlNode Serialize(XmlDocument xdocument, XmlElement parent)
        {
            /*
            parent.SetAttribute("type", "empty");
            return parent;*/
            return null;
        }

        public static Empty Deserialize(XmlElement node)
        {
            Empty obj = new Empty();
            return obj;
        }
    }
}
