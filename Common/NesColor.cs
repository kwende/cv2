namespace Common
{
    public class NesColor : IEquatable<NesColor>
    {
        public string ColorHexCode { get; private set; }
        public int NesColorNumber { get; private set; }

        public NesColor(string colorHexCode)
        {
            ColorHexCode = colorHexCode;
            NesColorNumber = NesColorsUtils.HexColorCodeToNesColorCode(colorHexCode);
        }

        public bool Equals(NesColor? other)
        {
            return other?.NesColorNumber == NesColorNumber;
        }
    }
}
