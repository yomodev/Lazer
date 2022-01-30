using System;
using System.Drawing;
using System.Xml;

namespace Lazer
{
    class Filter90 : Block, IRotable
    {
        private TextureBrush laser;
        protected TextureBrush[] brushes = new TextureBrush[2];
        private bool _monodirection = true;
        private Direction _direction = Direction.Left;

        public Filter90() :base()
        {
            isMovable = false;
        }

        public Filter90(Direction dir, bool monodirection) : this()
        {
            Direction = dir;
            Monodirection = monodirection;
        }

        public bool Monodirection
        {
            get { return _monodirection; }
            set
            {
                if (_monodirection != value)
                {
                    _monodirection = value;
                    needRefresh = true;
                }
            }
        }

        public Direction Direction
        {
            get { return _direction; }
            set
            {
                if (_direction != value)
                {
                    _direction = value;
                    needRefresh = true;
                }
            }
        }
        
        // return flipped input ie to propagate the ray
        public override Ray Process(Ray input)
        {
            Direction d = Direction;
            switch (Direction)
            {
                case Direction.Up: // v<
                    d = Direction.Left;
                    output = new Ray(Monodirection ? Color.Transparent : input.left, Color.Transparent, input.up, Color.Transparent);
                    break;
                case Direction.Right: // <^
                    d = Direction.Up;
                    output = new Ray(input.right, Color.Transparent, Color.Transparent, Monodirection ? Color.Transparent : input.up);
                    break;
                case Direction.Down: // ^>
                    d = Direction.Right;
                    output = new Ray(Color.Transparent, Monodirection ? Color.Transparent : input.right, Color.Transparent, input.down);
                    break;
                case Direction.Left: // >v
                    d = Direction.Down;
                    output = new Ray(Color.Transparent, input.left, Monodirection ? Color.Transparent : input.down, Color.Transparent);
                    break;
            }

            if (input[d] != Color.Transparent && output[d] != Color.Transparent)
            {
                output = input;// new Ray();
            }
            
            needRefresh = true;
            return output;
        }

        public override void Draw(Graphics g, Rectangle rect, bool showLaser = true)
        {
            if (needRefresh)
            {
                int angle = 0;
                Color c = Color.Transparent;
                switch (Direction)
                {
                    case Direction.Up: angle = 270; c = output.left; break;
                    case Direction.Down: angle = 90; c = output.right; break;
                    case Direction.Left: angle = 180; c = output.down; break;
                    case Direction.Right: angle = 0; c = output.up; break;
                }
                
                brushes[0] = new TextureBrush(Texture.extract(TextureType.Filter90, angle));
                brushes[1] = Monodirection ? new TextureBrush(Texture.colorize(Texture.extract(TextureType.Filter90Arrows, angle),Color.PowderBlue)) : null;

                c = output[Direction] == Color.Transparent ? c : output[Direction];
                laser = output.None ? null : laser = new TextureBrush(Texture.colorize(Texture.extract(TextureType.Ray90, angle), c ));
                
                needRefresh = false;
            }

            for (int i = 0; i < brushes.Length; i++)
            {
                if (brushes[i] != null)
                {
                    g.FillRectangle(brushes[i], rect);
                }
            }

            if (showLaser)
            {
                if (laser != null) g.FillRectangle(laser, rect);
            }
        }

        public override string ToString()
        {
            string s = "Filter90(";

            if (Monodirection)
            {
                switch (_direction)
                {
                    case Direction.Right: s += "<^"; break;
                    case Direction.Down: s += "^>"; break;
                    case Direction.Left: s += ">v"; break;
                    case Direction.Up: s += "v<"; break;
                }
            }
            else
            {
                switch (_direction)
                {    
                    case Direction.Right: s += "└"; break;
                    case Direction.Down: s += "┌"; break;
                    case Direction.Left: s += "┐"; break;
                    case Direction.Up: s += "┘"; break;
                }
            }

            return s + ")";
        }

        public override XmlNode Serialize(XmlDocument xdocument, XmlElement parent)
        {
            parent.SetAttribute("type", GetType().FullName);
            parent.SetAttribute("monodirection", Monodirection.ToString());
            parent.SetAttribute("direction", Direction.ToString());
            return parent;
        }

        public static Filter90 Deserialize(XmlElement node)
        {
            Filter90 obj = new Filter90();
            if (node.HasAttribute("monodirection"))
            {
                obj.Monodirection = Convert.ToBoolean(node.GetAttribute("monodirection"));
            }

            if (node.HasAttribute("direction"))
            {
                obj.Direction = (Direction)Enum.Parse(typeof(Direction), node.GetAttribute("direction"), true);
            }

            return obj;
        }

        public void Rotate()
        {
            Direction = (Direction)(Enum.GetValues(Direction.GetType()).Length == (int)Direction + 1 ? 0 : (int)Direction + 1);
        }
    }
}
