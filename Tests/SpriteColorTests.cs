using Common;

namespace Tests
{
    [TestClass]
    public sealed class SpriteColorTests
    {
        [TestMethod]
        public void CompositeSprite_ComprisedOfCompositeSprite_MakesSquare()
        {
            int[] paletteIndices1 = new int[8 * 8];
            Array.Fill<int>(paletteIndices1, 0);
            var sprite1 = Sprite.Load(paletteIndices1);

            int[] paletteIndices2 = new int[8 * 8];
            Array.Fill<int>(paletteIndices2, 1);
            var sprite2 = Sprite.Load(paletteIndices2);

            var lhs = CompositeSprite.Create([sprite1, sprite2], SpriteOrientationEnum.Vertical);

            int[] paletteIndices3 = new int[8 * 8];
            Array.Fill<int>(paletteIndices3, 2);
            var sprite3 = Sprite.Load(paletteIndices3);

            int[] paletteIndices4 = new int[8 * 8];
            Array.Fill<int>(paletteIndices4, 3);
            var sprite4 = Sprite.Load(paletteIndices4);

            var rhs = CompositeSprite.Create([sprite3, sprite4], SpriteOrientationEnum.Vertical);

            var square = CompositeSprite.Create([lhs, rhs], SpriteOrientationEnum.Horizontal);

            var squarePaletteIndices = square.PaletteIndices;

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var index = squarePaletteIndices[(y * square.Width) + x];
                    Assert.AreEqual(0, index);
                }
            }

            for (int y = 0; y < 8; y++)
            {
                for (int x = 8; x < 16; x++)
                {
                    var index = squarePaletteIndices[(y * square.Width) + x];
                    Assert.AreEqual(2, index);
                }
            }

            for (int y = 8; y < 16; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var index = squarePaletteIndices[(y * square.Width) + x];
                    Assert.AreEqual(1, index);
                }
            }

            for (int y = 8; y < 16; y++)
            {
                for (int x = 8; x < 16; x++)
                {
                    var index = squarePaletteIndices[(y * square.Width) + x];
                    Assert.AreEqual(3, index);
                }
            }
        }

        [TestMethod]
        public void CompositeSprite_CombinedFromTwoSpritesHorizontally_Works()
        {
            int[] paletteIndices1 = new int[8 * 8];
            Array.Fill<int>(paletteIndices1, 1);
            var sprite1 = Sprite.Load(paletteIndices1);

            int[] paletteIndices2 = new int[8 * 8];
            Array.Fill<int>(paletteIndices2, 2);
            var sprite2 = Sprite.Load(paletteIndices2);

            var compositeSprite = CompositeSprite.Create([sprite1, sprite2], SpriteOrientationEnum.Horizontal);

            Assert.AreEqual(8, compositeSprite.Height);
            Assert.AreEqual(16, compositeSprite.Width);

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var paletteindex = compositeSprite.PaletteIndices[(y * compositeSprite.Width) + x];
                    Assert.AreEqual(1, paletteindex);
                }
            }

            for (int y = 0; y < compositeSprite.Height; y++)
            {
                for (int x = 8; x < 16; x++)
                {
                    var paletteindex = compositeSprite.PaletteIndices[(y * compositeSprite.Width) + x];
                    Assert.AreEqual(2, paletteindex);
                }
            }
        }

        [TestMethod]
        public void CompositeSprite_CombinedFromTwoSpritesVertically_Works()
        {
            int[] paletteIndices1 = new int[8 * 8];
            Array.Fill<int>(paletteIndices1, 1);
            var sprite1 = Sprite.Load(paletteIndices1);

            int[] paletteIndices2 = new int[8 * 8];
            Array.Fill<int>(paletteIndices2, 2);
            var sprite2 = Sprite.Load(paletteIndices2);

            var compositeSprite = CompositeSprite.Create([sprite1, sprite2], SpriteOrientationEnum.Vertical);

            Assert.AreEqual(16, compositeSprite.Height);
            Assert.AreEqual(8, compositeSprite.Width);

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var paletteindex = compositeSprite.PaletteIndices[(y * compositeSprite.Width) + x];
                    Assert.AreEqual(1, paletteindex);
                }
            }

            for (int y = 8; y < 16; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var paletteindex = compositeSprite.PaletteIndices[(y * compositeSprite.Width) + x];
                    Assert.AreEqual(2, paletteindex);
                }
            }
        }

        [TestMethod]
        public void Sprite_ToRGBA_Works()
        {
            var hex0 = NesColorsUtils.FullNesPalette[0];
            var hex1 = NesColorsUtils.FullNesPalette[1];
            var hex2 = NesColorsUtils.FullNesPalette[2];

            var index0 = NesColorsUtils.HexColorCodeToNesColorIndex(hex0);
            var index1 = NesColorsUtils.HexColorCodeToNesColorIndex(hex1);
            var index2 = NesColorsUtils.HexColorCodeToNesColorIndex(hex2);

            int[] indices = [
                index0, index1, index2
            ];

            Assert.AreEqual(0, indices[0]);
            Assert.AreEqual(1, indices[1]);
            Assert.AreEqual(2, indices[2]);

            int[] spriteIndices = new int[8 * 8];
            for (int y = 0, i = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++, i++)
                {
                    spriteIndices[i] = indices[i % 3] + 1;
                }
            }

            var sprite = Sprite.Load(spriteIndices);

            var bytes = sprite.ToRGBA(NesColorsUtils.NesColorIndexToNesColor(index0),
                NesColorsUtils.NesColorIndexToNesColor(index1),
                NesColorsUtils.NesColorIndexToNesColor(index2)).ToArray();

            for (int y = 0, i = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++, i++)
                {
                    var colorIndex = i * 4;
                    var r = bytes[colorIndex];
                    var g = bytes[colorIndex + 1];
                    var b = bytes[colorIndex + 2];
                    var a = bytes[colorIndex + 3];

                    var index = indices[i % 3];

                    var color = NesColorsUtils.NesColorIndexToNesColor(index);

                    uint hexAsInt = Convert.ToUInt32($"{color}ff".Replace("#", "0x"), 16);

                    Assert.AreEqual((hexAsInt >> 24) & 0x000000ff, r);
                    Assert.AreEqual((hexAsInt >> 16) & 0x000000ff, g);
                    Assert.AreEqual((hexAsInt >> 8) & 0x000000ff, b);
                    Assert.AreEqual(255, a);
                }
            }
        }
    }
}
