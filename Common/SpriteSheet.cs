namespace Common
{
    public class SpriteSheet
    {
        public long FilePosition { get; set; }
        public bool EightBySixteen { get; set; }
        public int SheetNumber { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<ISprite> Sprites { get; set; } = new();
        public int SpriteWidth { get; set; }
        public int SpriteHeight { get; set; }
    }
}
