using System;
using System.Drawing;
using System.Xml;

namespace Lazer
{
    class Mirror : Block, IRotable
    {
        private Angle _angle = Angle.NESW;
        private TextureBrush brush;
        private TextureBrush laser1;
        private TextureBrush laser2;

        public Angle Angle
        {
            get { return _angle; }
            set 
            {
                if (_angle != value)
                {
                    _angle = value;
                    needRefresh = true;
                }
            }
        }

        public Mirror() : base()
        { 
            isMovable = true;
        }
        
        // define mirror reflection angle: can be NESW=/ or NWSE=\
        public Mirror(Angle a) :this()
        {
            Angle = a;
        }

        // return deflected input given mirror angle
        public override Ray Process(Ray input)
        {
            output = new Ray();
            
            if (_angle == Angle.NESW)
            {
                output.up = input.HasLeft ? input.left : output.up;
                output.left = input.HasUp ? input.up : output.left;
                output.right = input.HasDown ? input.down : output.right;
                output.down = input.HasRight ? input.right : output.down;
            }
            else 
            {
                output.up = input.HasRight ? input.right : output.up;
                output.left = input.HasDown ? input.down : output.left;
                output.right = input.HasUp ? input.up : output.right;
                output.down = input.HasLeft ? input.left : output.down;
            }

            needRefresh = true;
            return output;
        }

        public override void Draw(Graphics g, Rectangle rect, bool showLaser = true)
        {
            if (needRefresh)
            {
                brush = new TextureBrush(Texture.extract(TextureType.Mirror, _angle == Angle.NESW ? 0 : 90));
                if (_angle == Angle.NESW)
                {
                    laser1 = output.HasLeft || output.HasUp ? new TextureBrush(Texture.colorize(Texture.extract(TextureType.Ray45, 90), output.HasLeft ? output.left : output.up)) : null;
                    laser2 = output.HasRight || output.HasDown ? new TextureBrush(Texture.colorize(Texture.extract(TextureType.Ray45, 270), output.HasRight ? output.right : output.down)) : null;
                }
                else
                {
                    laser1 = output.HasRight || output.HasUp ? new TextureBrush(Texture.colorize(Texture.extract(TextureType.Ray45, 180), output.HasRight ? output.right : output.up)) : null;
                    laser2 = output.HasLeft || output.HasDown ? new TextureBrush(Texture.colorize(Texture.extract(TextureType.Ray45, 0), output.HasLeft ? output.left : output.down)) : null;
                }
                needRefresh = false;
            }

            g.FillRectangle(brush, rect);
            
            if (showLaser)
            {
                if (laser1 != null) g.FillRectangle(laser1, rect);
                if (laser2 != null) g.FillRectangle(laser2, rect);
            }
        }

        /*
        public override int CompareTo(IBlock obj)
        {
            if(obj is Mirror && ((Mirror)obj).angle == angle)
            {
                return 0;
            }
            return obj == null ? 1 : -1;
        }*/

        public override string ToString()
        {
            return _angle == Angle.NESW ? "Mirror(/)" : "Mirror(\\)";
        }

        public override XmlNode Serialize(XmlDocument xdocument, XmlElement parent)
        {
            parent.SetAttribute("type", GetType().FullName);
            parent.SetAttribute("angle", Angle.ToString());
            return parent;
        }

        public static Mirror Deserialize(XmlElement node)
        {
            Mirror obj = new Mirror();
            if(node.HasAttribute("angle"))
            {
                obj.Angle = (Angle) Enum.Parse(typeof(Angle), node.GetAttribute("angle"), true);
            }
            return obj;
        }

        public void Rotate()
        {
            Angle = (Angle)(Enum.GetValues(Angle.GetType()).Length == (int)Angle + 1 ? 0 : (int)Angle + 1);
        }
    }
}