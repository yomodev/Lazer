using System;
using System.Drawing;
using System.Xml;

namespace Lazer
{
    class Multiplier : Block, IRotable
    {
        protected TextureBrush[] brushes = new TextureBrush[3];
        private Direction _direction = Direction.Left;
        private Color DefaultColor = Color.Gray;
        private Color InnerColor = Color.Gray;


        public Multiplier() : base()
        {
            isMovable = true;
        }


        public Multiplier(Direction dir) : this()
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
            InnerColor = input[Direction] == Color.Transparent ? DefaultColor : input[Direction];
            output = new Ray(InnerColor);
            /*output.up = input.Coalesce(Direction.Up, InnerColor);
            output.down = input.Coalesce(Direction.Down, InnerColor);
            output.left = input.Coalesce(Direction.Left, InnerColor);
            output.right = input.Coalesce(Direction.Right, InnerColor);*/
            output[Direction] = Color.Transparent;
            
            /*
            // if there is more than one input stop!
            Ray test = new Ray(input);
            test[Direction] = Color.Transparent;
            if (test.none)
            {
                InnerColor = input[Direction] == Color.Transparent ? DefaultColor : input[Direction];
                output = new Ray(InnerColor);
                output[Direction] = Color.Transparent;
            }
            else
            {
                output = new Ray();
                InnerColor = DefaultColor;
            }
            */

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

                brushes[0] = new TextureBrush(Texture.colorize(Texture.extract(TextureType.ColorBlock), InnerColor));
                //brushes[1] = new TextureBrush(Texture.colorize(Texture.extract(TextureType.Multiply), InnerColor));
                brushes[1] = new TextureBrush(Texture.extract(TextureType.Multiply));
                brushes[2] = new TextureBrush(Texture.colorize(Texture.extract(TextureType.InputArrow, angle), Color.Gray));
                
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
            string s = "Multiplier(";
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


        public static Multiplier deserialize(XmlElement node)
        {
            Multiplier obj = new Multiplier();
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
