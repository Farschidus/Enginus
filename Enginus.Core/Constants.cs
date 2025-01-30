namespace Enginus.Core;

public class Constants
{
    //TODO: Will be static instead of const in the future
    //game constants 
    public const int SCREEN_WIDTH = 1920;  //2560 1920;
    public const int SCREEN_HEIGHT = 1080; //1440 1080;
    public const bool FULL_SCREEN = false;
    public const float ANIMATION_FPS = 20f;
    public const float PHYSIC_FPS = 60f;

    //Animation loop constants
    public const float FAN_SPEED = 0.070f;


    //Debug
    public static bool DisplayCollisionBoxes = false;
    public static bool AnimationDebugMessages = false;
    public static bool EntityDebugMessages = false;
    public static bool TraceEnabled = false;

    //-------------------------------------------------------------//
    //--------------------- `Asset Constants` ---------------------//
    //-------------------------------------------------------------//
    // preload assets for starting the game, in this case, the main menu
    public const string ASSET_BG_MESSAGE_BOX = "Menu/Bgs/MessageboxBg";
    public const string ASSET_BG_MAIN_MENU = "Menu/Bgs/MainMenuBg";

    public const string GAME_ASSETS_FILE = "GameAssets";
    public const string PUZZLES_ASSETS_FILE = "PuzzlePopups";

    //Interacives
    public const string Interacive_SwitchOff = "Images/SwitchOff";
    public const string Interacive_SwitchOn = "Images/SwitchOn";

    //Obstacles
    public const string Forground_BakerCasheer = "Forgrounds/BakerCasheer";

    //Images
    public const string Image_ExitObjects = "Images/placeHolder";
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
