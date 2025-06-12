using System.Drawing;

namespace Common
{
    public interface ISprite
    {
        int[] PaletteIndices { get; }
        int Width { get; }
        int Height { get; }
        void SetPaletteIndex(int x, int y, int paletteIndex);
        IEnumerable<byte> ToRGBA(NesColor color1, NesColor color2, NesColor color3);
        IEnumerable<string> ToHex(NesColor color1, NesColor color2, NesColor color3);
        Bitmap ToBitmap(NesColor color1, NesColor color2, NesColor color3);
    }
}
