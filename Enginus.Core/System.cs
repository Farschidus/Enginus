using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Enginus.Core;

/// <summary>
/// An abstract base class for systems in the game.
/// </summary>
public abstract class System
{
    /// <summary>
    /// Adds an gameObject to the system.
    /// </summary>
    /// <param name="gameObject">The gameObject to add.</param>
    public virtual void AddGameObject(GameObject gameObject)
    {

    }

    /// <summary>
    /// Removes an gameObject from the system.
    /// </summary>
    /// <param name="gameObject">The gameObject to remove.</param>
    public virtual void RemoveGameObject(GameObject gameObject)
    {

    }

    /// <summary>
    /// Subscribes system to the MessageBus events.
    /// </summary>
    public virtual void Subscribe()
    {

    }

    /// <summary>
    /// Unsubscribes system from the MessageBus events.
    /// </summary>
    public virtual void Unsubscribe()
    {

    }

    /// <summary>
    /// Updates the system.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
    public virtual void Update(GameTime gameTime)
    {

    }

    /// <summary>
    /// Draws the system.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to use for drawing.</param>
    public virtual void Draw(SpriteBatch spriteBatch)
    {

    }

}

