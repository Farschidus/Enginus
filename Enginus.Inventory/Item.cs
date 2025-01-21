using Enginus.Control;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Enginus.Inventory
{
	public enum ItemsEnum
    {
        Bamboo = 1,
        EmptyBucket = 2,
        Flag = 3,
        GrassDried = 4,
        Shell = 5,
        Matchbox = 6,
        Juice = 7,
        PlantRat = 8,
        Watch = 9,
        QardashMap = 10,
        Rope = 11
    }

    public class Item
    {
        public ItemsEnum ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }
        public Texture2D ItemTexture
        {
            get
            {
                return itemTexture;
            }
        }
        public Rectangle ItemRectangle
        {
            get
            {
                return itemRectangle;
            }
        }

        string textureName;
        ItemsEnum itemName;
        Texture2D itemTexture;
        Rectangle itemRectangle;
        bool isCombinable;
        ItemsEnum? combinePair;
        ItemsEnum target;
        Point position;
        const int ItemWithAndHeight = 70;
        bool inInventory;
        float layerDepth;

        public Item(Item item, ContentManager content)
            : this(item.itemName, item.isCombinable, item.combinePair, item.target, item.position, item.textureName, content, 0.89f)
        {
        }
        public Item(ItemsEnum itemName, bool isCombinable, ItemsEnum? combinePair, ItemsEnum target, Point position, string textureName, ContentManager content, float layerDepth)
        {
            this.textureName = textureName;
            this.itemName = itemName;
            this.isCombinable = isCombinable;
            if (combinePair.HasValue)
                this.combinePair = combinePair;
            this.target = target;
            this.position = position;
            itemRectangle = new Rectangle(position.X, position.Y, ItemWithAndHeight, ItemWithAndHeight);
            itemTexture = content.Load<Texture2D>(textureName);
            this.layerDepth = layerDepth;
        }

        public virtual void HandleInput(InputState input, Cursor cursor)
        {
            if (itemRectangle.Contains(input.CurrentMousePoint))
            {
                cursor.CursorType = CursorTexturType.Intract;
            }
        }
        public void Update(InputState input, Cursor cursor, InventoryManager manager)
        {
            if (input.MouseClicked && itemRectangle.Contains(input.MouseClickedPoint))
            {
                if (!inInventory)
                {
                    inInventory = true;
                    manager.AddItem(this);
                }              
            }
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(itemTexture, itemRectangle, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
        }
    }
}