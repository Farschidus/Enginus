using Microsoft.Xna.Framework.Graphics;

namespace Enginus.Animation
{
	public class SpriteFile
    {
        public string Texture { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class TextureSpriteFile
    {
        public Texture2D Texture { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
