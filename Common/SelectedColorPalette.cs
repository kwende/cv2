namespace Common
{
    public class SelectedColorPalette
    {
        public NesColor Slot1 { get; set; } = new(NesColorsUtils.FullNesPalette[0]);
        public NesColor Slot2 { get; set; } = new(NesColorsUtils.FullNesPalette[1]);
        public NesColor Slot3 { get; set; } = new(NesColorsUtils.FullNesPalette[2]);

        public string[] ColorCodes => [Slot1.ColorHexCode, Slot2.ColorHexCode, Slot3.ColorHexCode];
        public NesColor[] NesColorCodes => [Slot1, Slot2, Slot3];
    }
}
