using System;
using System.Drawing;
using System.Xml;


namespace Lazer
{
    class Splitter : Block, IRotable
    {
        private Direction _direction = Direction.Up;
        private TextureBrush brush;
        private TextureBrush laser1;
        private TextureBrush laser2;



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
        


        public Splitter() :base()
        { 
            isMovable = true;
        }


        // if hitten by a ray in its input direction return 2 perpendicolar rays as output (same color as the input)
        public Splitter(Direction dir) : this()
        {
            Direction = dir;
        }



        // if hitten by a ray in its input direction return 2 perpendicolar rays as output (same color as the input)
        public override Ray process(Ray input)
        {
            output = new Ray();
            switch (_direction)
            {
                case Direction.Up: 
                    output  = new Ray(Color.Transparent, Color.Transparent, input.up, input.up); 
                    break;

                case Direction.Down: 
                    output = new Ray(Color.Transparent, Color.Transparent, input.down, input.down); 
                    break;

                case Direction.Left: 
                    output = new Ray(input.left, input.left, Color.Transparent, Color.Transparent);
                    break;

                case Direction.Right: 
                    output = new Ray(input.right, input.right, Color.Transparent, Color.Transparent); 
                    break;
            }

            needRefresh = true;
            return output;
        }



        public override void draw(Graphics g, Rectangle rect, bool showLaser = true)
        {
            if (needRefresh)
            {
                int angle = 0;
                switch (_direction)
                {
                    case Direction.Up: angle = 270; break;
                    case Direction.Left: angle = 180; break;
                    case Direction.Down: angle = 90; break;
                }

                brush = new TextureBrush(Texture.extract(TextureType.Splitter, angle));

                laser1 = null;
                Color c = Color.Transparent;

                if (output.hasLeft && output.left == output.right)
                {
                    c = output.left;
                    laser1 = new TextureBrush(Texture.colorize(Texture.extract(TextureType.Ray), c));
                }
                else if (output.hasUp && output.up == output.down)
                {
                    c = output.up;
                    laser1 = new TextureBrush(Texture.colorize(Texture.extract(TextureType.Ray, 90), c));
                }

                laser2 = null;
                if (laser1 != null)
                {
                    switch (_direction)
                    {
                        case Direction.Up:
                            laser2 = new TextureBrush(Texture.colorize(Texture.extract(TextureType.RayHalf, 270), c));
                            break;

                        case Direction.Down:
                            laser2 = new TextureBrush(Texture.colorize(Texture.extract(TextureType.RayHalf, 90), c));
                            break;

                        case Direction.Left:
                            laser2 = new TextureBrush(Texture.colorize(Texture.extract(TextureType.RayHalf, 180), c));
                            break;

                        case Direction.Right:
                            laser2 = new TextureBrush(Texture.colorize(Texture.extract(TextureType.RayHalf, 0), c));
                            break;
                    }
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
            if (obj is Splitter && ((Splitter)obj).direction == direction)
            {
                return 0;
            }
            return obj == null ? 1 : -1;
        }*/



        public override string ToString()
        {
            string s = "Splitter(";
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


        public static Splitter deserialize(XmlElement node)
        {
            Splitter obj = new Splitter();

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
