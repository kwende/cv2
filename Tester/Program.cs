using Common;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Tester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //https://www.nesdev.org/wiki/INES#iNES_file_format
            //https://fms.komkon.org/EMUL8/NES.html#LABM
            using (FileStream fin = File.OpenRead(@"C:\Users\ben\Downloads\cv2.nes"))
            {
                byte[] headerBytes = new byte[16];
                fin.Read(headerBytes, 0, headerBytes.Length);

                var span = new ReadOnlySpan<byte>(headerBytes);
                var header = MemoryMarshal.Read<iNesHeader>(span);

                if (header.Magic == Constants.Magic)
                {
                    // cpu-accessible program bank
                    var codeSize = header.PrgROMSize * 0x4000L;
                    var ppuSize = header.ChrROMSize * 0x2000L;

                    fin.Seek(codeSize + 16, SeekOrigin.Begin);

                    byte[] ppuPanel = new byte[ppuSize];

                    fin.Read(ppuPanel, 0, ppuPanel.Length);

                    const int panelSize = 8;

                    // there are 32 tiles. 
                    int i = 0;
                    for (int tileNumber = 0; tileNumber < 32; tileNumber++)
                    {
                        using (Bitmap bmp = new Bitmap(128, 128))
                        {
                            // each tile is a 16x16 grid of sprites
                            for (int y = 0; y < 16; y++)
                            {
                                for (int x = 0; x < 16; x++)
                                {
                                    var panel1 = new ReadOnlySpan<byte>(ppuPanel, i, panelSize);
                                    i += 8;
                                    var panel2 = new ReadOnlySpan<byte>(ppuPanel, i, panelSize);
                                    i += 8;

                                    for (int _y = 0; _y < panelSize; _y++)
                                    {
                                        var panel1Byte = panel1[_y];
                                        var panel2Byte = panel2[_y];

                                        for (int _x = 0; _x < panelSize; _x++)
                                        {
                                            byte mask = (byte)(0x80 >> _x);

                                            int panel1Bit = (panel1Byte & mask) != 0 ? 1 : 0;
                                            int panel2Bit = (panel2Byte & mask) != 0 ? 1 : 0;

                                            int paletteIndex = (panel1Bit << 1) | panel2Bit;

                                            switch (paletteIndex)
                                            {
                                                case 0:
                                                    bmp.SetPixel((x * panelSize) + _x, (y * panelSize) + _y, Color.Yellow);
                                                    break;
                                                case 1:
                                                    bmp.SetPixel((x * panelSize) + _x, (y * panelSize) + _y, Color.Red);
                                                    break;
                                                case 2:
                                                    bmp.SetPixel((x * panelSize) + _x, (y * panelSize) + _y, Color.Blue);
                                                    break;
                                                case 3:
                                                    bmp.SetPixel((x * panelSize) + _x, (y * panelSize) + _y, Color.Green);
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                            bmp.Save(@$"C:\Users\ben\Desktop\images\{tileNumber}.bmp");
                        }
                    }
                }
            }
        }
    }
}
