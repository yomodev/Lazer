using System;
using System.Drawing;
using System.Xml;

namespace Lazer
{
    class Adder : Block, IRotable
    {
        protected TextureBrush[] brushes = new TextureBrush[3];
        private Direction _direction = Direction.Left;
        private Color DefaultColor = Color.Gray;


        public Adder() : base()
        {
            isMovable = true;
        }


        public Adder(Direction dir) : this()
        {
            Direction = dir;
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
        public override Ray process(Ray input)
        {
            output = new Ray();
            Ray x = new Ray(input);
            x[Direction] = Color.Transparent;
            int r = (x.up == Color.Transparent ? 0 : x.up.R) + (x.down == Color.Transparent ? 0 : x.down.R) + (x.left == Color.Transparent ? 0 : x.left.R) + (x.right == Color.Transparent ? 0 : x.right.R);
            int g = (x.up == Color.Transparent ? 0 : x.up.G) + (x.down == Color.Transparent ? 0 : x.down.G) + (x.left == Color.Transparent ? 0 : x.left.G) + (x.right == Color.Transparent ? 0 : x.right.G) ;
            int b = (x.up == Color.Transparent ? 0 : x.up.B) + (x.down == Color.Transparent ? 0 : x.down.B) + (x.left == Color.Transparent ? 0 : x.left.B) + (x.right == Color.Transparent ? 0 : x.right.B);

            if (input[Direction] == Color.Transparent)
            {
                output[Direction] = Color.FromArgb(0, Math.Min(r, 255), Math.Min(g, 255), Math.Min(b, 255));
            }

            needRefresh = true;
            return output;
        }



        public override void draw(Graphics g, Rectangle rect, bool showLaser = true)
        {
            if (needRefresh)
            {
                int angle = 0;
                switch (Direction)
                {
                    case Direction.Up: angle = 180; break;
                    case Direction.Down: angle = 0; break;
                    case Direction.Left: angle = 90; break;
                    case Direction.Right: angle = 270; break;
                }

                Color c = output[Direction];
                c = c == Color.Transparent || c.ToArgb() == 0 ? DefaultColor : c;
                brushes[0] = new TextureBrush(Texture.colorize(Texture.extract(TextureType.ColorBlock), c));
                //brushes[1] = new TextureBrush(Texture.colorize(Texture.extract(TextureType.Plus), c));
                brushes[1] = new TextureBrush(Texture.extract(TextureType.Plus));
                brushes[2] = new TextureBrush(Texture.colorize(Texture.extract(TextureType.OutputArrow, angle), Color.Gray));
                
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


        public override string ToString()
        {
            string s = "Adder(";
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


        public static Adder deserialize(XmlElement node)
        {
            Adder obj = new Adder();
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
