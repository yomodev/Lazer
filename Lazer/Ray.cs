using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Lazer
{
    class Ray
    {


        public Color up, down, left, right;

        public Ray(Color up, Color down, Color left, Color right)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
        }


        public Ray()
        {
            up = Color.Transparent;
            down = Color.Transparent;
            left = Color.Transparent;
            right = Color.Transparent;
        }



        public Ray(Color c)
        {
            up = c;
            down = c;
            left = c;
            right = c;
        }


        public Ray(Ray r)
        {
            up = r.up;
            down = r.down;
            left = r.left;
            right = r.right;
        }


        public static Direction opposite(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up: return Direction.Down;
                case Direction.Left: return Direction.Right;
                case Direction.Right: return Direction.Left;
                default: return Direction.Up;
            }
        }


        public override string ToString()
        {
            return "up: " + up.ToString() + ", down: " + down.ToString() + ", left: " + left.ToString() + ", right: " + right.ToString();
        }


        public bool hasUp { get { return up != Color.Transparent; } }
        public bool hasDown { get { return down != Color.Transparent; } }
        public bool hasLeft { get { return left != Color.Transparent; } }
        public bool hasRight { get { return right != Color.Transparent; } }
        public bool any
        {
            get
            {
                return
                    up != Color.Transparent
                    ||
                    down != Color.Transparent
                    ||
                    left != Color.Transparent
                    ||
                    right != Color.Transparent;
            }
        }

        public bool none
        {
            get
            {
                return
                    up == Color.Transparent
                    &&
                    down == Color.Transparent
                    &&
                    left == Color.Transparent
                    &&
                    right == Color.Transparent;
            }
        }


        public Color this[Direction dir]
        {
            get
            {
                switch (dir)
                {
                    case Direction.Up: return up;
                    case Direction.Left: return left;
                    case Direction.Right: return right;
                    default: return down;
                }
            }

            set
            {
                switch (dir)
                {
                    case Direction.Up: up = value; break;
                    case Direction.Left: left = value; break;
                    case Direction.Right: right = value; break;
                    default: down = value; break;
                }
            }
        }



        public Color Coalesce(Direction d, Color c)
        {
            Color p = this[d];
            return p == Color.Transparent ? c : p;
        }




    }
}
