using Enginus.Control;
using Enginus.Core;
using Enginus.Core.Utilities;
using Enginus.Screen;
using Jint.Parser;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Enginus.MenuScreens;

/// <summary>
/// The options screen is brought up over the top of the main menu
/// screen, and gives the user a chance to configure the game
/// in various hopefully useful ways.
/// </summary>
class OptionsMenu : Menu
{
    #region Fields

    MenuEntry ungulateMenuEntry;
    MenuEntry languageMenuEntry;
    MenuEntry frobnicateMenuEntry;
    MenuEntry elfMenuEntry;
    MenuEntry resolutionMenuEntry;
    List<DisplayMode> supportedResolutions;
    private int currentResolutionIndex;

    enum Ungulate
    {
        BactrianCamel,
        Dromedary,
        Llama,
    }

    static Ungulate currentUngulate = Ungulate.Dromedary;

    static string[] languages = { "C#", "French", "Deoxyribonucleic acid" };
    static int currentLanguage;

    static bool frobnicate = true;

    static int elf = 23;

    #endregion

    #region Initialization

    /// <summary>
    /// Constructor.
    /// </summary>
    public OptionsMenu() : base("Options")
    {
        supportedResolutions = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes
            .OrderByDescending(m => m.Width * m.Height)
            .ToList();
        supportedResolutions.ForEach(x => Debug.Write($"{x.Width} x {x.Height} | "));

        // Create our menu entries.
        ungulateMenuEntry = new MenuEntry("ungulateMenuEntry");
        languageMenuEntry = new MenuEntry("languageMenuEntry");
        frobnicateMenuEntry = new MenuEntry("frobnicateMenuEntry");
        elfMenuEntry = new MenuEntry("elfMenuEntry");
        resolutionMenuEntry = new MenuEntry("resolutionMenuEntry");

        SetMenuEntryText();

        MenuEntry back = new MenuEntry("Back");

        // Hook up menu event handlers.
        ungulateMenuEntry.Selected += UngulateMenuEntrySelected;
        languageMenuEntry.Selected += LanguageMenuEntrySelected;
        frobnicateMenuEntry.Selected += FrobnicateMenuEntrySelected;
        elfMenuEntry.Selected += ElfMenuEntrySelected;
        resolutionMenuEntry.Selected += ResolutionMenuEntrySelected;
        resolutionMenuEntry.Changed += ResolutionMenuEntryChanged;
        back.Selected += OnCancel;
        
        // Add entries to the menu.
        MenuEntries.Add(ungulateMenuEntry);
        MenuEntries.Add(languageMenuEntry);
        MenuEntries.Add(frobnicateMenuEntry);
        MenuEntries.Add(elfMenuEntry);
        MenuEntries.Add(resolutionMenuEntry);
        MenuEntries.Add(back);
    }

    /// <summary>
    /// Fills in the latest values for the options screen menu text.
    /// </summary>
    void SetMenuEntryText()
    {
        ungulateMenuEntry.Text = "Preferred ungulate: " + currentUngulate;
        languageMenuEntry.Text = "Language: " + languages[currentLanguage];
        frobnicateMenuEntry.Text = "Frobnicate: " + (frobnicate ? "on" : "off");
        elfMenuEntry.Text = "elf: " + elf;
        resolutionMenuEntry.Text = supportedResolutions[currentResolutionIndex].Width + " x " + supportedResolutions[currentResolutionIndex].Height;
    }

    #endregion

    #region Handle Input

    /// <summary>
    /// Event handler for when the Ungulate menu entry is selected.
    /// </summary>
    void UngulateMenuEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        currentUngulate++;

        if (currentUngulate > Ungulate.Llama)
            currentUngulate = 0;

        SetMenuEntryText();
    }
    /// <summary>
    /// Event handler for when the Language menu entry is selected.
    /// </summary>
    void LanguageMenuEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        currentLanguage = (currentLanguage + 1) % languages.Length;

        SetMenuEntryText();
    }
    /// <summary>
    /// Event handler for when the Frobnicate menu entry is selected.
    /// </summary>
    void FrobnicateMenuEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        frobnicate = !frobnicate;

        SetMenuEntryText();
    }
    /// <summary>
    /// Event handler for when the Elf menu entry is selected.
    /// </summary>
    void ElfMenuEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        elf++;

        SetMenuEntryText();
    }

    private void ResolutionMenuEntryChanged(object sender, PlayerIndexEventArgs e)
    {
        if (ScreenManager.InputManager.IsMenuRight())
        {
            currentResolutionIndex = (currentResolutionIndex + 1) % supportedResolutions.Count;
        }
        if (ScreenManager.InputManager.IsMenuLeft())
        {
            currentResolutionIndex = (currentResolutionIndex - 1 + supportedResolutions.Count) % supportedResolutions.Count;
        }

        resolutionMenuEntry.Text = supportedResolutions[currentResolutionIndex].Width + " x " + supportedResolutions[currentResolutionIndex].Height;
    }
    private void ResolutionMenuEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        if (ScreenManager.InputManager.IsNewKeyPress(Keys.Enter))
        {
            var mode = supportedResolutions[currentResolutionIndex];
            Resolution.SetResolution(mode.Width, mode.Height, Constants.FULL_SCREEN);
        }
    }

    #endregion
}
