using Common;
using System.Runtime.InteropServices;

namespace Tester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //https://www.nesdev.org/wiki/INES#iNES_file_format
            using (FileStream fin = File.OpenRead(@"C:\Users\ben\Downloads\cv2.nes"))
            {
                byte[] headerBytes = new byte[16];
                fin.Read(headerBytes, 0, headerBytes.Length);

                var span = new ReadOnlySpan<byte>(headerBytes);
                var header = MemoryMarshal.Read<iNesHeader>(span);

                if (header.Magic == Constants.Magic)
                {
                    // cpu-accessible program bank
                    fin.Seek(header.PrgROMSize * 0x4000, SeekOrigin.Current);

                    // ppu-accessibl chr bank w/ nametables, etc. 
                    // chr comes in two banks: first for backgrounds, second for sprites. 
                    byte[] chrBank0 = new byte[header.ChrROMSize * 0x2000];
                    byte[] chrBank1 = new byte[header.ChrROMSize * 0x2000];

                    fin.Read(chrBank0, 0, chrBank0.Length);
                    fin.Read(chrBank1, 0, chrBank1.Length);
                }
            }
        }
    }
}
