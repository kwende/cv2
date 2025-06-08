namespace Common
{
    public class NesColor : IEquatable<NesColor>
    {
        public uint ColorRGBUInt { get; private set; }
        public string ColorHexString { get; private set; }
        public int NesColorNumber { get; private set; }
        public byte[] ColorRGBBytes
        {
            get
            {
                byte[] bytes = new byte[4];
                bytes[0] = (byte)((ColorRGBUInt >> 24) & 0x000000ff);
                bytes[1] = (byte)((ColorRGBUInt >> 16) & 0x000000ff);
                bytes[2] = (byte)((ColorRGBUInt >> 8) & 0x000000ff);
                bytes[3] = (byte)((ColorRGBUInt) & 0x000000ff);

                return bytes;
            }
        }

        public NesColor(uint colorRGBUInt)
        {
            ColorRGBUInt = colorRGBUInt;
            ColorHexString = ColorRGBUInt.ToString("x8");
            NesColorNumber = NesColorsUtils.HexColorCodeToNesColorIndex(ColorHexString);
        }

        public NesColor(string colorHexString)
        {
            ColorHexString = colorHexString;
            ColorRGBUInt = Convert.ToUInt32($"{ColorHexString}".Replace("#", "0x"), 16);
            NesColorNumber = NesColorsUtils.HexColorCodeToNesColorIndex(colorHexString);
        }

        public bool Equals(NesColor? other)
        {
            return other?.NesColorNumber == NesColorNumber;
        }

        public override string ToString()
        {
            return ColorHexString;
        }
    }
}
