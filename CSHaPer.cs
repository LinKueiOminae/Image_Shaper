using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace ImageShaper
{
    public enum SHP_TS_BitFlags
    {
        None = 0,
        UnknownBit1 = 1,
        IsCompressed = 2,
        UnknownBit3 = 4,
        UnknownBit4 = 8,
        UnknownBit5 = 16,
        UnknownBit6 = 32,
        UnknownBit7 = 64,
        UnknownBit8 = 128
    }

    public enum SHP_TS_EncodingFormat
    {
        Undefined = 0,
        Uncompressed = 1,
        RLE_Zero = 2,
        Detect_best_size = 3,
        Uncompressed_Full_Frame = 4,
    }

    class CSHaPer
    {
        internal static bool IsSHP(string file)
        {
            ShpTSLoader shp = new ShpTSLoader();
            ISpriteFrame[] frames;
            bool result = false;
            using (StreamReader sr = new StreamReader(file))
            {
                result = shp.TryParseSprite(sr.BaseStream, out frames, true);
            }
            return result;
        }
        internal static CImageFile[] GetFrames(string shpfilename, int pal)
        {
            List<CImageFile> ImageFrames = new List<CImageFile>();
            ShpTSLoader shp = new ShpTSLoader();
            ISpriteFrame[] frames;

            using (StreamReader sr = new StreamReader(shpfilename))
            {
                shp.TryParseSprite(sr.BaseStream, out frames, true);
            }

            for (int i = 0; i < frames.Length; i++)
            {
                CImageFile f = new CImageFile(shpfilename, pal, SHP_TS_EncodingFormat.Undefined);
                f.BitFlags = frames[i].Format;

                if (BitHelper.IsSet(f.BitFlags, SHP_TS_BitFlags.IsCompressed))
                    f.CompressionFormat = SHP_TS_EncodingFormat.RLE_Zero;
                else f.CompressionFormat = SHP_TS_EncodingFormat.Uncompressed;
                f.IsSHP = true;
                f.SHPFrameNr = i;
                f.RadarColor = frames[i].RadarColor;
                ImageFrames.Add(f);
            }

            return ImageFrames.ToArray();
        }
        internal static Bitmap GetFrame(string file, int frameNr, CPalette pal)
        {
            Bitmap result = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
            if (string.IsNullOrEmpty(file)) return result;
            ShpTSLoader shp = new ShpTSLoader();
            ISpriteFrame SHPFframe = null;

            using (StreamReader sr = new StreamReader(file))
            {
                SHPFframe = shp.GetFrame(sr.BaseStream, frameNr);
            }
            if (SHPFframe == null) return result;

            result = new Bitmap(SHPFframe.MainCanvasSize.Width, SHPFframe.MainCanvasSize.Height, PixelFormat.Format8bppIndexed);
            if (pal != null)
                result.Palette = pal.GetAsColorPalette();
            BitmapData frame_data = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            // Copy the base_bytes from the image into a byte array
            byte[] frame_bytes = new byte[frame_data.Height * frame_data.Stride];
            System.Runtime.InteropServices.Marshal.Copy(frame_data.Scan0, frame_bytes, 0, frame_bytes.Length);

            int offsetx =0;
            int offsety=0;
            if (SHPFframe.MainCanvasSize.Width > SHPFframe.FrameSize.Width)
                offsetx = SHPFframe.MainCanvasSize.Width / 2 - SHPFframe.FrameSize.Width / 2;
            if (SHPFframe.MainCanvasSize.Height > SHPFframe.FrameSize.Height)
                offsety = SHPFframe.MainCanvasSize.Height / 2 - SHPFframe.FrameSize.Height / 2;
            offsetx = offsetx + (int)SHPFframe.Offset.X;
            offsety = offsety + (int)SHPFframe.Offset.Y;
            if (offsetx < 0) offsetx = 0;
            if (offsety < 0) offsety = 0;
            for (int x = 0; x < SHPFframe.FrameSize.Width; x++)
                for (int y = 0; y < SHPFframe.FrameSize.Height; y++)
                {
                    if (x + offsetx + (y + offsety) * frame_data.Stride < frame_bytes.Length)
                        frame_bytes[x + offsetx + (y + offsety) * frame_data.Stride] = SHPFframe.Data[x + y * SHPFframe.FrameSize.Width];
                }

            System.Runtime.InteropServices.Marshal.Copy(frame_bytes, 0, frame_data.Scan0, frame_bytes.Length);
            result.UnlockBits(frame_data);

            return result;
        }

        internal static int[] GetSHPPaletteUsage(string file)
        {
            int[] PaletteUsage = new int[256];
            for (int i = 0; i < PaletteUsage.Length; i++) PaletteUsage[i] = 0;

            List<CImageFile> ImageFrames = new List<CImageFile>();
            ShpTSLoader shp = new ShpTSLoader();
            ISpriteFrame[] frames;

            using (StreamReader sr = new StreamReader(file))
            {
                shp.TryParseSprite(sr.BaseStream, out frames, true);
            }

            for (int i = 0; i < frames.Length; i++)
            {
                ISpriteFrame SHPFrame = frames[i];
                using (StreamReader sr = new StreamReader(file))
                {
                    SHPFrame = shp.GetFrame(sr.BaseStream, i);
                }

                if (SHPFrame.Data == null) continue;

                int offsetx = 0;
                int offsety = 0;
                if (SHPFrame.MainCanvasSize.Width > SHPFrame.FrameSize.Width)
                    offsetx = SHPFrame.MainCanvasSize.Width / 2 - SHPFrame.FrameSize.Width / 2;
                if (SHPFrame.MainCanvasSize.Height > SHPFrame.FrameSize.Height)
                    offsety = SHPFrame.MainCanvasSize.Height / 2 - SHPFrame.FrameSize.Height / 2;
                offsetx = offsetx + (int)SHPFrame.Offset.X;
                offsety = offsety + (int)SHPFrame.Offset.Y;
                if (offsetx < 0) offsetx = 0;
                if (offsety < 0) offsety = 0;
                for (int x = 0; x < SHPFrame.FrameSize.Width; x++)
                    for (int y = 0; y < SHPFrame.FrameSize.Height; y++)
                        PaletteUsage[SHPFrame.Data[x + y * SHPFrame.FrameSize.Width]]++;
            }

            return PaletteUsage;
        }

        internal static bool IsColorUsed(string file, byte p)
        {
            List<CImageFile> ImageFrames = new List<CImageFile>();
            ShpTSLoader shp = new ShpTSLoader();
            ISpriteFrame[] frames;

            using (StreamReader sr = new StreamReader(file))
            {
                shp.TryParseSprite(sr.BaseStream, out frames, true);
            }

            for (int i = 0; i < frames.Length; i++)
            {
                ISpriteFrame SHPFrame = frames[i];
                using (StreamReader sr = new StreamReader(file))
                {
                    SHPFrame = shp.GetFrame(sr.BaseStream, i);
                }

                if (SHPFrame.Data == null) continue;

                int offsetx = 0;
                int offsety = 0;
                if (SHPFrame.MainCanvasSize.Width > SHPFrame.FrameSize.Width)
                    offsetx = SHPFrame.MainCanvasSize.Width / 2 - SHPFrame.FrameSize.Width / 2;
                if (SHPFrame.MainCanvasSize.Height > SHPFrame.FrameSize.Height)
                    offsety = SHPFrame.MainCanvasSize.Height / 2 - SHPFrame.FrameSize.Height / 2;
                offsetx = offsetx + (int)SHPFrame.Offset.X;
                offsety = offsety + (int)SHPFrame.Offset.Y;
                if (offsetx < 0) offsetx = 0;
                if (offsety < 0) offsety = 0;
                if (SHPFrame.Data.Contains(p)) return true;
                //for (int x = 0; x < SHPFrame.FrameSize.Width; x++)
                //    for (int y = 0; y < SHPFrame.FrameSize.Height; y++)
                //        if (SHPFrame.Data[x + y * SHPFrame.FrameSize.Width] == p) return true;
            }
            return false;
        }


        internal static void CreateSHP(string SHPFileName, CImageResult[] convertedframes, bool keepEvenFrameSizes, bool optimizeCanvas, bool keepCentered)
        {
            ShpTSWriter w = new ShpTSWriter(SHPFileName, convertedframes, keepEvenFrameSizes, optimizeCanvas, keepCentered);
        }

        private class ShpTSWriter
        {
            public ShpTSWriter(string SHPFileName, CImageResult[] frames, bool keepEvenFrameSizes, bool optimizeCanvas, bool keepCentered)
            {
                if (frames.Length > ushort.MaxValue) throw new Exception("More than " + ushort.MaxValue.ToString() + " frames are not possible");
                if (frames.Length == 0) return;
                if (frames.Any(f => (f.bmp.Size.Width > ushort.MaxValue) || (f.bmp.Size.Height > ushort.MaxValue)))
                    throw new Exception("Frames width & height must be smaller than " + ushort.MaxValue.ToString());

                var size = frames.First().bmp.Size;

                Point topleft = new Point(int.MaxValue, int.MaxValue);
                Point bottomright = new Point(0, 0);
                COptFrame[] optimizedframes = new COptFrame[frames.Length];
                for (int i = 0; i < frames.Length; i++)
                {
                    //Image Shaper doesn't care about different frame sizes
                    //it simply takes the biggest mainCanvasSize to fit them all in
                    if (frames[i].bmp.Size.Width > size.Width) size.Width = frames[i].bmp.Size.Width;
                    if (frames[i].bmp.Size.Height > size.Height) size.Height = frames[i].bmp.Size.Height;
                    optimizedframes[i] = GetOptimizedFrame(frames[i].bmp, frames[i].format, frames[i].bitflags, frames[i].RadarColor, keepEvenFrameSizes);

                    if (optimizeCanvas)
                    {
                        if ((optimizedframes[i].Size.Width > 0) && (optimizedframes[i].Size.Height > 0))
                        {
                            if (optimizedframes[i].Offset.X < topleft.X) topleft.X = optimizedframes[i].Offset.X;
                            if (optimizedframes[i].Offset.Y < topleft.Y) topleft.Y = optimizedframes[i].Offset.Y;
                            if (optimizedframes[i].Offset.X + optimizedframes[i].Size.Width > bottomright.X) bottomright.X = optimizedframes[i].Offset.X + optimizedframes[i].Size.Width;
                            if (optimizedframes[i].Offset.Y + optimizedframes[i].Size.Height > bottomright.Y) bottomright.Y = optimizedframes[i].Offset.Y + optimizedframes[i].Size.Height;
                        }
                    }
                }

                //minimize the main canvas to the smallest possible mainCanvasSize, adjust the optimizedframes.offset values accordingly
                //1. get the most topleft, bottomright point of all optimizedframes
                //2. set main canvas width/height to calculated distance from topleft to bottomright
                //3. adjust all optimizedframes.offset to match the new smaller canvas mainCanvasSize
                if (optimizeCanvas)
                {
                    if (keepCentered)
                    {
                        int right = size.Width - bottomright.X;
                        int bottom = size.Height - bottomright.Y;
                        if (topleft.X > right) topleft.X = right;
                        else bottomright.X = size.Width - topleft.X;
                        if (topleft.Y > bottom) topleft.Y = bottom;
                        else bottomright.Y = size.Height - topleft.Y;
                    }

                    size.Width = bottomright.X - topleft.X;
                    size.Height = bottomright.Y - topleft.Y;
                    for (int i = 0; i < optimizedframes.Length; i++)
                    {
                        if ((optimizedframes[i].Size.Width > 0) && (optimizedframes[i].Size.Height > 0))
                        {
                            optimizedframes[i].Offset.X = optimizedframes[i].Offset.X - topleft.X;
                            optimizedframes[i].Offset.Y = optimizedframes[i].Offset.Y - topleft.Y;
                            if (optimizedframes[i].Offset.X < 0) throw new Exception("Unexpected optimizeCanvas Error on frame " + i.ToString() + " Bad XOffset=" + optimizedframes[i].Offset.X.ToString());
                            if (optimizedframes[i].Offset.Y < 0) throw new Exception("Unexpected optimizeCanvas Error on frame " + i.ToString() + " Bad YOffset=" + optimizedframes[i].Offset.Y.ToString());
                        }
                    }
                }

                ushort frameCount = (ushort)optimizedframes.Length;

                using (FileStream fs = new FileStream(SHPFileName, FileMode.Create))
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write((ushort)0); //2 byte 00 00
                    bw.Write((ushort)size.Width);
                    bw.Write((ushort)size.Height);
                    bw.Write(frameCount);
                    int framesdatasize = 0;
                    //first write all the frame headers...
                    for (int i = 0; i < frameCount; i++)
                    {
                        bw.Write((ushort)optimizedframes[i].Offset.X);
                        bw.Write((ushort)optimizedframes[i].Offset.Y);
                        bw.Write((ushort)optimizedframes[i].Size.Width);
                        bw.Write((ushort)optimizedframes[i].Size.Height);
                        //format
                        bw.Write((byte)optimizedframes[i].BitFlags);
                        //switch (optimizedframes[i].format)
                        //{
                        //    case SHP_TS_EncodingFormat.Uncompressed: bw.Write((byte)1); break;
                        //    case SHP_TS_EncodingFormat.RLE_Zero: bw.Write((byte)3); break;
                        //    default: bw.Write((byte)1); break;
                        //}
                        bw.Write((byte)0);
                        bw.Write((byte)0);
                        bw.Write((byte)0);
                        bw.Write((byte)optimizedframes[i].RadarColor.R);
                        bw.Write((byte)optimizedframes[i].RadarColor.G);
                        bw.Write((byte)optimizedframes[i].RadarColor.B);
                        bw.Write((byte)0);
                        bw.Write((int)0);
                        //main header 8 byte
                        //each frame header 24 byte
                        //summed up data mainCanvasSize of all previous frames
                        int fileoffset = 8 + 24 * frameCount + framesdatasize;
                        if (optimizedframes[i].Bytes.Length == 0) fileoffset = 0;
                        framesdatasize += optimizedframes[i].Bytes.Length;
                        bw.Write(fileoffset);
                    }
                    //...then write the data of each frame
                    for (int i = 0; i < frameCount; i++)
                        bw.Write(optimizedframes[i].Bytes);
                }
            }

            private class COptFrame
            {
                public byte[] Bytes;
                public Point Offset;
                public Size Size;
                public SHP_TS_BitFlags BitFlags;
                public Color RadarColor;
            }

            /// <summary>
            /// reduce the frame to the smallest possible mainCanvasSize, removing the pure transparent part around the area of colored pixel
            /// </summary>
            /// <param name="bmp"></param>
            /// <param name="format"></param>
            /// <param name="BitFlags"></param>
            /// <param name="RadarColor"></param>
            /// <param name="keepEvenSize"></param>
            /// <returns></returns>
            private static COptFrame GetOptimizedFrame(Bitmap bmp, SHP_TS_EncodingFormat format, SHP_TS_BitFlags BitFlags, Color RadarColor, bool keepEvenSize)
            {
                Point topleft = new Point(int.MaxValue, int.MaxValue);
                Point bottomright = new Point(0, 0);

                byte[] bytes = ToBytes(bmp);

                if (format != SHP_TS_EncodingFormat.Uncompressed_Full_Frame)
                {
                    for (int x = 0; x < bmp.Width; x++)
                        for (int y = 0; y < bmp.Height; y++)
                            if (bytes[x + y * bmp.Width] != 0)
                            {
                                if (x < topleft.X) topleft.X = x;
                                if (y < topleft.Y) topleft.Y = y;
                                if (x > bottomright.X) bottomright.X = x;
                                if (y > bottomright.Y) bottomright.Y = y;
                            }
                }
                else
                {
                    topleft.X = 0;
                    topleft.Y = 0;
                    bottomright.X = bmp.Width - 1;
                    bottomright.Y = bmp.Height - 1;
                    format = SHP_TS_EncodingFormat.Uncompressed;//after this point, save as usual uncompressed format
                }

                if (keepEvenSize)
                {
                    if ((topleft.X % 2 != 0) && (topleft.X < bmp.Width)) topleft.X--;
                    if ((topleft.Y % 2 != 0) && (topleft.Y < bmp.Height)) topleft.Y--;
                }

                int optwidth = (bottomright.X + 1) - topleft.X;
                int optheight = (bottomright.Y + 1) - topleft.Y;
                if ((optwidth <= 0) || (optheight <= 0))
                {
                    optwidth = 0;
                    optheight = 0;
                    topleft = new Point(0, 0);
                }
                if (keepEvenSize)
                {
                    if ((optwidth % 2 != 0) && (optwidth < bmp.Width)) optwidth++;
                    if ((optheight % 2 != 0) && (optheight < bmp.Height)) optheight++;
                }

                byte[] optbytes = new byte[optwidth * optheight];

                //copy the small part of the unit image bytes into the optimized smaller canvas of the SHP frame
                for (int x = 0; x < optwidth; x++)
                    for (int y = 0; y < optheight; y++)
                        if (topleft.X + x + (topleft.Y + y) * bmp.Width < bytes.Length)
                            optbytes[x + y * optwidth] = bytes[topleft.X + x + (topleft.Y + y) * bmp.Width];

                COptFrame of = new COptFrame();
                of.Size = new Size(optwidth, optheight);
                of.Offset = topleft;
                of.BitFlags = BitFlags;
                switch (format)
                {
                    case SHP_TS_EncodingFormat.Uncompressed:
                        BitHelper.Unset(ref of.BitFlags, SHP_TS_BitFlags.IsCompressed);
                        of.Bytes = optbytes; 
                        break;
                    case SHP_TS_EncodingFormat.RLE_Zero:
                        BitHelper.Set(ref of.BitFlags, SHP_TS_BitFlags.IsCompressed);
                        of.Bytes = Encode3(optbytes, of.Size.Width, of.Size.Height); 
                        break;
                    case SHP_TS_EncodingFormat.Detect_best_size:
                        BitHelper.Set(ref of.BitFlags, SHP_TS_BitFlags.IsCompressed);
                        of.Bytes = Encode3(optbytes, of.Size.Width, of.Size.Height);
                        if (of.Bytes.Length > optbytes.Length)
                        {
                            BitHelper.Unset(ref of.BitFlags, SHP_TS_BitFlags.IsCompressed);
                            of.Bytes = optbytes;
                        }
                        break;
                    default:
                        BitHelper.Unset(ref of.BitFlags, SHP_TS_BitFlags.IsCompressed);
                        of.Bytes = optbytes;
                        break;
                }
                of.RadarColor = RadarColor;
                return of;
            }

            private static byte[] Encode3(byte[] data, int cx, int cy)
            {
                //worst case has 3 times as many data as original uncompressed
                //e.g. single pixel which is stored with linelength-byte, numberofrepeatingpixel-byte, colorofpixel-byte
                //e.g. a single pixel 0F is turned into 03 00 0F for a single compressed pixel in RLE compression
                byte[] Dest = new byte[2 * data.Length];//we triple the datasize, to make sure the worst case fits in
                //EDIT: unfortunately the worst case isn't even triple size. Now the array gets resized inside the loop if the new value is higher than the arrays length
                //thus initial size reduced to double size which in most cases works, and in bad cases the array is increased in the loop
                int SP = 0;
                int DP = 0;
                byte b1 = 0;
                byte b2 = 0;
                int c = 0;
                for (int y = 0; y < cy; y++)
                {
                    int SPEnd = SP + cx;
                    int DPLine = DP;
                    DP = DP + 2;
                    while (SP < SPEnd)
                    {
                        byte v = data[SP];
                        //triple size is not safe
                        //lets resize the array when necessary so it always works
                        if (DP >= Dest.Length)
                        {
                            byte[] newDest = new byte[Dest.Length + data.Length];
                            Array.Copy(Dest, newDest, Dest.Length);
                            Dest = newDest;
                        }
                        Dest[DP] = v;
                        DP++;//Inc(DP);
                        if (v != 0)
                            SP++;   //Inc(SP)
                        else
                        {
                            c = get_run_length(data, SP, SPEnd);
                            if (c > 255)
                                c = 255;
                            SP = SP + c;
                            if (DP >= Dest.Length)
                            {
                                byte[] newDest = new byte[Dest.Length + data.Length];
                                Array.Copy(Dest, newDest, Dest.Length);
                                Dest = newDest;
                            }
                            Dest[DP] = (byte)c;
                            DP++;//Inc(DP);
                        }
                    }
                    reinterpretbytesfromword((ushort)(DP - DPLine), out b1, out b2);
                    Dest[DPLine] = b1;
                    Dest[DPLine + 1] = b2;
                }

                byte[] result = new byte[DP];
                Array.Copy(Dest, result, DP);
                return result;
            }

            private static void reinterpretbytesfromword(ushort FullValue, out byte Byte1, out byte Byte2)
            {
                Byte2 = (byte)(FullValue / 256);
                Byte1 = (byte)(FullValue % 256);
            }

            private static int get_run_length(byte[] data, int SP, int SPEnd)
            {
                int Count = 1;
                byte v = data[SP];
                SP++;
                while ((SP < SPEnd) && (data[SP] == v))
                {
                    SP++;
                    Count++;
                }
                return Count;
            }

            private static byte[] ToBytes(Bitmap bitmap)
            {
                var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly,
                    PixelFormat.Format8bppIndexed);

                var bytes = new byte[bitmap.Width * bitmap.Height];
                for (var i = 0; i < bitmap.Height; i++)
                    Marshal.Copy(new IntPtr(data.Scan0.ToInt64() + i * data.Stride),
                        bytes, i * bitmap.Width, bitmap.Width);

                bitmap.UnlockBits(data);

                return bytes;
            }
        }




        private interface ISpriteFrame
        {
            /// <summary>
            /// FrameSize of the frame's `Data`.
            /// </summary>
            Size FrameSize { get; }

            /// <summary>
            /// FrameSize of the entire frame including the frame's `FrameSize`.
            /// Think of this like a picture frame.
            /// </summary>
            Size MainCanvasSize { get; }

            SHP_TS_BitFlags Format { get; }
            Color RadarColor { get; }

            float2 Offset { get; }
            byte[] Data { get; }
            bool DisableExportPadding { get; }
        }

        private class ShpTSLoader
        {
            class ShpTSFrame : ISpriteFrame
            {
                public Size FrameSize { get; private set; }
                public Size MainCanvasSize { get; private set; }
                public float2 Offset { get; private set; }
                public byte[] Data { get; set; }
                public bool DisableExportPadding { get { return false; } }

                public readonly uint FileOffset;
                public SHP_TS_BitFlags Format { get; private set; }

                public Color RadarColor { get { return Color.FromArgb(Radar_R, Radar_G, Radar_B); } }

                public byte Radar_R { get; private set; }
                public byte Radar_G { get; private set; }
                public byte Radar_B { get; private set; }

                public ShpTSFrame(Stream s, Size mainCanvasSize, bool headeronly)
                {
                    var x = StreamExts.ReadUInt16(s);
                    var y = StreamExts.ReadUInt16(s);
                    var width = StreamExts.ReadUInt16(s);
                    var height = StreamExts.ReadUInt16(s);

                    // Pad the dimensions to an even number to avoid issues with half-integer offsets
                    var dataWidth = width;
                    var dataHeight = height;
                    if (dataWidth % 2 == 1)
                        dataWidth += 1;

                    if (dataHeight % 2 == 1)
                        dataHeight += 1;

                    Offset = new int2(x + (dataWidth - mainCanvasSize.Width) / 2, y + (dataHeight - mainCanvasSize.Height) / 2);
                    FrameSize = new Size(dataWidth, dataHeight);
                    MainCanvasSize = mainCanvasSize;

                    Format = (SHP_TS_BitFlags)StreamExts.ReadUInt8(s);
                    s.Position += 3;
                    Radar_R = StreamExts.ReadUInt8(s);
                    Radar_G = StreamExts.ReadUInt8(s);
                    Radar_B = StreamExts.ReadUInt8(s);
                    s.Position += 5;
                    //s.Position += 11;
                    FileOffset = StreamExts.ReadUInt32(s);

                    if (FileOffset == 0)
                        return;

                    // Parse the frame data as we go (but remember to jump back to the header before returning!)
                    var start = s.Position;

                    if (!headeronly)
                    {
                        s.Position = FileOffset;
                        Data = new byte[dataWidth * dataHeight];

                        if (BitHelper.IsSet(Format, SHP_TS_BitFlags.IsCompressed))
                        {
                            // Format 3 provides RLE_Zero-zero compressed scanlines
                            for (var j = 0; j < height; j++)
                            {
                                var length = StreamExts.ReadUInt16(s) - 2;
                                RLEZerosCompression.DecodeInto(StreamExts.ReadBytes(s, length), Data, dataWidth * j);
                            }
                        }
                        else
                        {
                            // Format 2 provides uncompressed length-prefixed scanlines
                            // Formats 1 and 0 provide an uncompressed full-width row
                            var length = (int)Format == 2 ? StreamExts.ReadUInt16(s) - 2 : width;
                            for (var j = 0; j < height; j++)
                                StreamExts.ReadBytes(s, Data, dataWidth * j, length);
                        }
                    }
                    s.Position = start;
                }
            }
            bool IsShpTS(Stream s)
            {
                var start = s.Position;

                // First word is zero
                if (StreamExts.ReadUInt16(s) != 0)
                {
                    s.Position = start;
                    return false;
                }

                // Sanity Check the image count
                s.Position += 4;
                var imageCount = StreamExts.ReadUInt16(s);
                if (s.Position + 24 * imageCount > s.Length)
                {
                    s.Position = start;
                    return false;
                }

                s.Position += 4;
                ushort frame_w = 0, frame_h = 0;
                uint frame_offset = 0;
                //check all header if the fileoffsets are within the filesize,
                //->there is no other way than this to be sure the SHP is correct
                for (int i = 0; i < imageCount; i++)
                {
                    frame_w = StreamExts.ReadUInt16(s);
                    frame_h = StreamExts.ReadUInt16(s);
                    s.Position += 12;
                    frame_offset = StreamExts.ReadUInt32(s);
                    if (frame_offset > s.Length) return false;
                    s.Position += 4;
                }
                s.Position = start;
                return true;
            }

            //this returns false also for valid SHPs
            bool IsShpTS_tooStrictOpenRAMethod(Stream s)
            {
                var start = s.Position;

                // First word is zero
                if (StreamExts.ReadUInt16(s) != 0)
                {
                    s.Position = start;
                    return false;
                }

                // Sanity Check the image count
                s.Position += 4;
                var imageCount = StreamExts.ReadUInt16(s);
                if (s.Position + 24 * imageCount > s.Length)
                {
                    s.Position = start;
                    return false;
                }

                // Check the image mainCanvasSize and compression type format flag
                // Some files define bogus frames, so loop until we find a valid one
                s.Position += 4;
                ushort w, h, f = 0;
                uint frameoffset = 0;
                byte type;
                do
                {
                    w = StreamExts.ReadUInt16(s);
                    h = StreamExts.ReadUInt16(s);
                    type = StreamExts.ReadUInt8(s);

                    s.Position += 11;
                    frameoffset = StreamExts.ReadUInt32(s);

                    if (frameoffset > s.Length) return false;

                    //LKO:some bad tools write such bogus crap
                    //so this is no reliable way to tell if it's a SHP or not
                            // Zero sized frames always define a non-zero type
                            if ((w == 0 || h == 0) && type == 0)
                                return false;

                    s.Position += 4;
                }
                while (w == 0 && h == 0 && ++f < imageCount);

                s.Position = start;
                return f == imageCount || type < 4;
            }

            ShpTSFrame[] ParseFrames(Stream s, bool headeronly)
            {
                var start = s.Position;

                StreamExts.ReadUInt16(s);
                var width = StreamExts.ReadUInt16(s);
                var height = StreamExts.ReadUInt16(s);
                var MainCanvasSize = new Size(width, height);
                var frameCount = StreamExts.ReadUInt16(s);

                var frames = new ShpTSFrame[frameCount];
                for (var i = 0; i < frames.Length; i++)
                    frames[i] = new ShpTSFrame(s, MainCanvasSize, headeronly);

                s.Position = start;
                return frames;
            }

            public bool TryParseSprite(Stream s, out ISpriteFrame[] frames, bool headeronly)
            {
                if (!IsShpTS(s))
                {
                    frames = null;
                    return false;
                }

                frames = ParseFrames(s, headeronly);
                return true;
            }

            public ISpriteFrame GetFrame(Stream s, int frameNr)
            {
                var start = s.Position;

                StreamExts.ReadUInt16(s);
                var width = StreamExts.ReadUInt16(s);
                var height = StreamExts.ReadUInt16(s);
                var size = new Size(width, height);
                var frameCount = StreamExts.ReadUInt16(s);

                var frames = new ShpTSFrame[frameCount];

                ShpTSFrame result = null;

                for (var i = 0; i < frames.Length; i++)
                    if (frameNr == i)
                        result = new ShpTSFrame(s, size, false);
                    else
                        frames[i] = new ShpTSFrame(s, size, true);

                s.Position = start;
                return result;
            }

            #region RLE_Zero Encoding by Nyerguds (unused, since SHPs RLE compress only zeros)
            /// <summary>
            /// thanks to Nyerguds for this code
            /// </summary>
            public static Byte[] RleEncode(Byte[] buffer, Int32 minimumRepeating, Boolean swapWords)
            {
                // Technically, compressing a repetition of 2 is not useful: the compressed data
                // ends up the same mainCanvasSize, but it adds an extra byte to the data that follows it.
                // But it is allowed for the sake of completeness.
                // 1 is not allowed since it would disable all detection of non-repeating data.
                if (minimumRepeating < 2)
                    minimumRepeating = 2;
                Int32 inPtr = 0;
                Int32 outPtr = 0;
                // Ensure big enough buffer. Sanity check will be done afterwards.
                Byte[] bufferOut = new Byte[(buffer.Length * 3) / 2];

                // RLE_Zero implementation:
                // highest bit set = followed by range of repeating bytes
                // highest bit not set = followed by range of non-repeating bytes
                // In both cases, the "code" specifies the amount of bytes; either to write, or to skip.
                Int32 len = buffer.Length;
                Boolean repeatDetected = false;
                while (inPtr < len)
                {
                    if (repeatDetected || HasRepeatingAhead(buffer, len, inPtr, minimumRepeating))
                    {
                        repeatDetected = false;
                        // Found more than (minimumRepeating) bytes. Worth compressing. Apply run-length encoding.
                        Int32 start = inPtr;
                        //0x7F
                        Int32 end = Math.Min(inPtr + UInt16.MaxValue, len);
                        Byte cur = buffer[inPtr];
                        // Already checked these
                        inPtr += minimumRepeating;
                        // Increase inptr to the last repeated.
                        for (; inPtr < end && buffer[inPtr] == cur; inPtr++) { }
                        Int32 repeat = inPtr - start;
                        if (repeat < 0x80)
                        {
                            bufferOut[outPtr++] = (Byte)((0x100 - repeat) | 0x80);
                            bufferOut[outPtr++] = cur;
                        }
                        else
                        {
                            Byte lenHi = (Byte)((repeat >> 8) & 0xFF);
                            Byte lenLo = (Byte)(repeat & 0xFF);
                            bufferOut[outPtr++] = 0;
                            bufferOut[outPtr++] = swapWords ? lenLo : lenHi;
                            bufferOut[outPtr++] = swapWords ? lenHi : lenLo;
                            bufferOut[outPtr++] = cur;
                        }
                    }
                    else
                    {
                        while (!repeatDetected && inPtr < len)
                        {
                            Int32 start = inPtr;
                            Int32 end = Math.Min(inPtr + 0x7F, len);
                            for (; inPtr < end; inPtr++)
                            {
                                // detected bytes to compress after this one: abort.
                                if (!HasRepeatingAhead(buffer, len, inPtr, minimumRepeating))
                                    continue;
                                repeatDetected = true;
                                break;
                            }
                            bufferOut[outPtr++] = (Byte)(inPtr - start);
                            for (Int32 i = start; i < inPtr; i++)
                                bufferOut[outPtr++] = buffer[i];
                        }
                    }
                }
                Byte[] finalOut = new Byte[outPtr];
                Array.Copy(bufferOut, 0, finalOut, 0, outPtr);
                return finalOut;
            }

            public static Boolean HasRepeatingAhead(Byte[] buffer, Int32 max, Int32 ptr, Int32 minAmount)
            {
                if (ptr + minAmount - 1 >= max)
                    return false;
                Byte cur = buffer[ptr];
                for (Int32 i = 1; i < minAmount; i++)
                    if (buffer[ptr + i] != cur)
                        return false;
                return true;
            }
            #endregion
        }

        /*
        public class ShpTDSprite
        {
            enum Format { XORPrev = 0x20, XORLCW = 0x40, LCW = 0x80 }

            class ImageHeader : ISpriteFrame
            {
                public FrameSize FrameSize { get { return reader.FrameSize; } }
                public FrameSize MainCanvasSize { get { return reader.FrameSize; } }
                public float2 Offset { get { return float2.Zero; } }
                public byte[] Data { get; set; }
                public bool DisableExportPadding { get { return false; } }

                public uint FileOffset;
                public Format Format;

                public uint RefOffset;
                public Format RefFormat;
                public ImageHeader RefImage;

                ShpTDSprite reader;

                // Used by ShpWriter
                public ImageHeader() { }

                public ImageHeader(Stream stream, ShpTDSprite reader)
                {
                    this.reader = reader;
                    var data = StreamExts.ReadUInt32(stream);
                    FileOffset = data & 0xffffff;
                    Format = (Format)(data >> 24);

                    RefOffset = StreamExts.ReadUInt16(stream);
                    RefFormat = (Format)StreamExts.ReadUInt16(stream);
                }

                public void WriteTo(BinaryWriter writer)
                {
                    writer.Write(FileOffset | ((uint)Format << 24));
                    writer.Write((ushort)RefOffset);
                    writer.Write((ushort)RefFormat);
                }
            }

    //        public IReadOnlyList<ISpriteFrame> Frames { get; private set; }
            public readonly FrameSize FrameSize;

            int recurseDepth = 0;
            readonly int imageCount;

            readonly long shpBytesFileOffset;
            readonly byte[] shpBytes;

            public ShpTDSprite(Stream stream)
            {
                imageCount = StreamExts.ReadUInt16(stream);
                stream.Position += 4;
                var width = StreamExts.ReadUInt16(stream);
                var height = StreamExts.ReadUInt16(stream);
                FrameSize = new FrameSize(width, height);

                stream.Position += 4;
                var headers = new ImageHeader[imageCount];
                Frames = headers.AsReadOnly();
                for (var i = 0; i < headers.Length; i++)
                    headers[i] = new ImageHeader(stream, this);

                // Skip eof and zero headers
                stream.Position += 16;

                var offsets = headers.ToDictionary(h => h.FileOffset, h => h);
                for (var i = 0; i < imageCount; i++)
                {
                    var h = headers[i];
                    if (h.Format == Format.XORPrev)
                        h.RefImage = headers[i - 1];
                    else if (h.Format == Format.XORLCW && !offsets.TryGetValue(h.RefOffset, out h.RefImage))
                        throw new InvalidDataException("Reference doesn't point to image data {0}->{1}".F(h.FileOffset, h.RefOffset));
                }

                shpBytesFileOffset = stream.Position;
                shpBytes = stream.ReadBytes((int)(stream.Length - stream.Position));

                foreach (var h in headers)
                    Decompress(h);
            }

            void Decompress(ImageHeader h)
            {
                // No extra work is required for empty frames
                if (h.FrameSize.Width == 0 || h.FrameSize.Height == 0)
                    return;

                if (recurseDepth > imageCount)
                    throw new InvalidDataException("Format20/40 headers contain infinite loop");

                switch (h.Format)
                {
                    case Format.XORPrev:
                    case Format.XORLCW:
                        {
                            if (h.RefImage.Data == null)
                            {
                                ++recurseDepth;
                                Decompress(h.RefImage);
                                --recurseDepth;
                            }

                            h.Data = CopyImageData(h.RefImage.Data);
                            XORDeltaCompression.DecodeInto(shpBytes, h.Data, (int)(h.FileOffset - shpBytesFileOffset));
                            break;
                        }

                    case Format.LCW:
                        {
                            var imageBytes = new byte[FrameSize.Width * FrameSize.Height];
                            LCWCompression.DecodeInto(shpBytes, imageBytes, (int)(h.FileOffset - shpBytesFileOffset));
                            h.Data = imageBytes;
                            break;
                        }

                    default:
                        throw new InvalidDataException();
                }
            }

            byte[] CopyImageData(byte[] baseImage)
            {
                var imageData = new byte[FrameSize.Width * FrameSize.Height];
                Array.Copy(baseImage, imageData, imageData.Length);
                return imageData;
            }

            public static void Write(Stream s, FrameSize mainCanvasSize, IEnumerable<byte[]> frames)
            {
                var compressedFrames = frames.Select(f => LCWCompression.Encode(f)).ToList();

                // note: end-of-file and all-zeroes headers
                var dataOffset = 14 + (compressedFrames.Count + 2) * 8;

                using (var bw = new BinaryWriter(s))
                {
                    bw.Write((ushort)compressedFrames.Count);
                    bw.Write((ushort)0);
                    bw.Write((ushort)0);
                    bw.Write((ushort)mainCanvasSize.Width);
                    bw.Write((ushort)mainCanvasSize.Height);
                    bw.Write((uint)0);

                    foreach (var f in compressedFrames)
                    {
                        var ih = new ImageHeader { Format = Format.LCW, FileOffset = (uint)dataOffset };
                        dataOffset += f.Length;

                        ih.WriteTo(bw);
                    }

                    var eof = new ImageHeader { FileOffset = (uint)dataOffset };
                    eof.WriteTo(bw);

                    var allZeroes = new ImageHeader { };
                    allZeroes.WriteTo(bw);

                    foreach (var f in compressedFrames)
                        bw.Write(f);
                }
            }
        }
        */


    }




    class FastByteReader
    {
        readonly byte[] src;
        int offset;

        public FastByteReader(byte[] src)
        {
            this.src = src;
            this.offset = 0;
        }
        public FastByteReader(byte[] src, int offset)
        {
            this.src = src;
            this.offset = offset;
        }

        public bool Done() { return offset >= src.Length; }
        public byte ReadByte() { return src[offset++]; }
        public int ReadWord()
        {
            var x = ReadByte();
            return x | (ReadByte() << 8);
        }

        public void CopyTo(byte[] dest, int offset, int count)
        {
            Array.Copy(src, this.offset, dest, offset, count);
            this.offset += count;
        }

        public int Remaining() { return src.Length - offset; }
    }

    public static class RLEZerosCompression
    {
        public static void DecodeInto(byte[] src, byte[] dest, int destIndex)
        {
            var r = new FastByteReader(src);

            while (!r.Done())
            {
                var cmd = r.ReadByte();
                if (cmd == 0)
                {
                    var count = r.ReadByte();
                    while (count-- > 0)
                        dest[destIndex++] = 0;
                }
                else
                    dest[destIndex++] = cmd;
            }
        }
    }


    public static class StreamExts
    {

        public static byte[] ReadBytes(Stream s, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
            var buffer = new byte[count];
            ReadBytes(s, buffer, 0, count);
            return buffer;
        }

        public static void ReadBytes(Stream s, byte[] buffer, int offset, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
            while (count > 0)
            {
                int bytesRead;
                if ((bytesRead = s.Read(buffer, offset, count)) == 0)
                    throw new EndOfStreamException();
                offset += bytesRead;
                count -= bytesRead;
            }
        }

        public static byte ReadUInt8(Stream s)
        {
            var b = s.ReadByte();
            if (b == -1)
                throw new EndOfStreamException();
            return (byte)b;
        }

        public static ushort ReadUInt16(Stream s)
        {
            return BitConverter.ToUInt16(ReadBytes(s, 2), 0);
        }

        public static short ReadInt16(Stream s)
        {
            return BitConverter.ToInt16(ReadBytes(s, 2), 0);
        }

        public static uint ReadUInt32(Stream s)
        {
            return BitConverter.ToUInt32(ReadBytes(s, 4), 0);
        }

        public static int ReadInt32(Stream s)
        {
            return BitConverter.ToInt32(ReadBytes(s, 4), 0);
        }

    }


    public struct int2 : IEquatable<int2>
    {
        public readonly int X, Y;
        public int2(int x, int y) { X = x; Y = y; }
        public int2(Point p) { X = p.X; Y = p.Y; }
        public int2(Size p) { X = p.Width; Y = p.Height; }

        public static int2 operator +(int2 a, int2 b) { return new int2(a.X + b.X, a.Y + b.Y); }
        public static int2 operator -(int2 a, int2 b) { return new int2(a.X - b.X, a.Y - b.Y); }
        public static int2 operator *(int a, int2 b) { return new int2(a * b.X, a * b.Y); }
        public static int2 operator *(int2 b, int a) { return new int2(a * b.X, a * b.Y); }
        public static int2 operator /(int2 a, int b) { return new int2(a.X / b, a.Y / b); }

        public static int2 operator -(int2 a) { return new int2(-a.X, -a.Y); }

        public static bool operator ==(int2 me, int2 other) { return me.X == other.X && me.Y == other.Y; }
        public static bool operator !=(int2 me, int2 other) { return !(me == other); }

        public override int GetHashCode() { return X.GetHashCode() ^ Y.GetHashCode(); }

        public bool Equals(int2 other) { return this == other; }
        public override bool Equals(object obj) { return obj is int2 && Equals((int2)obj); }

        public override string ToString() { return X + "," + Y; }

        public int2 Sign() { return new int2(Math.Sign(X), Math.Sign(Y)); }
        public int2 Abs() { return new int2(Math.Abs(X), Math.Abs(Y)); }
        public int LengthSquared { get { return X * X + Y * Y; } }
        public int Length { get { return Exts.ISqrt(LengthSquared); } }

        public int2 WithX(int newX)
        {
            return new int2(newX, Y);
        }

        public int2 WithY(int newY)
        {
            return new int2(X, newY);
        }

        public static int2 Max(int2 a, int2 b) { return new int2(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y)); }
        public static int2 Min(int2 a, int2 b) { return new int2(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y)); }

        public static readonly int2 Zero = new int2(0, 0);
        public Point ToPoint() { return new Point(X, Y); }
        public PointF ToPointF() { return new PointF(X, Y); }
        public float2 ToFloat2() { return new float2(X, Y); }

        // Change endianness of a uint32
        public static uint Swap(uint orig)
        {
            return ((orig & 0xff000000) >> 24) | ((orig & 0x00ff0000) >> 8) | ((orig & 0x0000ff00) << 8) | ((orig & 0x000000ff) << 24);
        }

        public static int Lerp(int a, int b, int mul, int div)
        {
            return a + (b - a) * mul / div;
        }

        public static int2 Lerp(int2 a, int2 b, int mul, int div)
        {
            return a + (b - a) * mul / div;
        }

        public int2 Clamp(Rectangle r)
        {
            return new int2(Math.Min(r.Right, Math.Max(X, r.Left)),
                            Math.Min(r.Bottom, Math.Max(Y, r.Top)));
        }

        public static int Dot(int2 a, int2 b) { return a.X * b.X + a.Y * b.Y; }
    }
    public struct float2 : IEquatable<float2>
    {
        public readonly float X, Y;

        public float2(float x, float y) { X = x; Y = y; }
        public float2(PointF p) { X = p.X; Y = p.Y; }
        public float2(Point p) { X = p.X; Y = p.Y; }
        public float2(Size p) { X = p.Width; Y = p.Height; }
        public float2(SizeF p) { X = p.Width; Y = p.Height; }

        public PointF ToPointF() { return new PointF(X, Y); }
        public SizeF ToSizeF() { return new SizeF(X, Y); }

        public static implicit operator float2(int2 src) { return new float2(src.X, src.Y); }

        public static float2 operator +(float2 a, float2 b) { return new float2(a.X + b.X, a.Y + b.Y); }
        public static float2 operator -(float2 a, float2 b) { return new float2(a.X - b.X, a.Y - b.Y); }

        public static float2 operator -(float2 a) { return new float2(-a.X, -a.Y); }

        public static float Lerp(float a, float b, float t) { return a + t * (b - a); }

        public static float2 Lerp(float2 a, float2 b, float t)
        {
            return new float2(
                Lerp(a.X, b.X, t),
                Lerp(a.Y, b.Y, t));
        }

        public static float2 Lerp(float2 a, float2 b, float2 t)
        {
            return new float2(
                Lerp(a.X, b.X, t.X),
                Lerp(a.Y, b.Y, t.Y));
        }

        public static float2 FromAngle(float a) { return new float2((float)Math.Sin(a), (float)Math.Cos(a)); }

        static float Constrain(float x, float a, float b) { return x < a ? a : x > b ? b : x; }

        public float2 Constrain(float2 min, float2 max)
        {
            return new float2(
                Constrain(X, min.X, max.X),
                Constrain(Y, min.Y, max.Y));
        }

        public static float2 operator *(float a, float2 b) { return new float2(a * b.X, a * b.Y); }
        public static float2 operator *(float2 b, float a) { return new float2(a * b.X, a * b.Y); }
        public static float2 operator *(float2 a, float2 b) { return new float2(a.X * b.X, a.Y * b.Y); }
        public static float2 operator /(float2 a, float2 b) { return new float2(a.X / b.X, a.Y / b.Y); }
        public static float2 operator /(float2 a, float b) { return new float2(a.X / b, a.Y / b); }

        public static bool operator ==(float2 me, float2 other) { return me.X == other.X && me.Y == other.Y; }
        public static bool operator !=(float2 me, float2 other) { return !(me == other); }

        public override int GetHashCode() { return X.GetHashCode() ^ Y.GetHashCode(); }

        public bool Equals(float2 other) { return this == other; }
        public override bool Equals(object obj) { return obj is float2 && Equals((float2)obj); }

        public override string ToString() { return X + "," + Y; }

        public static readonly float2 Zero = new float2(0, 0);

        public static bool WithinEpsilon(float2 a, float2 b, float e)
        {
            var d = a - b;
            return Math.Abs(d.X) < e && Math.Abs(d.Y) < e;
        }

        public float2 Sign() { return new float2(Math.Sign(X), Math.Sign(Y)); }
        public static float Dot(float2 a, float2 b) { return a.X * b.X + a.Y * b.Y; }
        public float2 Round() { return new float2((float)Math.Round(X), (float)Math.Round(Y)); }

        public int2 ToInt2() { return new int2((int)X, (int)Y); }

        public static float2 Max(float2 a, float2 b) { return new float2(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y)); }
        public static float2 Min(float2 a, float2 b) { return new float2(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y)); }

        public float LengthSquared { get { return X * X + Y * Y; } }
        public float Length { get { return (float)Math.Sqrt(LengthSquared); } }
    }

    public static class Exts
    {
        public enum ISqrtRoundMode { Floor, Nearest, Ceiling }
        public static int ISqrt(int number)
        {
            return ISqrt(number, ISqrtRoundMode.Floor);
        }
        public static int ISqrt(int number, ISqrtRoundMode round)
        {
            if (number < 0)
                throw new InvalidOperationException("Attempted to calculate the square root of a negative integer: {0}");

            return (int)ISqrt((uint)number, round);
        }

        public static uint ISqrt(uint number)
        {
            return ISqrt(number, ISqrtRoundMode.Floor);
        }
        public static uint ISqrt(uint number, ISqrtRoundMode round)
        {
            var divisor = 1U << 30;

            var root = 0U;
            var remainder = number;

            // Find the highest term in the divisor
            while (divisor > number)
                divisor >>= 2;

            // Evaluate the root, two bits at a time
            while (divisor != 0)
            {
                if (root + divisor <= remainder)
                {
                    remainder -= root + divisor;
                    root += 2 * divisor;
                }

                root >>= 1;
                divisor >>= 2;
            }

            // Adjust for other rounding modes
            if (round == ISqrtRoundMode.Nearest && remainder > root)
                root += 1;
            else if (round == ISqrtRoundMode.Ceiling && root * root < number)
                root += 1;

            return root;
        }

        public static long ISqrt(long number)
        {
            return ISqrt(number, ISqrtRoundMode.Floor);
        }
        public static long ISqrt(long number, ISqrtRoundMode round)
        {
            if (number < 0)
                throw new InvalidOperationException("Attempted to calculate the square root of a negative integer: {0}");

            return (long)ISqrt((ulong)number, round);
        }

        public static ulong ISqrt(ulong number)
        {
            return ISqrt(number, ISqrtRoundMode.Floor);
        }
        public static ulong ISqrt(ulong number, ISqrtRoundMode round)
        {
            var divisor = 1UL << 62;

            var root = 0UL;
            var remainder = number;

            // Find the highest term in the divisor
            while (divisor > number)
                divisor >>= 2;

            // Evaluate the root, two bits at a time
            while (divisor != 0)
            {
                if (root + divisor <= remainder)
                {
                    remainder -= root + divisor;
                    root += 2 * divisor;
                }

                root >>= 1;
                divisor >>= 2;
            }

            // Adjust for other rounding modes
            if (round == ISqrtRoundMode.Nearest && remainder > root)
                root += 1;
            else if (round == ISqrtRoundMode.Ceiling && root * root < number)
                root += 1;

            return root;
        }

    }
}
