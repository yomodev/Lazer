using System;
using System.Drawing;
using System.ComponentModel;
using System.Xml;


namespace Lazer
{
    class Receiver : Block, IRotable
    {
        private Color _color = Color.White;
        protected TextureBrush[] brushes = new TextureBrush[3];
        private bool _lighting = false;
        private Direction _direction = Direction.Left;


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


        [Browsable(false)]
        public bool Lighting
        {
            get { return _lighting; }
            set
            {
                if (_lighting != value)
                {
                    _lighting = value;
                    needRefresh = true;
                }
            }
        }



        public Receiver() : base()
        {
            isMovable = false;
        }


        // if receive a ray of color c in specified direction it will light
        public Receiver(Direction dir, Color c) : this()
        {
            Direction = dir;
            Color = c;
        }



        // if receive a ray of color c in specified direction it will light
        public override Ray process(Ray input)
        {
            Lighting = input[_direction] == _color;
            return new Ray();
        }



        public override void draw(Graphics g, Rectangle rect, bool showLaser = true)
        {
            // speedup drawing
            if (needRefresh)
            {
                int angle = 0;
                switch (_direction)
                {
                    case Direction.Up: angle = 270; break;
                    case Direction.Left: angle = 180; break;
                    case Direction.Down: angle = 90; break;
                }

                //Bitmap b = Texture.extract(_lighting ? TextureType.ReceiverOn : TextureType.ReceiverOff, angle);
                //brush = new TextureBrush(Texture.colorize(b, _color));

                brushes[0] = _lighting ? new TextureBrush(Texture.colorize(Texture.extract(TextureType.RayHalfGradient, angle+180), _color)) : null;
                brushes[1] = new TextureBrush(Texture.colorize(Texture.extract(_lighting ? TextureType.DropOn : TextureType.DropOff), _color));
                brushes[2] = new TextureBrush(Texture.extract(_lighting ? TextureType.SmileHappy : TextureType.SmileSad));
                
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
            if (obj is Receiver)
            {
                Receiver o = obj as Receiver;
                if (o.color == color && o.direction == direction)
                {
                    return 0;
                }
            }
            return obj == null ? 1 : -1;
        }*/



        public override string ToString()
        {
            string s = "Receiver(";
            switch (_direction)
            { 
                case Direction.Up: s += "^"; break;
                case Direction.Down: s += "v"; break;
                case Direction.Left: s += "<"; break;
                case Direction.Right: s += ">"; break;
            }
            return s + ", " + _color + ")";
        }


        public void reset()
        {
            Lighting = false;
        }



        public override XmlNode serialize(XmlDocument xdocument, XmlElement parent)
        {
            parent.SetAttribute("type", GetType().FullName);
            parent.SetAttribute("direction", Direction.ToString());
            parent.SetAttribute("color", ColorTranslator.ToHtml(Color));
            return parent;
        }


        public static Block deserialize(XmlElement node)
        {
            Receiver obj = new Receiver();

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


        public void rotate()
        {
            Direction = (Direction)(Enum.GetValues(Direction.GetType()).Length == (int)Direction + 1 ? 0 : (int)Direction + 1);
        }
    }
}
