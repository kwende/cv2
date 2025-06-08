using System.Drawing;

namespace Common
{
    public class Sprite : ISprite
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int[] PaletteIndices { get; private set; }

        private Sprite(int width, int height, int[] paletteIndices)
        {
            Width = width;
            Height = height;
            PaletteIndices = paletteIndices;
        }

        public static Sprite Load(int[] paletteIndices)
        {
            if (paletteIndices.Length != 8 * 8)
            {
                Console.Write("");
            }
            ArgumentOutOfRangeException.ThrowIfNotEqual(paletteIndices.Length, 8 * 8, nameof(paletteIndices));
            return new Sprite(8, 8, paletteIndices);
        }

        public void SetPaletteIndex(int x, int y, int paletteIndex)
        {

            ArgumentOutOfRangeException.ThrowIfLessThan(x, 0, nameof(x));
            ArgumentOutOfRangeException.ThrowIfLessThan(y, 0, nameof(y));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(x, 7, nameof(x));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(y, 7, nameof(y));
            ArgumentOutOfRangeException.ThrowIfLessThan(paletteIndex, 0, nameof(paletteIndex));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(paletteIndex, Constants.MaxPaletteValue, nameof(paletteIndex));

            PaletteIndices[(y * 8) + x] = paletteIndex;
        }

        public IEnumerable<string> ToHex(NesColor color1, NesColor color2, NesColor color3)
        {
            ArgumentNullException.ThrowIfNull(color1, nameof(color1));
            ArgumentNullException.ThrowIfNull(color2, nameof(color2));
            ArgumentNullException.ThrowIfNull(color3, nameof(color3));

            string[] hexvalues = [
                color1.ColorHexString,
                color2.ColorHexString,
                color3.ColorHexString,
            ];

            return PaletteIndices.Select(n => n == 0 ? "" : hexvalues[n - 1]);
        }

        public IEnumerable<byte> ToRGBA(NesColor color1, NesColor color2, NesColor color3)
        {
            ArgumentNullException.ThrowIfNull(color1, nameof(color1));
            ArgumentNullException.ThrowIfNull(color2, nameof(color2));
            ArgumentNullException.ThrowIfNull(color3, nameof(color3));

            byte[] ret = new byte[Width * Height * 4];

            string[] hexvalues = [
                color1.ColorHexString,
                color2.ColorHexString,
                color3.ColorHexString,
            ];

            uint[] paletteAsInts = null;
            try
            {
                paletteAsInts = hexvalues.Select(
                    n => Convert.ToUInt32($"{n}ff".Replace("#", "0x"), 16)).ToArray();
            }
            catch
            {
                throw new FormatException($"All hexcodes must be in format #000000");
            }

            for (int y = 0, i = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++, i++)
                {
                    var destinationIndex = i * 4;

                    var paletteIndex = PaletteIndices[i];
                    if (paletteIndex == 0)
                    {
                        // this is the background color. for RGB, we're 
                        // going to return transparent. 
                        ret[destinationIndex] = 0xFF;
                        ret[destinationIndex + 1] = 0xFF;
                        ret[destinationIndex + 2] = 0xFF;
                        ret[destinationIndex + 3] = 0xFF;
                    }
                    else
                    {
                        // subtract one to index the colors. 
                        uint sourceValue = paletteAsInts[paletteIndex - 1];

                        ret[destinationIndex] = (byte)((sourceValue & 0xff000000) >> 24);
                        ret[destinationIndex + 1] = (byte)((sourceValue & 0x00ff0000) >> 16);
                        ret[destinationIndex + 2] = (byte)((sourceValue & 0x0000ff00) >> 8);
                        ret[destinationIndex + 3] = (byte)(sourceValue & 0xff0000ff);
                    }
                }
            }

            return ret;
        }

        public Bitmap ToBitmap(NesColor color1, NesColor color2, NesColor color3)
        {
            throw new NotImplementedException();
        }
    }
}
