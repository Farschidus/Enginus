using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Enginus.Core.Utilities;

public static class Resolution
{
    static private GraphicsDeviceManager _Device = null;

    static private int _Width;
    static private int _Height;
    static private int _VWidth = Constants.SCREEN_WIDTH;
    static private int _VHeight = Constants.SCREEN_HEIGHT;
    static private Matrix _ScaleMatrix;
		static private bool _dirtyMatrix = true;
    static public Viewport GameViewPort
    {
        get
        {
            return _Device.GraphicsDevice.Viewport;
        }
    }
    static public bool IsFullScreen { get; private set; } = true;

    static public void Init(ref GraphicsDeviceManager device)
    {
        IsFullScreen = true;
        _Width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width; //  device.PreferredBackBufferWidth;
        _Height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height; // device.PreferredBackBufferHeight;
        _Device = device;
        _dirtyMatrix = true;
        ApplyResolutionSettings();
    }

    static private void RecreateScaleMatrix()
    {
        _dirtyMatrix = false;
        _ScaleMatrix = Matrix.CreateScale(
                       (float)_Device.GraphicsDevice.Viewport.Width / _VWidth,
                       (float)_Device.GraphicsDevice.Viewport.Height / _VHeight,
                       1f);
    }
    static private void ApplyResolutionSettings()
    {

#if XBOX360
       _FullScreen = true;
#endif

        // If we aren't using a full screen mode, the height and width of the window can
        // be set to anything equal to or smaller than the actual screen size.
        if (IsFullScreen == false)
        {
            if ((_Width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                && (_Height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height))
            {
                _Device.PreferredBackBufferWidth = _Width;
                _Device.PreferredBackBufferHeight = _Height;
                _Device.IsFullScreen = IsFullScreen;
                _Device.ApplyChanges();
            }
        }
        else
        {
            // If we are using full screen mode, we should check to make sure that the display
            // adapter can handle the video mode we are trying to set.  To do this, we will
            // iterate through the display modes supported by the adapter and check them against
            // the mode we want to set.
            foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                // Check the width and height of each mode against the passed values
                if ((dm.Width == _Width) && (dm.Height == _Height))
                {
                    // The mode is supported, so set the buffer formats, apply changes and return
                    _Device.PreferredBackBufferWidth = _Width;
                    _Device.PreferredBackBufferHeight = _Height;
                    _Device.IsFullScreen = IsFullScreen;
                    _Device.ApplyChanges();
                }
            }
        }

        _dirtyMatrix = true;

        _Width = _Device.PreferredBackBufferWidth;
        _Height = _Device.PreferredBackBufferHeight;
    }

    static public Matrix GetScaleMatrix()
    {
        if (_dirtyMatrix)
            RecreateScaleMatrix();

        return _ScaleMatrix;
    }
    /// <summary>
    /// Sets the device to use the draw pump
    /// Sets correct aspect ratio
    /// </summary>
    static public void BeginDraw()
    {
        // Start by reseting viewport to (0,0,1,1)
        FullViewport();
        // Clear to Black
        _Device.GraphicsDevice.Clear(Color.Black);
        // Calculate Proper Viewport according to Aspect Ratio
        ResetViewport();
        // and clear that
        // This way we are gonna have black bars if aspect ratio requires it and
        // the clear color on the rest
        //_Device.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);
        _Device.GraphicsDevice.Clear(Color.Black);
    }
    static public void FullViewport()
    {
        var vp = new Viewport();
        vp.X = vp.Y = 0;
        vp.Width = _Width;
        vp.Height = _Height;
        _Device.GraphicsDevice.Viewport = vp;
    }
    static public void ResetViewport()
    {
        float targetAspectRatio = GetVirtualAspectRatio();
        // figure out the largest area that fits in this resolution at the desired aspect ratio
        int width = _Device.PreferredBackBufferWidth;
        int height = (int)(width / targetAspectRatio + .5f);
        bool changed = false;

        if (height > _Device.PreferredBackBufferHeight)
        {
            height = _Device.PreferredBackBufferHeight;
            // PillarBox
            width = (int)(height * targetAspectRatio + .5f);
            changed = true;
        }

        // set up the new viewport centered in the backbuffer
        var viewport = new Viewport
        {
            X = (_Device.PreferredBackBufferWidth / 2) - (width / 2),
            Y = (_Device.PreferredBackBufferHeight / 2) - (height / 2),
            Width = width,
            Height = height,
            MinDepth = 0,
            MaxDepth = 1
        };

        if (changed)
        {
            _dirtyMatrix = true;
        }

        _Device.GraphicsDevice.Viewport = viewport;
    }
    /// <summary>
    /// Get virtual Mode Aspect Ratio
    /// </summary>
    /// <returns>aspect ratio</returns>
    static public float GetVirtualAspectRatio()
    {
        return (float)_VWidth / (float)_VHeight;
    }
    static public void SetVirtualResolution(int Width, int Height)
    {
        _VWidth = Width;
        _VHeight = Height;

        _dirtyMatrix = true;
    }
    static public void SetResolution(int Width, int Height, bool FullScreen)
    {
        _Width = Width;
        _Height = Height;

        IsFullScreen = FullScreen;
        ApplyResolutionSettings();
    }
}
