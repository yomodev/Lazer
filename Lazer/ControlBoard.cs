﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Lazer
{
    class ControlBoard : PictureBox
    {
        public Board board;
        private Bitmap _bitmap;
        private Point _overPoint = new(0, 0);
        private Point _selectedPoint;
        private Rectangle boardRectangle = new();
        private readonly Bitmap cursorOK;
        private readonly Bitmap cursorKO;
        private bool _editing = false;
        private bool isDragging = false;

        public event CheckDelegate OnCheck;
        public delegate void CheckDelegate(object sender, bool win);

        public event SelectionChangeDelegate OnSelectionChange;
        public delegate void SelectionChangeDelegate(object sender, Point selectedPoint);

        public ControlBoard()
        {
            LoadBoard();
            cursorOK = Texture.colorize(Texture.extract(TextureType.Pointer), Color.Lime);
            cursorKO = Texture.colorize(Texture.extract(TextureType.Pointer), Color.Red);
        }

        public void Redraw(bool showLaser)
        {
            _bitmap = board.draw(showLaser);
            Invalidate();
        }

        public bool Test()
        {
            bool win = board.process(true);
            Redraw(true);

            OnCheck?.Invoke(this, win);

            return win;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            if (_bitmap != null)
            {
                pe.Graphics.DrawImageUnscaled(_bitmap, 0, 0);

                pe.Graphics.DrawImageUnscaled(
                        (OverBlock.isMovable && !isDragging)
                        || IsEditing
                        || (isDragging && OverBlock is Empty) ? cursorOK : cursorKO,
                        _overPoint.X * Board.squareSize,
                        _overPoint.Y * Board.squareSize);
            }
        }

        public Block GetBlock(Point p)
        {
            return board.getBlock(p);
        }

        public Block GetBlock(int x, int y)
        {
            return board.getBlock(x, y);
        }

        public Block this[Point p]
        {
            get { return board[p.X, p.Y]; }
            set { board.setBlock(p.X, p.Y, value); }
        }

        public Block this[int x, int y]
        {
            get { return board[x, y]; }
            set { board.setBlock(x, y, value); }
        }

        public bool SetBlock(int x, int y, Block b)
        {
            return board.setBlock(x, y, b);
        }

        public bool SetBlock(Point p, Block b)
        {
            return board.setBlock(p, b);
        }

        public bool IsEditing
        {
            get { return _editing; }
            set
            {
                if (_editing != value)
                {
                    _editing = value;
                    if (/*_editing ||*/ isDragging) Redraw(false); else Test();
                }
            }
        }

        public Block SelectedBlock
        {
            get
            {
                return board[_selectedPoint];
            }
        }

        public Point SelectedPoint
        {
            get
            {
                if (_selectedPoint == Point.Empty)
                {
                    _selectedPoint = new Point(0, 0);
                }

                return _selectedPoint;
            }
        }

        public Block OverBlock
        {
            get
            {
                return board[_overPoint];
            }
        }

        public Point OverPoint
        {
            get
            {
                if (_overPoint == Point.Empty)
                {
                    _overPoint = new Point(0, 0);
                }

                return _overPoint;
            }

            set
            {
                if (_overPoint != value)
                {
                    _overPoint = value;
                    if (isDragging) Redraw(false); else Invalidate();
                }
            }
        }


        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Right && board[_overPoint].isMovable)
            {
                OverBlock.isPinned = !OverBlock.isPinned;
            }

            if (isDragging)
            {
                if (
                    OverPoint != SelectedPoint
                    &&
                    (IsEditing || (SelectedBlock.isMovable && OverBlock is Empty))
                    )
                {
                    board.swap(OverPoint, SelectedPoint);
                    _selectedPoint = OverPoint;
                }
                isDragging = false;
            }
            else
            {
                _selectedPoint = OverPoint;
            }

            /*if (isEditing) Redraw(false); else*/
            Test();

            OnSelectionChange?.Invoke(this, _selectedPoint);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            _selectedPoint = OverPoint;
            isDragging = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            Point p = new(e.X / Board.squareSize, e.Y / Board.squareSize);
            if (boardRectangle.Contains(p))
            {
                OverPoint = p;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            isDragging = false;
            Invalidate();
        }

        public void LoadBoard(XmlElement xmlElement = null)
        {
            //Checks to see that Control is pressed, but doesn't care about other modifier keys
            bool scramble = !((Control.ModifierKeys & Keys.Control) == Keys.Control);

            board = xmlElement == null ? Board.create() : Board.load(xmlElement, scramble);
            boardRectangle.Width = board.width;
            boardRectangle.Height = board.height;

            Test();
        }
    }
}
