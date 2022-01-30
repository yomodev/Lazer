using System;
using System.Drawing;
using System.Xml;

namespace Lazer
{
    class FilterHV : Block, IRotable
    {
        private TextureBrush laser;
        protected TextureBrush[] brushes = new TextureBrush[2];
        private Mode _mode = Mode.Direction;
        private Direction _direction = Direction.Left;
        Color innerColor = Color.Transparent;

        public FilterHV() :base()
        {
            isMovable = false;
        }

        public FilterHV(Mode m, Direction dir) : this()
        {
            Mode = m;
            Direction = dir;
        }

        public Mode Mode
        {
            get { return _mode; }
            set
            {
                if (_mode != value)
                {
                    _mode = value;
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
            innerColor = Color.Transparent;
            if (_mode == Mode.Horizontal)
            {
                output = new Ray(
                    Color.Transparent, 
                    Color.Transparent, 
                    input.left == Color.Transparent ? input.right : input.left, 
                    input.right == Color.Transparent ? input.left : input.right);
                innerColor = output.HasLeft ? output.left : output.right;
            }
            else if (_mode == Mode.Vertical)
            {
                output = new Ray(
                    input.up == Color.Transparent ? input.down : input.up, 
                    input.down == Color.Transparent ? input.up : input.down, 
                    Color.Transparent, 
                    Color.Transparent);
                innerColor = output.HasUp ? output.up : output.down;
            }
            else //if (_mode == Mode.Direction)
            {
                output = new Ray();
                if (input[Direction] == Color.Transparent)
                {
                    output[Direction] = input[Ray.Opposite(Direction)];
                    innerColor = output[Direction];
                }
                else
                {
                    output[Direction] = input[Direction];
                    innerColor = input[Ray.Opposite(Direction)];
                }
            }
            
            needRefresh = true;
            return output;
        }

        public override void Draw(Graphics g, Rectangle rect, bool showLaser = true)
        {
            if (needRefresh)
            {
                int angle = _mode == Mode.Horizontal ? 0 : 90;
                brushes[1] = null;
                
                if (_mode == Mode.Direction)
                {
                    switch (_direction)
                    {
                        case Direction.Up:
                            brushes[1] = new TextureBrush(Texture.colorize(Texture.extract(TextureType.FilterHVArrows, 270),Color.Turquoise));
                            angle = 90;
                            break;
                        case Direction.Down:
                            brushes[1] = new TextureBrush(Texture.colorize(Texture.extract(TextureType.FilterHVArrows, 90), Color.Turquoise));
                            angle = 90;
                            break;
                        case Direction.Left:
                            brushes[1] = new TextureBrush(Texture.colorize(Texture.extract(TextureType.FilterHVArrows, 180), Color.Turquoise));
                            angle = 0;
                            break;
                        case Direction.Right:
                            brushes[1] = new TextureBrush(Texture.colorize(Texture.extract(TextureType.FilterHVArrows, 0), Color.Turquoise));
                            angle = 0;
                            break;
                    }
                }
                
                brushes[0] = new TextureBrush(Texture.extract(TextureType.FilterHV, angle));
                
                laser = output.None ? null : new TextureBrush(Texture.colorize(Texture.extract(TextureType.Ray, output.HasLeft || output.HasRight ? 0 : 90), innerColor));
                
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
            string s = "FilterHV("; 
            
            if (_mode == Mode.Direction)
            {
                switch (_direction)
                {
                    case Direction.Up: s += "^"; break;
                    case Direction.Down: s += "v"; break;
                    case Direction.Left: s += "<"; break;
                    case Direction.Right: s += ">"; break;
                }
            }
            else
            {
                s += _mode == Mode.Horizontal ? "-" : "|";
            }

            return s + ")";
        }

        public override XmlNode Serialize(XmlDocument xdocument, XmlElement parent)
        {
            parent.SetAttribute("type", GetType().FullName);
            parent.SetAttribute("mode", Mode.ToString());
            if (Mode == Mode.Direction)
            {
                parent.SetAttribute("direction", Direction.ToString());
            }
            return parent;
        }

        public static FilterHV Deserialize(XmlElement node)
        {
            FilterHV obj = new FilterHV();
            if (node.HasAttribute("mode"))
            {
                obj.Mode = (Mode)Enum.Parse(typeof(Mode), node.GetAttribute("mode"), true);
            }

            if (node.HasAttribute("direction"))
            {
                obj.Direction = (Direction)Enum.Parse(typeof(Direction), node.GetAttribute("direction"), true);
            }

            return obj;
        }

        public void Rotate()
        {
            if (Mode == Mode.Direction)
            {
                Direction = (Direction)(Enum.GetValues(Direction.GetType()).Length == (int)Direction + 1 ? 0 : (int)Direction + 1);
            }
            else
            {
                Mode = Mode == Mode.Horizontal ? Mode.Vertical : Mode.Horizontal;
            }
        }
    }
}
