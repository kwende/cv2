using Common.Exceptions;
using System.Runtime.InteropServices;

namespace Common
{
    public class NesROM
    {
        private MemoryStream? _stream = null;
        private iNesHeader _iNesHeader;

        private long _prgSize = 0, _chrSize = 0;

        public NesROM()
        {
            _stream = new MemoryStream(275_000);
        }

        public List<SpriteSheet> GetSpriteSheets(bool eightBySixteenMode)
        {
            if (_stream == null || _prgSize == 0 || _chrSize == 0)
            {
                throw new NotInitializedException();
            }
            else
            {
                List<SpriteSheet> sheets = new List<SpriteSheet>();
                _stream.Seek(_prgSize + 16, SeekOrigin.Begin);

                byte[] chrBank = new byte[_chrSize];

                _stream.Read(chrBank, 0, chrBank.Length);

                // hack just for CV2 for now. 
                int i = 0;

                int rows = eightBySixteenMode ? 8 : 16;
                int height = eightBySixteenMode ? 16 : 8;

                for (int tileNumber = 0; tileNumber < 32; tileNumber++)
                {
                    var sheet = new SpriteSheet
                    {
                        Height = 128,
                        Width = 128,
                        Sprites = new List<Sprite>(),
                        SpriteHeight = height,
                        SpriteWidth = 8,
                    };

                    sheets.Add(sheet);
                    // each tile is a 16xwidth grid of sprites
                    for (int y = 0; y < rows; y++)
                    {
                        for (int x = 0; x < 16; x++)
                        {
                            var panel1 = new ReadOnlySpan<byte>(chrBank, i, 8);
                            i += 8;
                            var panel2 = new ReadOnlySpan<byte>(chrBank, i, 8);
                            i += 8;

                            if (eightBySixteenMode)
                            {
                                byte[] panel1Combined = new byte[16];
                                byte[] panel2Combined = new byte[16];

                                var panel3 = new ReadOnlySpan<byte>(chrBank, i, 8);
                                i += 8;
                                var panel4 = new ReadOnlySpan<byte>(chrBank, i, 8);
                                i += 8;

                                panel1.CopyTo(panel1Combined);
                                panel2.CopyTo(panel2Combined);

                                panel3.CopyTo(panel1Combined.AsSpan(panel1.Length));
                                panel4.CopyTo(panel2Combined.AsSpan(panel2.Length));

                                panel1 = new ReadOnlySpan<byte>(panel1Combined);
                                panel2 = new ReadOnlySpan<byte>(panel2Combined);
                            }

                            int[] sheetData = new int[8 * height];
                            for (int _y = 0; _y < height; _y++)
                            {
                                var panel1Byte = panel1[_y];
                                var panel2Byte = panel2[_y];

                                for (int _x = 0; _x < 8; _x++)
                                {
                                    byte mask = (byte)(0x80 >> _x);

                                    int panel1Bit = (panel1Byte & mask) != 0 ? 1 : 0;
                                    int panel2Bit = (panel2Byte & mask) != 0 ? 1 : 0;

                                    int paletteIndex = (panel2Bit << 1) | panel1Bit;

                                    var index = (_y * 8) + _x;
                                    sheetData[index] = paletteIndex;
                                }
                            }

                            sheet.Sprites.Add(new Sprite
                            {
                                Width = 8,
                                Height = height,
                                SheetData = sheetData,
                            });
                        }
                    }
                }
                return sheets;
            }
        }

        public async Task Load(Stream stream)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));

            stream.Seek(0, SeekOrigin.Begin);

            await stream.CopyToAsync(_stream);

            if (stream.Length < 16)
            {
                throw new InvalidRomSizeException();
            }

            stream.Seek(0, SeekOrigin.Begin);

            byte[] headerBytes = new byte[16];
            await stream.ReadAsync(headerBytes, 0, headerBytes.Length);

            _iNesHeader = MemoryMarshal.Read<iNesHeader>(new ReadOnlySpan<byte>(headerBytes));

            _prgSize = _iNesHeader.PrgROMSize * 0x4000L;
            _chrSize = _iNesHeader.ChrROMSize * 0x2000L;

            if (_iNesHeader.Magic != Constants.Magic ||
                stream.Length < (16 + _prgSize + _chrSize))
            {
                throw new InvalidRomFormatException();
            }
        }
    }
}
