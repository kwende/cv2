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

                }
            }
        }
    }
}
