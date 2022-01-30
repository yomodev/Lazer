using System;
using System.Drawing;
using System.Xml;

namespace Lazer
{
    class Prism : Block, IRotable
    {
        protected Direction _direction = Direction.Left;
        protected TextureBrush[] brushes = new TextureBrush[6];
        protected Color[] _color = new Color[4] { Color.White, Color.Yellow, Color.Fuchsia, Color.Cyan};



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
        


        public Prism() : base()
        { 
            isMovable = true;
        }



        public Prism(Direction dir) : this()
        {
            Direction = dir;
        }


        // for now, if hit by a white ray return yellow, red and cyan rays
        public override Ray process(Ray input)
        {
            output = new Ray();
            if (_direction == Direction.Up && input.up == _color[0])
            {
                output.left = _color[1];
                output.down = _color[2];
                output.right = _color[3];
            }
            else if (_direction == Direction.Down && input.down == _color[0])
            {
                output.right = _color[1];
                output.up = _color[2];
                output.left = _color[3];
            }
            else if (_direction == Direction.Left && input.left == _color[0])
            {
                output.down = _color[1];
                output.right = _color[2];
                output.up = _color[3];
            }
            else if (_direction == Direction.Right && input.right == _color[0])
            {
                output.up = _color[1];
                output.left = _color[2];
                output.down = _color[3];
            }
            
            return output;
        }



        public override void draw(Graphics g, Rectangle rect, bool showLaser = true)
        {
            if (needRefresh)
            {
                brushes[0] = new TextureBrush(Texture.colorize(Texture.extract(TextureType.TetraBase),Color.White));
                int rotation = 0;
                switch (_direction)
                {
                    case Direction.Up: rotation = 0; break;
                    case Direction.Down: rotation = 180; break;
                    case Direction.Left: rotation = 270; break;
                    case Direction.Right: rotation = 90; break;
                }

                brushes[1] = new TextureBrush(Texture.colorize(Texture.extract(TextureType.TetraTriangle, rotation), _color[0]));
                brushes[2] = new TextureBrush(Texture.colorize(Texture.extract(TextureType.TetraTriangle, (270 + rotation) % 360), _color[1]));
                brushes[3] = new TextureBrush(Texture.colorize(Texture.extract(TextureType.TetraTriangle, (180 + rotation) % 360), _color[2]));
                brushes[4] = new TextureBrush(Texture.colorize(Texture.extract(TextureType.TetraTriangle, (90 + rotation) % 360), _color[3]));
                brushes[5] = new TextureBrush(Texture.colorize(Texture.extract(TextureType.InputArrow, (180 + rotation) % 360), Color.Silver));

                needRefresh = false;
            }

            for (int i = 0; i < brushes.Length; i++)
            {
                if (brushes[i] != null)
                {
                    g.FillRectangle(brushes[i], rect);
                }
            }
        }



        /*
        public override int CompareTo(IBlock obj)
        {
            if (obj is Prism && ((Prism)obj).direction == direction)
            {
                return 0;
            }
            return obj == null ? 1 : -1;
        }*/



        public override string ToString()
        {
            string s = "Prism(";
            switch (_direction)
            {
                case Direction.Up: s += "^"; break;
                case Direction.Down: s += "v"; break;
                case Direction.Left: s += "<"; break;
                case Direction.Right: s += ">"; break;
            }
            return s + ")";
        }



        public override XmlNode serialize(XmlDocument xdocument, XmlElement parent)
        {
            parent.SetAttribute("type", GetType().FullName);
            parent.SetAttribute("direction", Direction.ToString());
            return parent;
        }


        public static Prism deserialize(XmlElement node)
        {
            Prism obj = new Prism();

            if (node.HasAttribute("direction"))
            {
                obj.Direction = (Direction)Enum.Parse(typeof(Direction), node.GetAttribute("direction"), true);
            }

            return obj;
        }


        public void rotate()
        {
            Direction = (Direction)(Enum.GetValues(Direction.GetType()).Length == (int)Direction + 1 ? 0 : (int)Direction + 1);
        }
    }

}
