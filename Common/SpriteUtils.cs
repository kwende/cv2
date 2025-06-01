namespace Common
{
    public static class SpriteUtils
    {
        public static async Task<byte[]> PaintSprite(Sprite sprite, string[] hexCodes)
        {
            ArgumentNullException.ThrowIfNull(sprite, nameof(sprite));
            ArgumentNullException.ThrowIfNull(hexCodes, nameof(hexCodes));
            ArgumentOutOfRangeException.ThrowIfNotEqual(hexCodes.Length, 3, nameof(hexCodes));

            byte[] image = new byte[sprite.Width * sprite.Height * 4];

            await Task.Run(() =>
            {
                uint[] colorInts = null;
                try
                {
                    colorInts = hexCodes.Select(
                        n => Convert.ToUInt32($"{n}ff".Replace("#", "0x"), 16)).ToArray();
                }
                catch
                {
                    throw new FormatException($"All hexcodes must be in format #000000");
                }


                for (int y = 0, i = 0; y < sprite.Height; y++)
                {
                    for (int x = 0; x < sprite.Width; x++, i++)
                    {
                        var value = sprite.SheetData[i] - 1;
                        uint sourceValue = 0x00000000;
                        if (value > -1)
                        {
                            sourceValue = colorInts[value];
                        }
                        var destinationIndex = i * 4;

                        image[destinationIndex] = (byte)((sourceValue & 0xff000000) >> 24);
                        image[destinationIndex + 1] = (byte)((sourceValue & 0x00ff0000) >> 16);
                        image[destinationIndex + 2] = (byte)((sourceValue & 0x0000ff00) >> 8);
                        image[destinationIndex + 3] = (byte)(sourceValue & 0xff0000ff);
                    }
                }
            });
            return image;

        }

        public static Sprite MergeSideBySide(Sprite sprite1, Sprite sprite2)
        {
            ArgumentNullException.ThrowIfNull(sprite1, nameof(sprite1));
            ArgumentNullException.ThrowIfNull(sprite2, nameof(sprite2));

            if (sprite1.Width != sprite2.Width && sprite1.Height != sprite2.Height)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(sprite1)} and {nameof(sprite2)} must have the same dimensions.");
            }

            Sprite merged = new Sprite
            {
                Width = sprite1.Width + sprite2.Width,
                Height = sprite1.Height,
                SheetData = new int[(sprite1.Width + sprite2.Width) * sprite1.Height],
            };

            for (int y = 0, sourceIndex = 0; y < sprite1.Height; y++)
            {
                for (int x = 0; x < sprite1.Width; x++, sourceIndex++)
                {
                    var sheet1Value = sprite1.SheetData[sourceIndex];
                    var sheet2Value = sprite2.SheetData[sourceIndex];

                    merged.SheetData[(y * merged.Width) + x] = sheet1Value;
                    merged.SheetData[(y * merged.Width) + x + sprite1.Width] = sheet2Value;
                }
            }

            return merged;
        }
    }
}
