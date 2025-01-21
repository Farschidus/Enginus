using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Enginus.Global
{
	public class Helper
    {
        // a handy little function that gives a random float between two
        // values. This will be used in several places in the sample, in particilar in
        // ParticleSystem.InitializeParticle.
        private static Random random = new Random();
        public static float RandomBetween(float min, float max)
        {
            return min + (float)random.NextDouble() * (max - min);
        }
    }

    public class Enums
    {
        public enum Characters
        {
            Mizuki = 1,
            Baker = 2,
            Dabchick = 3,
            Secretary = 4,
            Soldier = 5,
            Scarecrow = 6,
            OwlTall = 7,
            OwlFat = 8,
            Mardak = 9,
            Hawaii = 10,
            SuperFrog = 11,
            BadGuy = 12,
            Peter = 13,
            Girl = 14,
            OperaCat = 15,
            Moose = 16,
            Rat = 17,
            Elephant = 18,
            LobbyGuard = 19,
            Qardash = 20,
            Psychotherap = 21,
            Spaghettiman = 22,
            Florist = 23,
            Monkey = 24,
            Vanya = 25,
            Tramp = 26,
            Ghost = 27,
            York = 28,
            Frogs = 29,
            ShadowMan = 30,
            FatWoman = 31
        }
        public enum Direction
        {
            North,
            South,
            East,
            West,
            NorthEast,
            NorthWest,
            SouthEast,
            SouthWest,
            Unknown
        }
    }

    public class Constants
    {
        //Integers
        public const int GameOriginalWidth = 1920;
        public const int GameOriginalHeight = 1080;
        //Floats
        public const float FanSpeed = 0.070f;
        //Scene Names
        public const string Scene_Bakery = "Bakery";
        public const string Scenes_GameAssets_File = "GameAssets";
        public const string Scenes_PuzzlePopups_File = "PuzzlePopups";
        //Interacives
        public const string Interacive_SwitchOff = "Images/SwitchOff";
        public const string Interacive_SwitchOn = "Images/SwitchOn";
        //Obstacles
        public const string Forground_BakerCasheer = "Forgrounds/BakerCasheer";
        //Images
        public const string Image_ExitObjects = "Images/placeHolder"; //"Dialogues/DialogueBoxBackground";
        public const string Image_PlaceHolder = "Images/placeHolder"; 
        public const string Image_DialogueBg = "Dialogues/DialogueBoxBackground";
        public const string Image_MapBg = "Images/Map";
        public const string Image_MapIcon = "Images/mapIcon";
        public const string Image_MapIconHover = "Images/mapIconHover";
        //Cursor
        public const string Cursor_Pointer = "Images/Pointer";
        public const string Cursor_Intract = "Images/Intract";
        public const string Cursor_Talk = "Images/Talk";
        public const string Cursor_Walk = "Images/Walk";
        public const string Cursor_Exit = "Images/Exit";
        //Animations
        public const string Animations_BakeryLights = "Animations/BakeryLights";
        public const string Animations_BakeryFan = "Animations/BakeryFan";
        //Inventory
        public const string Inventory_Flour = "Inventory/Flour";
        public const string Inventory_Bg = "Inventory/InventoryBg";
        public const string Inventory_IconNorm = "Inventory/IconNormal";
        public const string Inventory_IconHover = "Inventory/IconHover";
    }

    public static class PSBlendState
    {
        public static BlendState Multiply = new BlendState
        {
            ColorSourceBlend = Blend.DestinationColor,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.InverseSourceAlpha
        };
        public static BlendState Screen = new BlendState
        {
            ColorSourceBlend = Blend.InverseDestinationColor,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.InverseSourceAlpha
        };
        public static BlendState Darken = new BlendState
        {
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorBlendFunction = BlendFunction.Min,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.InverseSourceAlpha
        };
        public static BlendState Lighten = new BlendState
        {
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorBlendFunction = BlendFunction.Max,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.InverseSourceAlpha
        };
        public static BlendState LinearDodge = new BlendState
        {
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.InverseSourceAlpha
        };
        public static BlendState LinearBurn = new BlendState
        {
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            ColorBlendFunction = BlendFunction.ReverseSubtract,
            AlphaSourceBlend = Blend.SourceAlpha,
            AlphaDestinationBlend = Blend.InverseSourceAlpha
        };
        public static BlendState ColorDodge = new BlendState
        {
            AlphaSourceBlend = Blend.SourceAlpha,
            ColorSourceBlend = Blend.One,
            ColorBlendFunction = BlendFunction.Add,
            ColorDestinationBlend = Blend.InverseSourceAlpha,
            AlphaDestinationBlend = Blend.InverseSourceAlpha
        };
    } 

    /// <summary>
    /// Custom event argument which includes the index of the player who
    /// triggered the event. This is used by the MenuEntry.Selected event.
    /// </summary>
    public class PlayerIndexEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the index of the player who triggered this event.
        /// </summary>
        PlayerIndex playerIndex;
        public PlayerIndex PlayerIndex
        {
            get { return playerIndex; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlayerIndexEventArgs(PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;
        }
    }
}
