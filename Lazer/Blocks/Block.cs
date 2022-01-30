using System;
using System.ComponentModel;
using System.Drawing;
using System.Xml;


namespace Lazer
{
    public enum Angle
    {
        [Description("/")]
        NESW, // /
        [Description("\\")]
        NWSE  // \
    };

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    };

    public enum Mode
    {
        Horizontal,
        Vertical,
        Direction
    };

    class Block : IComparable<Block>
    {
        public bool isMovable = false;
        public bool isPinned = false;
        protected bool needRefresh = true;
        protected Ray output = new Ray();
        public virtual void Draw(Graphics g, Rectangle rect, bool showLaser = true)
        { }

        public virtual Ray Process(Ray ray)
        {
            return new Ray(Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent);
        }

        public virtual int CompareTo(Block obj)
        {
            return this.ToString().CompareTo(obj.ToString());
        }

        public virtual XmlNode Serialize(XmlDocument xdocument, XmlElement parent)
        {
            return parent;
        }

        /*public static void deserialize(XmlElement node, object obj)
        {
            if (node.HasAttribute("isPinned"))
            {
                obj.Color = ColorTranslator.FromHtml(node.GetAttribute("color"));
            }

            return obj;
        }*/
    }
}