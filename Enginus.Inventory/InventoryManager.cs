using Enginus.Core;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Enginus.Inventory;

public sealed class InventoryManager
{
    #region Properties

    public Dictionary<InventoryItems, Item> ItemsCollection;
    public List<InventoryItems> InInventory;

    ContentManager content;
    static InventoryManager instance;

    #endregion

    #region Methods

    public static InventoryManager Instance(ContentManager content)
    {
        if (instance == null)
        {
            instance = new InventoryManager(content);
        }
        return instance;
    }
    private InventoryManager(ContentManager content)
    {
        ItemsCollection = new Dictionary<InventoryItems, Item>();
        InInventory = new List<InventoryItems>();
        this.content = content;
    }

    public void AddItem(Item item)
    {
        Item newItem = new Item(item, content);
        if (!ItemsCollection.ContainsKey(newItem.ItemName))
        {
            ItemsCollection.Add(newItem.ItemName, newItem);
            InInventory.Add(newItem.ItemName);
        }
    }
    public void RemoveItem(InventoryItems itemName)
    {
        ItemsCollection.Remove(itemName);
    }
    public bool HasItem(int itemId)
    {
        //TODO: Check later
        //return InInventory.Contains(itemName);
        return false;
    }

    #endregion
}