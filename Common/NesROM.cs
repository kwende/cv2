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

        public async Task SaveToStream(Stream outStream)
        {
            await Task.Run(() =>
            {
                _stream.WriteTo(outStream);
            });
        }

        public async Task SaveToFile(string filePath)
        {
            await Task.Run(() =>
            {
                using (FileStream fs = File.Create(filePath))
                {
                    _stream.WriteTo(fs);
                }
            });
        }

        public void SaveSpriteSheets(List<SpriteSheet> sheetsToSave)
        {
            byte[] entireGame = _stream.GetBuffer();
            long fileOffset = 0;

            var chrOffset = _prgSize + 16;

            var length = _stream.Length;
            foreach (var sheet in sheetsToSave)
            {
                // each sprite is 16 bytes. There are 16x16 sprites on a sheet
                // so each sheet is 16*16*16 = 4k bytes. 
                fileOffset = chrOffset + (sheet.SheetNumber * 4_096);

                if (fileOffset != sheet.FilePosition)
                {
                    throw new CorruptedSpriteSheetException();
                }

                for (int y = 0, i = 0; y < (sheet.EightBySixteen ? 8 : 16); y++)
                {
                    for (int x = 0; x < 16; x++, i++)
                    {
                        var sprite = sheet.Sprites[i];

                        var spritePalette = sprite.PaletteIndices;
                        if (spritePalette.Length != 8 * (sheet.EightBySixteen ? 16 : 8))
                        {
                            throw new InvalidPaletteIndexArraySize();
                        }
                        var spriteOffset = i * (sheet.EightBySixteen ? 32 : 16);

                        int spritePaletteOffset = 0;
                        for (int bytePairIndex = 0; bytePairIndex < (sheet.EightBySixteen ? 2 : 1); bytePairIndex++)
                        {
                            for (int bytePairByteIndex = 0; bytePairByteIndex < 8; bytePairByteIndex++)
                            {
                                byte panel1Byte = 0x0, panel2Byte = 0x0;
                                int byteOffset = bytePairByteIndex + (bytePairIndex * 16);

                                for (int bytePairBitIndex = 0; bytePairBitIndex < 8; bytePairBitIndex++, spritePaletteOffset++)
                                {
                                    // palette index for that pixel. 
                                    int paletteIndex = spritePalette[spritePaletteOffset];
                                    int panel2Bit = paletteIndex >> 1;
                                    int panel1Bit = paletteIndex & 0x1;

                                    panel1Byte |= (byte)(panel1Bit << (7 - bytePairBitIndex));
                                    panel2Byte |= (byte)(panel2Bit << (7 - bytePairBitIndex));
                                }

                                if (entireGame[fileOffset + spriteOffset + byteOffset] != panel1Byte)
                                {
                                    Console.WriteLine();
                                }
                                if (entireGame[fileOffset + spriteOffset + byteOffset + Constants.SpriteSizeInBytes] != panel2Byte)
                                {
                                    Console.WriteLine();
                                }

                                entireGame[fileOffset + spriteOffset + byteOffset] = panel1Byte;
                                entireGame[fileOffset + spriteOffset + byteOffset + Constants.SpriteSizeInBytes] = panel2Byte;
                            }

                        }
                    }
                }
                if (_stream.Length != length)
                {
                    Console.WriteLine();
                }
            }
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
                long filePointer = _stream.Position;

                byte[] chrBank = new byte[_chrSize];

                _stream.Read(chrBank, 0, chrBank.Length);

                int chrBankOffset = 0;
                for (int tileNumber = 0; tileNumber < 32; tileNumber++)
                {
                    var sheet = new SpriteSheet
                    {
                        FilePosition = filePointer,
                        EightBySixteen = eightBySixteenMode,
                        SheetNumber = tileNumber,
                        Height = 128,
                        Width = 128,
                        Sprites = new List<ISprite>(),
                        SpriteHeight = eightBySixteenMode ? 16 : 8,
                        SpriteWidth = 8,
                    };

                    sheets.Add(sheet);
                    ISprite? prevSprite = null;
                    for (int y = 0; y < 16; y++)
                    {
                        for (int x = 0; x < 16; x++)
                        {
                            var panel1 = new ReadOnlySpan<byte>(chrBank, chrBankOffset, 8);
                            chrBankOffset += 8;
                            filePointer += 8;

                            var panel2 = new ReadOnlySpan<byte>(chrBank, chrBankOffset, 8);
                            chrBankOffset += 8;
                            filePointer += 8;

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

                            var curSprite = Sprite.LoadFromIndices(paletteIndices);
                            curSprite.FilePointer = filePointer; // for debugging. 

                            if (eightBySixteenMode)
                            {
                                if (prevSprite == null)
                                {
                                    prevSprite = curSprite;
                                }
                                else
                                {
                                    var compositeSprite = CompositeSprite.Create([prevSprite, curSprite], SpriteOrientationEnum.Vertical);
                                    sheet.Sprites.Add(compositeSprite);
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

        public async Task Load(string filePath)
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                await Load(fs);
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
