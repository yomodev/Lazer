using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Data;
using System.ComponentModel;
using System.Linq;
using System.Xml;


namespace Lazer
{
    class Board
    {
        private Block[,] _board;
        private Bitmap _bitmap;
        public const int squareSize = 48;
        public string name;
        private TextureBrush brush;
        private int _width;
        private int _height;

        private bool invalidateCollections = false;
        private Dictionary<Emitter,Point> _emitters;
        private Dictionary<Receiver, Point> _receivers;
        private Dictionary<Splitter, Point> _splitters;



        private Board(int width, int height)
        {
            _width = width;
            _height = height;
            _board = new Block[_width, _height];
            _bitmap = new Bitmap(_width * squareSize, _height * squareSize, PixelFormat.Format32bppArgb);
            brush = new TextureBrush(Texture.extract(TextureType.Floor));
        }



        // TODO load level from xml file
        public static Board load(XmlElement xml, bool scramble)
        {
            return deserialize(xml, scramble);
        }


        public static Board create(int width = 10, int height = 10)
        {
            Board b = new Board(width, height);
            b.clear(true);
            return b;
        }


        // fill with empty objects and wall in the edges
        public void clear(bool wallEdges = false)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    if (wallEdges && ( x == 0 || y == 0 || x == _width - 1 || y == _height - 1) )
                    {
                        _board[x, y] = new Wall();
                    }
                    else
                    {
                        _board[x, y] = new Empty();
                    }
                }
            }
        }


        // this method is used to speedup the brute force solver
        // return true if it is necessary to process the board (calling process()) to determine if 
        // there are the minimum conditions to solve; return false if the board is unsolvable
        public bool analyze()
        {
            Block b;
            Emitter e;
            Receiver r;
            Splitter s;

            // foreach emitter check in its output direction if first object return transparent: if so stop processing
            foreach (KeyValuePair<Emitter, Point> kv in emitters)
            {
                e = kv.Key as Emitter;
                b = firstInDirection(e.Direction, (Point)kv.Value);
                if (b != null && b is Wall == false)
                {
                    Ray ray = new Ray();
                    ray[Ray.Opposite(e.Direction)] = Color.White;
                    if (b.Process(ray).None)
                    {
                        return false;
                    }
                }
            }

            // foreach receiver check in its input direction if first object return transparent: if so stop processing
            foreach (KeyValuePair<Receiver, Point> kv in receivers)
            {
                r = kv.Key as Receiver;
                b = firstInDirection(r.Direction, (Point)kv.Value);
                if (b != null && (b is Emitter || b is Mirror || b is Splitter))
                {
                    if (b.Process(new Ray(r.Color))[Ray.Opposite(r.Direction)] != r.Color)
                    {
                        return false;
                    }
                }
            }

            // foreach splitter check in its input direction if first object return transparent: if so stop processing
            foreach (KeyValuePair<Splitter, Point> kv in splitters)
            {
                s = kv.Key as Splitter;
                b = firstInDirection(s.Direction, (Point)kv.Value);
                if (b != null && b is Wall == false)
                {
                    if (b.Process(new Ray(Color.White))[Ray.Opposite(s.Direction)] == Color.Transparent)
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        // return first (non empty) element in a direction relative to point p
        private Block firstInDirection(Direction dir, Point p)
        {
            switch (dir)
            {
                case Direction.Up:
                    for (int i = p.Y-1; i >= 0; i--)
                    {
                        if (_board[p.X, i] is Empty == false)
                        {
                            return _board[p.X, i];
                        }
                    }
                    break;

                case Direction.Down:
                    for (int i = p.Y+1; i < height; i++)
                    {
                        if (_board[p.X, i] is Empty == false)
                        {
                            return _board[p.X, i];
                        }
                    }
                    break;

                case Direction.Left:
                    for (int i = p.X-1; i >= 0; i--)
                    {
                        if (_board[i, p.Y] is Empty == false)
                        {
                            return _board[i, p.Y];
                        }
                    }
                    break;

                case Direction.Right:
                    for (int i = p.X+1; i < width; i++)
                    {
                        if (_board[i, p.Y] is Empty == false)
                        {
                            return _board[i, p.Y];
                        }
                    }
                    break;
            }

            return null;
        }



        /* old processing logic method (slow)
        public bool process(bool prepareDraw = true)
        {
            List<Tuple<Point, Direction>> list = new List<Tuple<Point, Direction>>();
            Stack<Point> stack = new Stack<Point>();
            Rectangle area = new Rectangle(0, 0, _width, _height);
            List<Point> emitters = new List<Point>();
            List<Receiver> receivers = new List<Receiver>();
            Point p, n;
            IBlock b;
            Ray inRay, outRay;
            Tuple<Point, Direction> t;
            int step = 1;

            // reset
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    b = board[x, y];
                    b.process(new Ray(), prepareDraw);

                    if (b is Emitter)
                    {
                        emitters.Add(new Point(x, y));
                    }
                    else if (b is Receiver)
                    {
                        receivers.Add(b as Receiver);
                    }
                }
            }

            // store emitters location
            foreach (Point e in emitters)
            {
                list.Add(new Tuple<Point, Direction>(e, Direction.Up));
                list.Add(new Tuple<Point, Direction>(e, Direction.Down));
                list.Add(new Tuple<Point, Direction>(e, Direction.Left));
                list.Add(new Tuple<Point, Direction>(e, Direction.Right));
                stack.Push(e);
            }

            while (stack.Count > 0)
            {
                p = stack.Pop();
                b = board[p.X, p.Y];
                Debug.WriteLine(step + ": " + b.ToString() + " " + p.ToString());

                inRay = RayFromNeighbors(p);
                Debug.WriteLine("in: " + inRay);

                outRay = b.process(inRay, prepareDraw);
                Debug.WriteLine("out: " + outRay);

                if (outRay.hasUp)
                {
                    n = new Point(p.X, p.Y - 1);
                    t = new Tuple<Point, Direction>(n, Direction.Up);
                    if (!list.Contains(t) && area.Contains(n))
                    {
                        list.Add(t);
                        stack.Push(n);
                    }
                }

                if (outRay.hasDown)
                {
                    n = new Point(p.X, p.Y + 1);
                    t = new Tuple<Point, Direction>(n, Direction.Down);
                    if (!list.Contains(t) && area.Contains(n))
                    {
                        list.Add(t);
                        stack.Push(n);
                    }
                }

                if (outRay.hasLeft)
                {
                    n = new Point(p.X - 1, p.Y);
                    t = new Tuple<Point, Direction>(n, Direction.Left);
                    if (!list.Contains(t) && area.Contains(n))
                    {
                        list.Add(t);
                        stack.Push(n);
                    }
                }

                if (outRay.hasRight)
                {
                    n = new Point(p.X + 1, p.Y);
                    t = new Tuple<Point, Direction>(n, Direction.Right);
                    if (!list.Contains(t) && area.Contains(n))
                    {
                        list.Add(t);
                        stack.Push(n);
                    }
                }

                step++;
            }

            if (receivers.Count > 0 && receivers.All(receiver => receiver.lighting))
            {
                return true;
            }

            return false;
        }
        */


        // fast processing logic
        // search emitters and follow the outpur rays of every hitten object
        // then check if all receivers are lighting: if so return true
        public bool process(bool prepareDraw = true, string info = "")
        {
            List<String> list = new List<String>();
            Stack<Point> stack = new Stack<Point>();
            Rectangle area = new Rectangle(0, 0, _width, _height);
            List<Point> emitters = new List<Point>();
            List<Receiver> receivers = new List<Receiver>();
            Point p, n;
            Block b;
            Ray inRay, outRay;
            int step = 1;
            int x, y;
            Ray[,] outRays = new Ray[_width, _height];
            List<Block> toProcessForDraw = new List<Block>();
            String str;

            // init rays, search emitters and receivers
            for (x = 0; x < _width; x++)
            {
                for (y = 0; y < _height; y++)
                {
                    outRays[x, y] = new Ray();
                    b = _board[x, y];

                    if (b is Emitter)
                    {
                        emitters.Add(new Point(x, y));
                    }
                    else if (b is Receiver)
                    {
                        receivers.Add(b as Receiver);
                        // reset lighting
                        (b as Receiver).Reset();
                        //b.process(new Ray());
                    }
                    else
                    {
                        toProcessForDraw.Add(b);
                    }
                }
            }

            // store emitters location
            foreach (Point e in emitters)
            {
                /*list.Add(new Tuple<Point, Direction>(e, Direction.Up));
                list.Add(new Tuple<Point, Direction>(e, Direction.Down));
                list.Add(new Tuple<Point, Direction>(e, Direction.Left));
                list.Add(new Tuple<Point, Direction>(e, Direction.Right));*/
                stack.Push(e);
            }

            // put coordinates of every item to process in a stack
            while (stack.Count > 0)
            {
                p = stack.Pop();
                x = p.X;
                y = p.Y;

                b = _board[x, y];
                // if processed remove from redraw queue
                toProcessForDraw.Remove(b);
                Debug.WriteLine(info + step + ": " + b.ToString() + " " + p.ToString());

                // get rays for this coords
                inRay = new Ray(outRays[x, y].up, outRays[x, y].down, outRays[x, y].left, outRays[x, y].right);
                Debug.WriteLine("in: " + inRay);

                str = p.ToString() + inRay.ToString();
                if (list.Contains(str))
                {
                    continue;
                }
                else
                {
                    list.Add(str);
                }

                // send rays to element in this coords
                outRay = b.Process(inRay);
                Debug.WriteLine("out: " + outRay);

                // if the element produced output rays (non transparent)
                // update output ray table
                // add next point to stack
                // remember to not process the same item 2 times given the same input ray
                if (outRay.HasUp)
                {
                    n = new Point(p.X, p.Y - 1);
                    if (area.Contains(n))
                    {
                        outRays[n.X, n.Y].down = outRay.up;
                        stack.Push(n);
                    }
                }

                if (outRay.HasDown)
                {
                    n = new Point(p.X, p.Y + 1);
                    if (area.Contains(n))
                    {
                        outRays[n.X, n.Y].up = outRay.down;
                        stack.Push(n);
                    }
                }

                if (outRay.HasLeft)
                {
                    n = new Point(p.X - 1, p.Y);
                    if (area.Contains(n))
                    {
                        outRays[n.X, n.Y].right = outRay.left;
                        stack.Push(n);
                    }
                }

                if (outRay.HasRight)
                {
                    n = new Point(p.X + 1, p.Y);
                    if (area.Contains(n))
                    {
                        outRays[n.X, n.Y].left = outRay.right;
                        stack.Push(n);
                    }
                }

                step++;
            }

            // clean board
            if (prepareDraw && toProcessForDraw.Count > 0)
            {
                toProcessForDraw.ForEach(item => item.Process(new Ray()));
            }

            // solved?
            if (receivers.Count > 0 && receivers.All(receiver => receiver.Lighting))
            {
                return true;
            }

            return false;
        }



        // fast processing logic
        // search emitters and follow the outpur rays of every hitten object
        // then check if all receivers are lighting: if so return true
        public bool old_process(bool prepareDraw = true, string info = "")
        {
            List<Tuple<Point, Direction>> list = new List<Tuple<Point, Direction>>();
            Stack<Point> stack = new Stack<Point>();
            Rectangle area = new Rectangle(0, 0, _width, _height);
            List<Point> emitters = new List<Point>();
            List<Receiver> receivers = new List<Receiver>();
            Point p, n;
            Block b;
            Ray inRay, outRay;
            Tuple<Point, Direction> t;
            int step = 1;
            int x, y;
            Ray[,] outRays = new Ray[_width, _height];
            List<Block> toProcessForDraw = new List<Block>();

            // init rays, search emitters and receivers
            for (x = 0; x < _width; x++)
            {
                for (y = 0; y < _height; y++)
                {
                    outRays[x, y] = new Ray();
                    b = _board[x, y];

                    if (b is Emitter)
                    {
                        emitters.Add(new Point(x, y));
                    }
                    else if (b is Receiver)
                    {
                        receivers.Add(b as Receiver);
                        // reset lighting
                        (b as Receiver).Reset();
                        //b.process(new Ray());
                    }
                    else
                    {
                        toProcessForDraw.Add(b);
                    }
                }
            }

            // store emitters location
            foreach (Point e in emitters)
            {
                list.Add(new Tuple<Point, Direction>(e, Direction.Up));
                list.Add(new Tuple<Point, Direction>(e, Direction.Down));
                list.Add(new Tuple<Point, Direction>(e, Direction.Left));
                list.Add(new Tuple<Point, Direction>(e, Direction.Right));
                stack.Push(e);
            }

            // put coordinates of every item to process in a stack
            while (stack.Count > 0)
            {
                p = stack.Pop();
                x = p.X;
                y = p.Y;

                b = _board[x, y];
                // if processed remove from redraw queue
                toProcessForDraw.Remove(b);
                Debug.WriteLine(info + step + ": " + b.ToString() + " " + p.ToString());

                // get rays for this coords
                inRay = new Ray(outRays[x, y].up, outRays[x, y].down, outRays[x, y].left, outRays[x, y].right);
                Debug.WriteLine("in: " + inRay);

                // send rays to element in this coords
                outRay = b.Process(inRay);
                Debug.WriteLine("out: " + outRay);

                // if the element produced output rays (non transparent)
                // update output ray table
                // add next point to stack
                // remember to not process the same item 2 times given the same input ray
                if (outRay.HasUp)
                {
                    n = new Point(p.X, p.Y - 1);
                    if (area.Contains(n))
                    {
                        outRays[n.X, n.Y].down = outRay.up;
                        t = new Tuple<Point, Direction>(n, Direction.Up);
                        if (!list.Contains(t))
                        {
                            list.Add(t);
                            stack.Push(n);
                        }
                    }
                }

                if (outRay.HasDown)
                {
                    n = new Point(p.X, p.Y + 1);
                    if (area.Contains(n))
                    {
                        outRays[n.X, n.Y].up = outRay.down;
                        t = new Tuple<Point, Direction>(n, Direction.Down);
                        if (!list.Contains(t))
                        {
                            list.Add(t);
                            stack.Push(n);
                        }
                    }
                }

                if (outRay.HasLeft)
                {
                    n = new Point(p.X - 1, p.Y);
                    if (area.Contains(n))
                    {
                        outRays[n.X, n.Y].right = outRay.left;
                        t = new Tuple<Point, Direction>(n, Direction.Left);
                        if (!list.Contains(t))
                        {
                            list.Add(t);
                            stack.Push(n);
                        }
                    }
                }

                if (outRay.HasRight)
                {
                    n = new Point(p.X + 1, p.Y);
                    if (area.Contains(n))
                    {
                        outRays[n.X, n.Y].left = outRay.right;
                        t = new Tuple<Point, Direction>(n, Direction.Right);
                        if (!list.Contains(t))
                        {
                            list.Add(t);
                            stack.Push(n);
                        }
                    }
                }

                step++;
            }

            // clean board
            if (prepareDraw && toProcessForDraw.Count > 0)
            {
                toProcessForDraw.ForEach(item => item.Process(new Ray()));
            }

            // solved?
            if (receivers.Count > 0 && receivers.All(receiver => receiver.Lighting))
            {
                return true;
            }

            return false;
        }


        /* retrieve output rays from element @point (old version block items stored output rays)
        private Ray RayFromNeighbors(Point p)
        {
            Ray r = new Ray();
            
            if (p.Y > 0)
            {
                r.up = board[p.X, p.Y - 1].getOutRays().down;
            }

            if( p.Y < _height-1)
            {
                r.down = board[p.X, p.Y + 1].getOutRays().up;
            }

            if (p.X > 0)
            {
                r.left = board[p.X - 1, p.Y].getOutRays().right;
            }

            if (p.X < _width-1)
            {
                r.right = board[p.X + 1, p.Y].getOutRays().left;
            }

            return r;
        }*/


        // draw floor the call each item draw method
        public Bitmap draw(bool showLaser = true)
        {
            Graphics g = Graphics.FromImage(_bitmap);
            g.FillRectangle(brush, new Rectangle(0, 0, _bitmap.Width, _bitmap.Height));
            TextureBrush pin = new TextureBrush(Texture.extract(TextureType.Pin));
            Block b;
            Rectangle rect;
            
            #if DEBUG
                Font f = new Font(FontFamily.GenericMonospace, 8);
            #endif

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    b = _board[x, y];
                    rect = new Rectangle(x*squareSize, y*squareSize, squareSize, squareSize);

                    b.Draw(g, rect, showLaser);
                    
                    #if DEBUG
                        g.DrawString(x + "," + y, f, Brushes.White, x * squareSize, y * squareSize);
                    #endif

                    if (b.isPinned)
                    {
                        g.FillRectangle(pin, rect);
                    }
                }
            }

            g.Dispose();
            return _bitmap;
        }



        // save screenshot
        public void saveScreenshot(string path)
        {
            process(true, "save " + path);
            Bitmap b = draw();
            b.Save(path, ImageFormat.Png);
        }



        // swap items in the board given their coords
        public void swap(Point src, Point dest)
        {
            Block b = _board[dest.X, dest.Y];
            _board[dest.X, dest.Y] = _board[src.X, src.Y];
            _board[src.X, src.Y] = b;
        }


        public int width
        {
            get { return _width; }
        }

        public int height
        {
            get { return _height; }
        }


        public Block getBlock(Point p)
        {
            return _board[p.X, p.Y];
        }

        public Block getBlock(int x, int y)
        {
            return _board[x, y];
        }

        public Block this[Point p]
        {
            get { return _board[p.X, p.Y]; }
            set { setBlock(p.X, p.Y, value); }
        }

        public Block this[int x, int y]
        {
            get { return _board[x, y]; }
            set { setBlock(x, y, value); }
        }



        public bool setBlock(int x, int y, Block b)
        {
            if (x >= 0 && x < _width && y >= 0 && y < _height)
            {
                invalidateCollections = true;
                _board[x, y] = b;
                return true;
            }

            return false;
        }



        public bool setBlock(Point p, Block b)
        {
            return setBlock(p.X, p.Y, b);
        }



        public Board copy()
        {
            return deserialize(serialize(new XmlDocument()));
        }



        // store receivers, emitters and splitters location
        // used to speedup solver analyser
        private void updateCollections()
        {
            _emitters = new Dictionary<Emitter,Point>();
            _receivers = new Dictionary<Receiver, Point>();
            _splitters = new Dictionary<Splitter, Point>();
            Block b;

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    b = _board[x,y];
                    if (b is Emitter)
                    {
                        _emitters[b as Emitter] = new Point(x,y);
                    }
                    else if (b is Receiver)
                    {
                        _receivers[b as Receiver] = new Point(x,y);
                    }
                    else if (b is Splitter)
                    {
                        _splitters[b as Splitter] = new Point(x, y);
                    }
                }
            }
            invalidateCollections = false;
        }


        Dictionary<Emitter,Point> emitters
        {
            get
            {
                if (_emitters == null || invalidateCollections)
                {
                    updateCollections();
                }
                return _emitters;
            }
        }


        Dictionary<Receiver,Point> receivers
        {
            get
            {
                if (_receivers == null || invalidateCollections)
                {
                    updateCollections();
                }
                return _receivers;
            }
        }


        Dictionary<Splitter,Point> splitters
        {
            get
            {
                if (_splitters == null || invalidateCollections)
                {
                    updateCollections();
                }
                return _splitters;
            }
        }


        /*
        public void saveLevel(string filename)
        {
            serialize().Save(filename);
        }*/



        public XmlElement serialize(XmlDocument xdocument)
        {
            XmlElement xboard = xdocument.CreateElement("board");
            xboard.SetAttribute("name", name);
            xboard.SetAttribute("width", width.ToString());
            xboard.SetAttribute("height", height.ToString());
            Block b;

            for (int x = 1; x <= width; x++)
            {
                for (int y = 1; y <= height; y++)
                {
                    b = getBlock(x-1, y-1);
                    
                    XmlElement xblock = xdocument.CreateElement("block");
                    xblock.SetAttribute("x", x.ToString());
                    xblock.SetAttribute("y", y.ToString());

                    XmlNode xnode = b.Serialize(xdocument, xblock);
                    if (xnode != null)
                    {
                        xboard.AppendChild(xnode);
                    }
                }
            }

            return xboard;
        }


        public static Board deserialize(XmlElement xboard, bool scramble = false)
        {
            Board board = create(Convert.ToInt32(xboard.GetAttribute("width")), Convert.ToInt32(xboard.GetAttribute("height")));
            Block block;
            board.name = xboard.GetAttribute("name");
            List<Point> empty = new List<Point>();
            List<Point> movable = new List<Point>();

            foreach (XmlElement xblock in xboard.GetElementsByTagName("block"))
            {
                string typeName = xblock.GetAttribute("type");
                Type type = Type.GetType(typeName);
                type = type == null ? Type.GetType("Lazer." + char.ToUpper(typeName[0]) + typeName.Substring(1)) : type;
                if (type == null)
                {
                    throw new Exception("invalid type " + typeName);
                }

                block = (Block)type.GetMethod("deserialize").Invoke(null, new object[] {xblock});
                int x = Convert.ToInt32(xblock.GetAttribute("x")) - 1;
                int y = Convert.ToInt32(xblock.GetAttribute("y")) - 1;
                board.setBlock(x, y, block);

                // store for scramble
                if (block.isMovable && block is Empty == false)
                {
                    movable.Add(new Point(x, y));
                }
            }

            if (scramble)
            {
                for (int x = 0; x < board.width; x++)
                {
                    for (int y = 0; y < board.height; y++)
                    {
                        block = board[x, y];
                        if (block is Empty)
                        {
                            empty.Add( new Point(x, y) );
                        }
                    }
                }

                Random random = new Random();
                while (movable.Count > 0)
                {
                    Point p1 = movable[movable.Count - 1];
                    Point p2 = empty[random.Next(0, empty.Count-1)];
                    movable.Remove(p1);
                    empty.Remove(p2);
                    board.swap(p1, p2);
                    empty.Add(p1);
                }

                if (board.process(false))
                {
                    return deserialize(xboard, scramble);
                }
            }

            return board;
        }


    }
}
