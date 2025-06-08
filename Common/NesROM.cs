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

                for (int tileNumber = 0; tileNumber < 32; tileNumber++)
                {
                    var sheet = new SpriteSheet
                    {
                        Height = 128,
                        Width = 128,
                        Sprites = new List<ISprite>(),
                        SpriteHeight = eightBySixteenMode ? 16 : 8,
                        SpriteWidth = 8,
                    };

                    sheets.Add(sheet);
                    // each tile is a 16xwidth grid of sprites
                    ISprite? prevSprite = null;
                    for (int y = 0; y < 16; y++)
                    {
                        for (int x = 0; x < 16; x++)
                        {
                            var panel1 = new ReadOnlySpan<byte>(chrBank, i, 8);
                            i += 8;
                            var panel2 = new ReadOnlySpan<byte>(chrBank, i, 8);
                            i += 8;

                            int[] paletteIndices = new int[8 * 8];
                            for (int _y = 0; _y < 8; _y++)
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
                                    paletteIndices[index] = paletteIndex;
                                }
                            }

                            var curSprite = Sprite.Load(paletteIndices);
                            if (eightBySixteenMode)
                            {
                                if (prevSprite == null)
                                {
                                    prevSprite = curSprite;
                                }
                                else
                                {
                                    sheet.Sprites.Add(CompositeSprite.Create([prevSprite, curSprite], SpriteOrientationEnum.Vertical));
                                    prevSprite = null;
                                }
                            }
                            else
                            {
                                sheet.Sprites.Add(curSprite);
                            }
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
