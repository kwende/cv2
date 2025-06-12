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
                        var value = sprite.PaletteIndices[i] - 1;
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
    }
}
