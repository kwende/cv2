namespace Common.Layout
{
    public class SpriteMenuGroupLayout
    {
        public string Game { get; set; }
        public string Title { get; set; }
        public List<SpriteMenuSpriteGroup> SpriteGroups { get; set; }
        public int Color1PaletteIndex { get; set; }
        public int Color2PaletteIndex { get; set; }
        public int Color3PaletteIndex { get; set; }
    }
}
