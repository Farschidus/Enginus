namespace Enginus.Core;


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

public enum AnimationFileType
{
    Single,
    Seprate
}

public enum AnimationType
{
    Linear,
    Random
}

public enum CursorTexturType
{
    Pointer,
    Intract,
    Talk,
    Walk,
    Exit,
    Texture
}

public enum PointerType
{
    None,
    Left,
    Right,
    Center
}

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

/// <summary>
/// Enum describes the screen transition state.
/// </summary>
public enum ScreenState
{
    TransitionOn,
    Active,
    TransitionOff,
    Hidden
}

/// <summary>
/// Options for AudioManager.CancelFade
/// </summary>
public enum FadeCancelOptions
{
    /// <summary>
    /// Return to pre-fade volume
    /// </summary>
    Source,
    /// <summary>
    /// Snap to fade target volume
    /// </summary>
    Target,
    /// <summary>
    /// Keep current volume
    /// </summary>
    Current
}


/// <summary>
/// Place the labels for the Transitions in this enum.
/// Don't change the first label, NullTransition as FSMSystem class uses it.
/// </summary>
public enum Transition
{
    NullTransition = 0, // Use this transition to represent a non-existing transition in your system
    Init,
    Idle,
    Look,
    Talk,
    Use,
    Take
}

/// <summary>
/// Place the labels for the States in this enum.
/// Don't change the first label, NullTransition as FSMSystem class uses it.
/// </summary>
public enum StateID
{
    NullStateID = 0, // Use this ID to represent a non-existing State in your system
    InitGroup,
    IdleGroup,
    LookGroup,
    TalkGroup,
    UseGroup,
    TakeGroup
}