using System.Runtime.InteropServices;

namespace Common
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct iNesHeader
    {
        public int Magic;
        public byte PrgROMSize;
        public byte ChrROMSize;
        public byte Flags6;
        public byte Flags7;
        public byte Flags8;
        public byte Flags9;
        public byte Flags10;
        public fixed byte Padding[5];
    }
}
