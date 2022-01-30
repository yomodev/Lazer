using System;
using System.Drawing;
using System.Xml;

namespace Lazer
{
    class Emitter : Block, IRotable
    {
        private Direction _direction = Direction.Right;
        private Color _color = Color.White;
        protected TextureBrush[] brushes = new TextureBrush[3];

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

        public Emitter() : base()
        {
            isMovable = true;
        }

        // emitter just return an array of color c @ specified direction
        public Emitter(Direction dir, Color c) : this()
        {
            Direction = dir;
            Color = c;
        }

        // emitter just return an array of color c @ specified direction
        public override Ray Process(Ray input)
        {
            return new Ray(
                _direction == Direction.Up ? _color : Color.Transparent,
                _direction == Direction.Down ? _color : Color.Transparent,
                _direction == Direction.Left ? _color : Color.Transparent,
                _direction == Direction.Right ? _color : Color.Transparent);
        }

        public override void Draw(Graphics g, Rectangle rect, bool showLaser = true)
        {
            if (needRefresh)
            {
                int angle = 0;
                switch (_direction)
                {
                    case Direction.Up: angle = 270; break;
                    case Direction.Left: angle = 180; break;
                    case Direction.Right: angle = 0; break;
                    case Direction.Down: angle = 90; break;
                }

                brushes[0] = new TextureBrush(Texture.extract(TextureType.SpotBody, angle));
                brushes[1] = new TextureBrush(Texture.colorize(Texture.extract(TextureType.SpotLight, angle), _color));
                brushes[2] = showLaser ? new TextureBrush(Texture.colorize(Texture.extract(TextureType.RayHalfGradient, 180 + angle), _color)) : null;

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
            if (obj is Emitter)
            {
                Emitter o = obj as Emitter;
                if(o.color == color && o.direction == direction)
                {
                    return 0;
                }
            }
            return obj == null ? 1 : -1;
        }*/

        public override string ToString()
        {
            string s = "Emitter(";
            switch (_direction)
            {
                case Direction.Up: s += "^"; break;
                case Direction.Down: s += "v"; break;
                case Direction.Left: s += "<"; break;
                case Direction.Right: s += ">"; break;
            }

            return s + ", " + _color + ")";
        }
        public override XmlNode Serialize(XmlDocument xdocument, XmlElement parent)
        {
            parent.SetAttribute("type", GetType().FullName);
            parent.SetAttribute("direction", Direction.ToString());
            parent.SetAttribute("color", ColorTranslator.ToHtml(Color));
            return parent;
        }

        public static Emitter Deserialize(XmlElement node)
        {
            Emitter obj = new Emitter();

            if (node.HasAttribute("direction"))
            {
                obj.Direction = (Direction)Enum.Parse(typeof(Direction), node.GetAttribute("direction"), true);
            }

            if (node.HasAttribute("color"))
            {
                obj.Color = ColorTranslator.FromHtml(node.GetAttribute("color"));
            }

            return obj;
        }

        public void Rotate()
        {
            Direction = (Direction)(Enum.GetValues(Direction.GetType()).Length == (int)Direction + 1 ? 0 : (int)Direction + 1);
        }
    }
}
