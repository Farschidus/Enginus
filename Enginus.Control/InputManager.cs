using Enginus.Core;
using Enginus.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Enginus.Control;

/// <summary>
/// Helper for reading input from keyboard, gamepad, and touch input. This class 
/// tracks both the current and previous state of the input devices, and implements 
/// query methods for high level input actions such as "move up through the menu"
/// or "pause the game".
/// </summary>
public class InputManager
{
    #region Fields

    private double timePassed;
    private bool isFullScreen;
    private Viewport gameViewport;
    private MouseState lastMouseStates;
    private MouseState currentMouseStates;
    private double previousGameTime;
    private const double TimerDelay = Constants.DOUBLE_CLICK_SPEED; 
    private Point mouseClickedPoint;

    public Point CurrentMousePoint;
    public const int MaxInputs = 1;
    public readonly KeyboardState[] CurrentKeyboardStates;
    public readonly KeyboardState[] LastKeyboardStates;
    public bool MouseClicked
    {
        get
        {
            if (currentMouseStates.LeftButton == ButtonState.Pressed && lastMouseStates.LeftButton == ButtonState.Released)
            {
                mouseClickedPoint = Mouse.GetState().Position;
                return true;
            }
            else
                return false;

        }
    }
    public bool MouseRightClicked
    {
        get
        {
            return (lastMouseStates.RightButton == ButtonState.Pressed && currentMouseStates.RightButton == ButtonState.Released);
        }
    }
    public bool DoubleClicked;

    public Point MouseClickedPoint { get => mouseClickedPoint; }
    //TODO: for picking up items from inventory
    public object MouseActiveInventoryItem { get; set; }

    #endregion

    #region Initialization

    /// <summary>
    /// Constructs a new input state.
    /// </summary>
    public InputManager(Viewport gameViewPort, bool isFullScreen)
    {
        this.isFullScreen = isFullScreen;
        CurrentKeyboardStates = new KeyboardState[MaxInputs];
        LastKeyboardStates = new KeyboardState[MaxInputs];
        this.gameViewport = gameViewPort;

        mouseClickedPoint = Point.Zero;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Reads the latest state of the keyboard and Mouse.
    /// </summary>
    public void Update(GameTime gameTime)
    {
        for (int i = 0; i < MaxInputs; i++)
        {
            LastKeyboardStates[i] = CurrentKeyboardStates[i];
            CurrentKeyboardStates[i] = Keyboard.GetState();
        }

        if (isFullScreen)
        {
            if (Mouse.GetState().Y <= gameViewport.Y)
                Mouse.SetPosition(Mouse.GetState().X, gameViewport.Y);
            if (Mouse.GetState().Y >= gameViewport.Y + gameViewport.Height - 1)
                Mouse.SetPosition(Mouse.GetState().X, gameViewport.Y + gameViewport.Height - 1);
            if (Mouse.GetState().X <= gameViewport.X)
                Mouse.SetPosition(gameViewport.X, Mouse.GetState().Y);
            if (Mouse.GetState().X >= gameViewport.X + gameViewport.Width)
                Mouse.SetPosition(gameViewport.X + gameViewport.Width, Mouse.GetState().Y);
        }

        lastMouseStates = currentMouseStates;
        currentMouseStates = Mouse.GetState();

        var MouseStateVector = new Vector2(currentMouseStates.X - Resolution.GameViewPort.X, currentMouseStates.Y - Resolution.GameViewPort.Y);
        MouseStateVector = Vector2.Transform(MouseStateVector, Matrix.Invert(Resolution.GetScaleMatrix()));
        CurrentMousePoint = new Point((int)Math.Round(MouseStateVector.X), (int)Math.Round(MouseStateVector.Y));
        CheckMouseClick(gameTime);
    }

    /// <summary>
    /// Helper for checking if a key was newly pressed during this update. The
    /// controllingPlayer parameter specifies which player to read input for.
    /// If this is null, it will accept input from any player. When a keypress
    /// is detected, the output playerIndex reports which player pressed it.
    /// </summary>
    public bool IsNewKeyPress(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
    {
        if (controllingPlayer.HasValue)
        {
            // Read input from the specified player.
            playerIndex = controllingPlayer.Value;

            int i = (int)playerIndex;

            return (CurrentKeyboardStates[i].IsKeyDown(key) &&
                    LastKeyboardStates[i].IsKeyUp(key));
        }
        else
        {
            // Accept input from any player.
            return IsNewKeyPress(key, PlayerIndex.One, out playerIndex) ||
                    IsNewKeyPress(key, PlayerIndex.Two, out playerIndex) ||
                    IsNewKeyPress(key, PlayerIndex.Three, out playerIndex) ||
                    IsNewKeyPress(key, PlayerIndex.Four, out playerIndex);
        }
    }
    /// <summary>
    /// Helper for checking if a key was newly pressed during this update.
    /// it will accept input from player one.
    /// </summary>
    public bool IsNewKeyPress(Keys key) => (CurrentKeyboardStates[(int)PlayerIndex.One].IsKeyDown(key) &&
        LastKeyboardStates[(int)PlayerIndex.One].IsKeyUp(key));
    /// <summary>
    /// Checks for a "menu select" input action.
    /// The controllingPlayer parameter specifies which player to read input for.
    /// If this is null, it will accept input from any player. When the action
    /// is detected, the output playerIndex reports which player pressed it.
    /// </summary>
    public bool IsMenuSelect() => IsNewKeyPress(Keys.Space) || IsNewKeyPress(Keys.Enter);  
    //public bool IsMenuSelectByMouse(List<Rectangle> menuEntries)
    //{
    //    MenuEntry selectedMenuEntry = null;
    //    foreach (var menuItem in menuEntries)
    //    {
    //        new Rectangle(menuItem.X, menuItem.Y, menuItem.Width, menuItem.Height).Contains(CurrentMousePoint);
    //        break;
    //    }

    //    if (IsLeftMouseClicked())
    //    {
    //        Point mouseLocation = new Point(CurrentMouseStates.X, CurrentMouseStates.Y);
    //    }
    //    return true;
    //}
    /// <summary>
    /// Checks for a "menu cancel" input action.
    /// The controllingPlayer parameter specifies which player to read input for.
    /// If this is null, it will accept input from any player. When the action
    /// is detected, the output playerIndex reports which player pressed it.
    /// </summary>
    public bool IsMenuCancel() => IsNewKeyPress(Keys.Escape);
    /// <summary>
    /// Checks for a "menu up" input action.
    /// The controllingPlayer parameter specifies which player to read
    /// input for. If this is null, it will accept input from any player.
    /// </summary>
    public bool IsMenuUp() => IsNewKeyPress(Keys.Up);
    /// <summary>
    /// Checks for a "menu down" input action.
    /// The controllingPlayer parameter specifies which player to read
    /// input for. If this is null, it will accept input from any player.
    /// </summary>
    public bool IsMenuDown() => IsNewKeyPress(Keys.Down);
    /// <summary>
    /// Checks for a "pause the game" input action.
    /// The controllingPlayer parameter specifies which player to read
    /// input for. If this is null, it will accept input from any player.
    /// </summary>
    public bool IsPauseGame() => IsNewKeyPress(Keys.Escape);

    #endregion

    #region Keyboard

    public bool KeyS()
    {
        return IsNewKeyPress(Keys.S);
    }
    public bool KeyA()
    {
        return IsNewKeyPress(Keys.A);
    }
    public bool KeyE()
    {
        return IsNewKeyPress(Keys.E);
    }
    public bool KeyD()
    {
        return IsNewKeyPress(Keys.D);
    }
    public bool KeyN()
    {
        return IsNewKeyPress(Keys.N);
    }
    public bool KeyL()
    {
        return IsNewKeyPress(Keys.L);
    }
    public bool KeyM()
    {
        return IsNewKeyPress(Keys.M);
    }

    #endregion

    #region Mouse

    private void CheckMouseClick(GameTime gameTime)
    {
        timePassed = gameTime.TotalGameTime.Milliseconds - previousGameTime;
        DoubleClicked = timePassed > 0 && timePassed < TimerDelay &&
            currentMouseStates.LeftButton == ButtonState.Released && lastMouseStates.LeftButton == ButtonState.Pressed;
    }

    public void CenterMousePosition()
    {
        Mouse.SetPosition(gameViewport.Width / 2, gameViewport.Height / 2);
    }

    #endregion
}