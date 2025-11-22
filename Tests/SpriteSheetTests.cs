//using Common;

//namespace Tests
//{
//    [TestClass]
//    public class SpriteSheetTests
//    {
//        [TestMethod]
//        public async Task SpriteSheet_WhenLoadedEightBySixteen_HasLength8x16Sprites()
//        {
//            NesROM rom = new NesROM();
//            await rom.Load(@"C:\Users\ben\Downloads\cv2.nes");

//            var spriteSheets = await rom.GetSpriteSheets(true);
//            var spriteSheetToExamine = spriteSheets[0];

//            Assert.AreEqual(8, spriteSheetToExamine.SpriteWidth);
//            Assert.AreEqual(16, spriteSheetToExamine.SpriteHeight);
//            Assert.AreEqual(128, spriteSheetToExamine.Sprites.Count);

//            var sprite = spriteSheetToExamine.Sprites[0];
//            Assert.AreEqual(8, sprite.Width);
//            Assert.AreEqual(16, sprite.Height);

//            var paletteIndices = sprite.PaletteIndices;
//            Assert.AreEqual(8 * 16, paletteIndices.Length);
//        }

//        [TestMethod]
//        public async Task SpriteSheet_WhenNotLoadedEightBySixteen_HasLength8x8Sprites()
//        {
//            NesROM rom = new NesROM();
//            await rom.Load(@"C:\Users\ben\Downloads\cv2.nes");

//            var spriteSheets = await rom.GetSpriteSheets(false);
//            var spriteSheetToExamine = spriteSheets[0];

//            Assert.AreEqual(8, spriteSheetToExamine.SpriteWidth);
//            Assert.AreEqual(8, spriteSheetToExamine.SpriteHeight);
//            Assert.AreEqual(256, spriteSheetToExamine.Sprites.Count);

//            var sprite = spriteSheetToExamine.Sprites[0];
//            Assert.AreEqual(8, sprite.Width);
//            Assert.AreEqual(8, sprite.Height);

//            var paletteIndices = sprite.PaletteIndices;
//            Assert.AreEqual(8 * 8, paletteIndices.Length);
//        }
//    }
//}
