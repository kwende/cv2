namespace Common
{
    public class SpriteSheet
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public List<ISprite> Sprites { get; set; } = new();
        public int SpriteWidth { get; set; }
        public int SpriteHeight { get; set; }
    }
}
