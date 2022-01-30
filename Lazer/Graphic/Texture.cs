using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace Lazer
{
    // texture names from top left to right down
    public enum TextureType
    {
        Floor,
        Wall,
        Splitter,
        Emitter,
        ReceiverOn,
        ReceiverOff,
        RayHalf,
        Ray,
        Ray45,
        ColorBlock,
        Prism,
        Tetra,
        TetraBase,
        TetraTriangle,
        InputArrow,
        OutputArrow,
        FilterHVArrows,
        Filter90Arrows,
        Ray90,
        RayHalfGradient,
        Pointer,
        Plus,
        Minus,
        Multiply,
        Pin,
        Mirror,
        FilterHV,
        Filter90,
        Hole1,
        Hole2,  
        DropOff,
        DropOn,
        SmileSad,
        SmileHappy,
        SpotBody,
        SpotLight,

    }


    class Texture
    {
        const string textureSource = "texture.png";
        
        private static Bitmap _texture;
        public static Bitmap texture
        {
            get
            {
                if (_texture == null)
                {
                    if (File.Exists(textureSource))
                    {
                        _texture = new Bitmap(textureSource);
                    }
                    else
                    {
                        Assembly a = Assembly.GetExecutingAssembly();
                        Stream s = a.GetManifestResourceStream("Lazer.Resource." + textureSource);
                        _texture = new Bitmap(s);
                    }
                }

                return _texture;
            }
        }



        public static Bitmap extract(TextureType tex, int rotation = 0, int newWidth = 0, int newHeight = 0)
        {
            return extract((int)tex, rotation, newWidth, newHeight);
        }


        public static Bitmap extract(int id, int rotation = 0, int newWidth = 0, int newHeight = 0)
        {
            int size = Lazer.Board.squareSize;
            return extract(new Rectangle((id % 10) * size, (id / 10) * size, size, size), rotation, newWidth, newHeight);
        }


        // extract a bitmap portion and rotate if necessary (clockwise, step 90°)
        public static Bitmap extract(Rectangle rect, int rotation = 0, int newWidth = 0, int newHeight = 0)
        {
            Bitmap b = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(texture,
                    new Rectangle(0, 0, newWidth > 0 ? newWidth : rect.Width, newHeight > 0 ? newHeight : rect.Height),
                    new Rectangle(rect.X, rect.Y, rect.Width, rect.Height),
                    GraphicsUnit.Pixel);
            }

            if( rotation != 0 )
            {
                switch (rotation)
                {
                    case -270:
                    case 450:
                    case 90: b.RotateFlip(RotateFlipType.Rotate90FlipNone); break;
                    
                    case -180:
                    case 540:
                    case 180: b.RotateFlip(RotateFlipType.Rotate180FlipNone); break;
                    
                    case -90:
                    case 630:
                    case 270: b.RotateFlip(RotateFlipType.Rotate270FlipNone); break;

                }
            }
            
            return b;
        }


        // colorize a black texture, 
        // just now only some colors are implemented
        public static Bitmap colorize(Bitmap b, Color c)
        {
            float[][] coeff = {
                            new float[] { 1, 0, 0, 0, 0 },
                            new float[] { 0, 1, 0, 0, 0 },
                            new float[] { 0, 0, 1, 0, 0 },
                            new float[] { 0, 0, 0, 1, 0 },
                            new float[] { 
                                c.R > 0 ? (float)c.R/255 : 0, 
                                c.G > 0 ? (float)c.G/255 : 0, 
                                c.B > 0 ? (float)c.B/255 : 0, 
                                0, 1 }};

            ColorMatrix cm = new ColorMatrix(coeff);

            /*
            if (c == Color.White)
            {
                cm.Matrix40 = 1;
                cm.Matrix41 = 1;
                cm.Matrix42 = 1;
            }
            else if (c == Color.Red)
            {
                cm.Matrix40 = 1;
            }
            else if (c == Color.Green)
            {
                cm.Matrix41 = 1;
            }
            else if (c == Color.Blue)
            {
                cm.Matrix42 = 1;
            }
            else if (c == Color.Cyan)
            {
                cm.Matrix41 = 1;
                cm.Matrix42 = 1;
            }
            else if (c == Color.Yellow)
            {
                cm.Matrix40 = 1;
                cm.Matrix41 = 1;
            }
            else if (c == Color.Magenta)
            {
                cm.Matrix40 = 1;
                cm.Matrix42 = 1;
            }
            */

            var ia = new ImageAttributes();
            ia.SetColorMatrix(cm);

            Bitmap dest = new Bitmap(b.Width, b.Height, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(dest))
            {
                g.DrawImage(b, new Rectangle(0, 0, b.Width, b.Height),
                    0, 0, b.Width, b.Height, GraphicsUnit.Pixel, ia);
            }

            return dest;
        }

    }
}
