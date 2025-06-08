namespace Common
{
    public class Sprite
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int[] PaletteIndices { get; set; } = Array.Empty<int>();
    }
}
