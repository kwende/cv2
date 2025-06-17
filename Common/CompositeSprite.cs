using System.Drawing;

namespace Common
{
    public class CompositeSprite : ISprite
    {
        private IEnumerable<ISprite> _sprites;

        public IReadOnlyCollection<ISprite> Sprites => _sprites.ToList().AsReadOnly();
        public SpriteOrientationEnum Orientation { get; set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        private int _spriteWidth = 0, _spriteHeight = 0;

        public int[] PaletteIndices
        {
            get
            {
                int[] ret = new int[Width * Height];
                var spriteArray = Sprites.ToArray();

                // there might be a better way to do this. 
                int cornerX = 0, cornerY = 0;
                for (int i = 0; i < Sprites.Count; i++)
                {
                    var sprite = spriteArray[i];
                    for (int y = cornerY, srcIndex = 0; y < (cornerY + _spriteHeight); y++)
                    {
                        for (int x = cornerX; x < (cornerX + _spriteWidth); x++, srcIndex++)
                        {
                            ret[(y * Width) + x] = sprite.PaletteIndices[srcIndex];
                        }
                    }

                    if (Orientation == SpriteOrientationEnum.Horizontal)
                    {
                        cornerX += _spriteWidth;
                    }
                    else
                    {
                        cornerY += _spriteHeight;
                    }
                }

                return ret;
            }
        }

        public int SpriteIndex { get; set; }
        public int SheetNumber { get; set; }

        public static CompositeSprite Create(IEnumerable<ISprite> sprites, SpriteOrientationEnum orientation)
        {
            ArgumentNullException.ThrowIfNull(sprites, nameof(sprites));
            ArgumentOutOfRangeException.ThrowIfLessThan(sprites.Count(), 2, nameof(sprites));

            int width = sprites.First().Width;
            int height = sprites.First().Height;

            bool dimensionValid = sprites.All(n => n.Width == width && n.Height == height);

            if (!dimensionValid)
            {
                throw new ArgumentOutOfRangeException($"All sprites must be the same dimension.", nameof(sprites));
            }

            if (orientation == SpriteOrientationEnum.Horizontal)
            {
                width = width * sprites.Count();
            }
            else if (orientation == SpriteOrientationEnum.Vertical)
            {
                height = height * sprites.Count();
            }
            else
            {
                throw new ArgumentException("Unsupported orientation.", nameof(orientation));
            }

            return new CompositeSprite(width, height, sprites, orientation);
        }

        private CompositeSprite(int width, int height, IEnumerable<ISprite> sprites, SpriteOrientationEnum orientation)
        {
            Orientation = orientation;
            _sprites = sprites;

            Width = width;
            Height = height;

            _spriteWidth = _sprites.First().Width;
            _spriteHeight = _sprites.First().Height;
        }

        public void SetPaletteIndex(int x, int y, int paletteIndex)
        {
            int spriteIndex = 0;
            int spriteX = 0, spriteY = 0;
            switch (Orientation)
            {
                case SpriteOrientationEnum.Horizontal:
                    spriteIndex = x / _spriteWidth;
                    spriteX = x % _spriteWidth;
                    spriteY = y;
                    break;
                case SpriteOrientationEnum.Vertical:
                    spriteIndex = y / _spriteHeight;
                    spriteX = x;
                    spriteY = y % _spriteHeight;
                    break;
            }

            var sprite = _sprites.Skip(spriteIndex).Take(1).First();
            sprite.SetPaletteIndex(spriteX, spriteY, paletteIndex);
        }

        public IEnumerable<byte> ToRGBA(NesColor color1, NesColor color2, NesColor color3)
        {
            ArgumentNullException.ThrowIfNull(color1, nameof(color1));
            ArgumentNullException.ThrowIfNull(color2, nameof(color2));
            ArgumentNullException.ThrowIfNull(color3, nameof(color3));

            byte[] bytes = new byte[Width * Height * 4];

            byte[][] colors = [
                color1.ColorRGBBytes, color2.ColorRGBBytes, color3.ColorRGBBytes,
            ];

            var paletteIndicies = PaletteIndices;
            for (int srcIndex = 0; srcIndex < Width * Height; srcIndex++)
            {
                var paletteIndex = paletteIndicies[srcIndex];

                byte[] color;
                if (paletteIndex > 0)
                {
                    color = colors[paletteIndex - 1];
                }
                else
                {
                    color = [0xFF, 0xFF, 0xFF, 0xFF];
                }

                var destIndex = srcIndex * 4;
                bytes[destIndex] = color[0];
                bytes[destIndex + 1] = color[1];
                bytes[destIndex + 2] = color[2];
                bytes[destIndex + 3] = color[3];
            }

            return bytes;
        }

        public IEnumerable<string> ToHex(NesColor color1, NesColor color2, NesColor color3)
        {
            ArgumentNullException.ThrowIfNull(color1, nameof(color1));
            ArgumentNullException.ThrowIfNull(color2, nameof(color2));
            ArgumentNullException.ThrowIfNull(color3, nameof(color3));

            List<IEnumerable<string>> bytes = new();
            foreach (var sprite in _sprites)
            {
                bytes.Add(sprite.ToHex(color1, color2, color3));
            }
            return bytes.SelectMany(n => n);
        }

        public Bitmap ToBitmap(NesColor color1, NesColor color2, NesColor color3)
        {
            throw new NotImplementedException();
        }

        public List<ISprite> Flatten(bool eightBySixteenMode)
        {
            List<ISprite> ret = new List<ISprite>();
            foreach (var sprite in _sprites)
            {
                RecurseTillBaseSprite(sprite, ret, eightBySixteenMode);
            }
            return ret;
        }

        private void RecurseTillBaseSprite(ISprite parent, List<ISprite> sprites, bool eightBySixteenMode)
        {
            if (!eightBySixteenMode && parent is Sprite)
            {
                sprites.Add(parent);
            }
            else if (eightBySixteenMode && parent.Height == 16 && parent.Width == 8)
            {
                sprites.Add(parent);
            }
            else
            {
                var childSprites = parent.Flatten(eightBySixteenMode);
                foreach (var childSprite in childSprites)
                {
                    RecurseTillBaseSprite(childSprite, sprites, eightBySixteenMode);
                }
            }
        }
    }
}
