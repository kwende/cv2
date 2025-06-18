using Common;

namespace Tests
{
    [TestClass]
    public class RomTests
    {

        [TestMethod]
        public async Task NesRom_SavingSpriteSheetsWithNoChange_PreservesOriginalFile()
        {
            NesROM rom = new NesROM();
            long fileSize = 0;
            using (FileStream fs = File.OpenRead(@"C:\Users\ben\Downloads\cv2.nes"))
            {
                await rom.Load(fs);
                fileSize = fs.Length;
            }

            var spriteSheets = await rom.GetSpriteSheets(true);
            await rom.SaveSpriteSheets(spriteSheets);

            MemoryStream memStream = new MemoryStream();
            await rom.SaveToStream(memStream);

            Assert.AreEqual(fileSize, memStream.Length);
            int bytesExamined = 0;
            using (FileStream fs = File.OpenRead(@"C:\Users\ben\Downloads\cv2.nes"))
            {
                memStream.Seek(0, SeekOrigin.Begin);
                int fileByte = 0;
                while ((fileByte = fs.ReadByte()) != -1)
                {
                    var memoryByte = memStream.ReadByte();
                    Assert.AreEqual(fileByte, memoryByte);
                    bytesExamined++;
                }
            }
        }

        [TestMethod]
        public async Task NesROM_WhenSavingNoChanges_PreservesOriginalFile()
        {
            NesROM rom = new NesROM();
            long fileSize = 0;
            using (FileStream fs = File.OpenRead(@"C:\Users\ben\Downloads\cv2.nes"))
            {
                await rom.Load(fs);
                fileSize = fs.Length;
            }

            MemoryStream memStream = new MemoryStream();
            await rom.SaveToStream(memStream);

            Assert.AreEqual(fileSize, memStream.Length);
        }

        [TestMethod]
        public async Task ChangeSimonTest()
        {
            NesROM rom = new NesROM();
            await rom.Load(@"C:\Users\ben\Downloads\cv2.nes");

            var spriteSheets = await rom.GetSpriteSheets(true);

            var simonSheet = spriteSheets[1];

            var sprite1 = simonSheet.Sprites[1];
            var sprite2 = simonSheet.Sprites[2];

            var compositeSimon = CompositeSprite.Create([sprite1, sprite2], SpriteOrientationEnum.Horizontal);

            compositeSimon.SetPaletteIndex(0, 0, 1);

            var compositeSimonSprites = compositeSimon.Sprites.ToArray();

            simonSheet.Sprites[1] = compositeSimonSprites[0];
            simonSheet.Sprites[2] = compositeSimonSprites[1];

            await rom.SaveSpriteSheets(new List<SpriteSheet>([simonSheet]));
            await rom.SaveToFile("C:/users/ben/downloads/test.nes");
        }
    }
}
