namespace Common
{
    public static class Constants
    {
        public const int MaxPaletteValue = 63;
        public const uint Magic = 0x1a53454e;
        // actually starts with 0x0f, but that's background so hiding it. 
        public static readonly int[] SimonPalette = [0x0f, 0x16, 0x20];
    }
}
