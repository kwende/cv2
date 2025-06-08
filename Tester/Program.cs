using Common;
using System.Drawing;

namespace Tester
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //https://www.nesdev.org/wiki/INES#iNES_file_format
            //https://fms.komkon.org/EMUL8/NES.html#LABM
            using (FileStream fin = File.OpenRead(@"C:\Users\ben\Downloads\cv2.nes"))
            {
                NesROM rom = new NesROM();
                await rom.Load(fin);
                var sheets = rom.GetSpriteSheets(true);

                int sheetNumber = 0;

                foreach (var sheet in sheets)
                {
                    using (Bitmap bmp = new Bitmap(sheet.Width, sheet.Height))
                    {
                        var sheetWidthInSprites = sheet.Width / sheet.SpriteWidth;
                        var sheetHeightInSprites = sheet.Height / sheet.SpriteHeight;

                        for (int _y = 0; _y < sheetHeightInSprites; _y++)
                        {
                            for (int _x = 0; _x < sheetWidthInSprites; _x++)
                            {
                                var sprite = sheet.Sprites[(_y * sheetWidthInSprites) + _x];

                                for (int y = 0; y < sprite.Height; y++)
                                {
                                    for (int x = 0; x < sprite.Width; x++)
                                    {
                                        var paletteIndex = sprite.PaletteIndices[(y * sprite.Width) + x];

                                        var spriteXInSheetCoordinates = (_x * sheet.SpriteWidth) + x;
                                        var spriteYInSheetCoordinates = (_y * sheet.SpriteHeight) + y;
                                        switch (paletteIndex)
                                        {
                                            case 0:
                                                bmp.SetPixel(spriteXInSheetCoordinates, spriteYInSheetCoordinates, Color.Yellow);
                                                break;
                                            case 1:
                                                bmp.SetPixel(spriteXInSheetCoordinates, spriteYInSheetCoordinates, Color.Red);
                                                break;
                                            case 2:
                                                bmp.SetPixel(spriteXInSheetCoordinates, spriteYInSheetCoordinates, Color.Blue);
                                                break;
                                            case 3:
                                                bmp.SetPixel(spriteXInSheetCoordinates, spriteYInSheetCoordinates, Color.Green);
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                            }
                        }

                        bmp.Save($"C:/users/ben/desktop/images/{sheetNumber++}.bmp");
                    }
                }
            }
        }
    }
}
