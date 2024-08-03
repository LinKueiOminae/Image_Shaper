using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageShaper
{
    public enum ColorConversionMethod
    {
        Euclidean = 0,
        DeltaCIE2000 = 1
    }

    class CConverter
    {

        private class CPaletteLookUp
        {
            struct Tuple<T, U> : IEquatable<Tuple<T, U>>
            {
                readonly T Palette;
                readonly U Color;
                public Tuple(T Palette, U Color)
                {
                    this.Palette = Palette;
                    this.Color = Color;
                }

                public T First { get { return Palette; } }
                public U Second { get { return Color; } }

                public override int GetHashCode()
                {
                    return Palette.GetHashCode() ^ Color.GetHashCode();
                }

                public override bool Equals(object obj)
                {
                    if (obj == null || GetType() != obj.GetType())
                        return false;

                    return Equals((Tuple<T, U>)obj);
                }

                public bool Equals(Tuple<T, U> other)
                {
                    return other.Palette.Equals(Palette) && other.Color.Equals(Color);
                }
            }

            private struct LookUpColor
            {
                public LookUpColor(CPalette pal, Color col)
                {
                    this.palette = pal;
                    this.color = col;
                }
                public CPalette palette;
                public Color color;
            }

            Dictionary<Tuple<CPalette, Color>, int> LookUp;
            internal CPaletteLookUp()
            {
                LookUp = new Dictionary<Tuple<CPalette, Color>, int>();
            }

            internal int GetPaletteColor(Color c, CPalette palette)
            {
                int result = 0;

                if (!LookUp.TryGetValue(new Tuple<CPalette, Color>(palette, c), out result))
                {
                    switch (palette.ConversionMethod)
                    {
                        case ColorConversionMethod.Euclidean: result = CPalette.NearestColorEuclidean(palette, c); break;
                        case ColorConversionMethod.DeltaCIE2000: result = CPalette.NearestColorDeltaE2000(palette, c); break;
                        default: result = CPalette.NearestColorEuclidean(palette, c); break;
                    }

                    LookUp.Add(new Tuple<CPalette, Color>(palette, c), result);
                }
                return result;
            }

            internal void SetPaletteColor(Color c, CPalette Palette, int p)
            {
                if (!LookUp.ContainsKey(new Tuple<CPalette, Color>(Palette, c)))
                    LookUp.Add(new Tuple<CPalette, Color>(Palette, c), 0);
            }
        }

        CPaletteLookUp LookUpTable;
        public CConverter()
        {
            LookUpTable = new CPaletteLookUp();
        }


        public CImageResult CombineAndConvert(CImageFile[] files2combine)
        {
            CImageResult result = new CImageResult(new Bitmap(1, 1), -1, SHP_TS_EncodingFormat.Undefined, SHP_TS_BitFlags.None, Color.FromArgb(0, 0, 0), "");
            if (files2combine.Length > 0)
            {
                CImageFile basefile = files2combine[0];
                Bitmap convertedbaseimage = null;
                if (!basefile.IsSHP)
                    convertedbaseimage = LoadImageWithoutFuckingDPI(basefile.FileName, PaletteManager.GetPalette(basefile.PaletteIndex), basefile.CustomBackgroundColor, basefile.UseCustomBackgroundColor, basefile.CombineTransparentPixel);
                else
                    convertedbaseimage = CSHaPer.GetFrame(basefile.FileName, basefile.SHPFrameNr, PaletteManager.GetPalette(basefile.PaletteIndex));

                //bool stores the Inverted transparency flag
                List<KeyValuePair<Bitmap, bool>> convertedsubimages = new List<KeyValuePair<Bitmap, bool>>();
                if (files2combine.Length > 1)
                    for (int i = 1; i < files2combine.Length; i++)
                        if (!files2combine[i].IsSHP)
                        {
                            convertedsubimages.Add(new KeyValuePair<Bitmap, bool>(
                                LoadImageWithoutFuckingDPI(files2combine[i].FileName, PaletteManager.GetPalette(files2combine[i].PaletteIndex), files2combine[i].CustomBackgroundColor, files2combine[i].UseCustomBackgroundColor, files2combine[i].CombineTransparentPixel),
                                files2combine[i].CombineTransparentPixel));
                        }
                        else
                            convertedsubimages.Add(new KeyValuePair<Bitmap, bool>(
                                CSHaPer.GetFrame(files2combine[i].FileName, files2combine[i].SHPFrameNr, PaletteManager.GetPalette(files2combine[i].PaletteIndex)), 
                                false));

                Color avgRadarColor;
                //combine the palette indexed images
                convertedbaseimage = CombineImages(convertedbaseimage, convertedsubimages.ToArray(), basefile.RadarColorAverage, out avgRadarColor);

                result.bmp = convertedbaseimage;
                result.RadarColor = avgRadarColor;
                result.bitflags = basefile.BitFlags;
                return result;
            }
            return result;// new Bitmap(1, 1);
        }

        private Bitmap CombineImages(Bitmap baseimage, KeyValuePair<Bitmap, bool>[] subimages, bool RadarColorAverage, out Color AvgRadarColor)
        {
            AvgRadarColor = Color.FromArgb(0, 0, 0);
            bool IncorrectFormat = false;
            IncorrectFormat = baseimage.PixelFormat != PixelFormat.Format8bppIndexed;
            for (int i = 0; i < subimages.Length; i++)
                if (subimages[i].Key.PixelFormat != PixelFormat.Format8bppIndexed) IncorrectFormat = true;
            if (IncorrectFormat) return baseimage;

            BitmapData base_data = baseimage.LockBits(new Rectangle(0, 0, baseimage.Width, baseimage.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            // Copy the base_bytes from the image into a byte array
            byte[] base_bytes = new byte[base_data.Height * base_data.Stride];
            System.Runtime.InteropServices.Marshal.Copy(base_data.Scan0, base_bytes, 0, base_bytes.Length);

            BitmapData[] sub_data = new BitmapData[subimages.Length];
            byte[][] sub_bytes = new byte[subimages.Length][];
            for (int i = 0; i < sub_data.Length; i++)
            {
                sub_data[i] = subimages[i].Key.LockBits(new Rectangle(0, 0, subimages[i].Key.Width, subimages[i].Key.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                sub_bytes[i] = new byte[sub_data[i].Height * sub_data[i].Stride];
                System.Runtime.InteropServices.Marshal.Copy(sub_data[i].Scan0, sub_bytes[i], 0, sub_bytes[i].Length);
            }

            long R = 0, G = 0, B = 0, CP = 0;

            //accessing image.palette.Entries is super slow, so copy the palette and do the work on that array
            Color[] pal = baseimage.Palette.Entries;

            for (int x = 0; x < baseimage.Width; x++)
                for (int y = 0; y < baseimage.Height; y++)
                {
                    int col = -1;
                    //check if the current pixel is inside the subimage and if it is not transparent color #0
                    for (int i = subimages.Length-1; i >= 0; i--)
                        if ((x < subimages[i].Key.Width) && (y < subimages[i].Key.Height))
                        {
                            if (!subimages[i].Value) //not combine transparent pixel (normal mode)
                            {
                                if ((col == -1) && (sub_bytes[i][x + y * sub_data[i].Stride] != 0))
                                    col = sub_bytes[i][x + y * sub_data[i].Stride];
                                
                                if (col == 0)
                                    col = -1;
                            }
                            else//combine transparent pixel
                            {
                                if ((col == -1) && (sub_bytes[i][x + y * sub_data[i].Stride] == 0))
                                    col = 0;
                            }
                        }

                    if (col != -1)
                        base_bytes[x + y * base_data.Stride] = (byte)col;

                    if (RadarColorAverage)
                    {
                        byte tmp = base_bytes[x + y * base_data.Stride];
                        if (tmp != 0)
                        {
                            R += pal[tmp].R;
                            G += pal[tmp].G;
                            B += pal[tmp].B;
                            CP++;
                        }
                    }
                }
            if (CP != 0)
                AvgRadarColor = Color.FromArgb((int)(R / CP), (int)(G / CP), (int)(B / CP));

            for (int i = 0; i < subimages.Length; i++)
                subimages[i].Key.UnlockBits(sub_data[i]);

            // Copy the input_bytes from the byte array into the image
            System.Runtime.InteropServices.Marshal.Copy(base_bytes, 0, base_data.Scan0, base_bytes.Length);
            baseimage.UnlockBits(base_data);

            return baseimage;
        }

        /// <summary>
        /// stupid .NET image routines consider the DPI of an image, unlike every fucking editor and image viewer on this planet
        /// Due to this stupidity, DrawImage will consider the DPI and creates garbage by resizing an image according to its DPI
        /// e.g.
        /// Image A= 100x100 pixel with DPI=96 
        /// Image B= 100x100 pixel with DPI=48
        /// A.DrawImage(B) results in B being drawn in half the mainCanvasSize on the top left corner of A (AAARGH, FUCKING STUPID MICROSOFT SHIT)
        /// </summary>
        public Bitmap LoadImageWithoutFuckingDPI(string file, CPalette Palette, Color CustomBackgroundColor, bool UseCustomBackgroundColor, bool InvertTransparency)
        {
            Bitmap input;   //(Bitmap)Image.FromFile(file);
            //changed to relase the file lock after the image is loaded
            //this way the image in the preview isn't locking the file and the files can be replaced/changed without closing ImageShaper

            //(Bitmap)Image.FromFile(file) is used instead of Bitmap(file), because Image.FromFile throws a FileNotFoundException when the file wasn't found
            //Bitmap(file) only throws a meaningless "invalid parameter" crap
            using (Bitmap tmp = (Bitmap)Image.FromFile(file))
            {
                input = new Bitmap(tmp);
            }
            input.SetResolution(96, 96);

            int s = Image.GetPixelFormatSize(input.PixelFormat) / 8;

            //convert all images into 32bppArgb format, that are not palette indexed and not 3 bytes (RGB) or 4 bytes (ARGB) for each pixel
            //this is pretty time consuming when done over hundreds of files, thus this tool can handle 3 byte and 4 byte per pixel color coded files and only does this conversion for other formats
            if ((input.PixelFormat != PixelFormat.Format8bppIndexed) && ((s != 3) && (s != 4)))
            {
                Bitmap tmp = new Bitmap(input.Width, input.Height, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(tmp))
                {
                    g.DrawImage(input, new Rectangle(0, 0, tmp.Width, tmp.Height));
                }
                input = tmp;
                s = Image.GetPixelFormatSize(input.PixelFormat) / 8;
            }


            BitmapData input_data = input.LockBits(new Rectangle(0, 0, input.Width, input.Height),
                   System.Drawing.Imaging.ImageLockMode.ReadWrite,
                   input.PixelFormat);

            // Copy the input_bytes from the image into a byte array
            byte[] input_bytes = new byte[input_data.Height * input_data.Stride];
            System.Runtime.InteropServices.Marshal.Copy(input_data.Scan0, input_bytes, 0, input_bytes.Length);

            input.UnlockBits(input_data);


            //after the initial edits to the input file, create/edit the palette indexed bmp file

            Bitmap output = new Bitmap(input.Width, input.Height, PixelFormat.Format8bppIndexed);

            if (Palette != null)
            {

                output.Palette = Palette.GetAsColorPalette();

                BitmapData output_data = output.LockBits(new Rectangle(0, 0, output.Width, output.Height),
                                           System.Drawing.Imaging.ImageLockMode.ReadWrite,
                                           System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                // Copy the bmp_bytes from the image into a byte array
                byte[] output_bytes = new byte[output_data.Height * output_data.Stride];
                System.Runtime.InteropServices.Marshal.Copy(output_data.Scan0, output_bytes, 0, output_bytes.Length);

                Color FrameTransparentColor = CustomBackgroundColor;

                Color c = Color.FromArgb(0, 0, 0, 0);

                //if no special background color is used (which sets these pixel to Alpha=0), 
                //then use the top left corner pixel as base for the background color, regardless of this pixels color value
                if (!UseCustomBackgroundColor)
                {
                    if (s == 4)
                        c = Color.FromArgb(
                             input_bytes[3],
                             input_bytes[2],
                             input_bytes[1],
                             input_bytes[0]);
                    if (s == 3)
                        c = Color.FromArgb(
                             input_bytes[2],
                             input_bytes[1],
                             input_bytes[0]);
                    FrameTransparentColor = c;
                }

                for (int x = 0; x < input.Width; x++)
                    for (int y = 0; y < input.Height; y++)
                    {
                        int palette_col = 0;
                        if (input.PixelFormat != PixelFormat.Format8bppIndexed)
                        {
                            if (s == 4)
                                c = Color.FromArgb(
                                     input_bytes[(x * s) + y * input_data.Stride + 3],
                                     input_bytes[(x * s) + y * input_data.Stride + 2],
                                     input_bytes[(x * s) + y * input_data.Stride + 1],
                                     input_bytes[(x * s) + y * input_data.Stride]);
                            if (s == 3)
                                c = Color.FromArgb(
                                     input_bytes[(x * s) + y * input_data.Stride + 2],
                                     input_bytes[(x * s) + y * input_data.Stride + 1],
                                     input_bytes[(x * s) + y * input_data.Stride]);


                            if ((c.R == FrameTransparentColor.R) && (c.G == FrameTransparentColor.G) && (c.B == FrameTransparentColor.B))
                                palette_col = 0;
                            else
                                palette_col = LookUpTable.GetPaletteColor(c, Palette);
                        }
                        else //palette indexed images are loaded directly. no color conversion is done
                            palette_col = input_bytes[x + y * input_data.Stride];

                        output_bytes[x + y * output_data.Stride] = (byte)palette_col;
                    }


                // Copy the input_bytes from the byte array into the image
                System.Runtime.InteropServices.Marshal.Copy(output_bytes, 0, output_data.Scan0, output_bytes.Length);
                output.UnlockBits(output_data);

                
                return output;
            }
            return input;
        }

        public static int GetPixel(int x, int y, Image image)
        {
            int palettevalue = -1;
            Bitmap bmp = (Bitmap)image;

            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                           System.Drawing.Imaging.ImageLockMode.ReadOnly,
                           System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            // Copy the bmp_bytes from the image into a byte array
            byte[] bmp_bytes = new byte[bmp_data.Height * bmp_data.Stride];
            System.Runtime.InteropServices.Marshal.Copy(bmp_data.Scan0, bmp_bytes, 0, bmp_bytes.Length);
            if ((x < image.Width) && (y < image.Height))
                palettevalue = bmp_bytes[x + y * bmp_data.Stride];
            bmp.UnlockBits(bmp_data);

            return palettevalue;
        }

    }
}
