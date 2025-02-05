namespace Enginus.Core;

public enum Characters
{
    Mizuki,
    Baker,
    Dabchick,
    Secretary,
    Soldier,
    Scarecrow,
    OwlTall,
    OwlFat,
    Mardak,
    Hawaii,
    SuperFrog,
    BadGuy,
    Peter,
    Girl,
    OperaCat,
    Moose,
    Rat,
    Elephant,
    LobbyGuard,
    Qardash,
    Psychotherap,
    Spaghettiman,
    Florist,
    Monkey,
    Vanya,
    Tramp,
    Ghost,
    York,
    Frogs,
    ShadowMan,
    FatWoman
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
    Joined,
    Seprate
}

public enum AnimationType
{
    Linear,
    Random
}

public struct FrameRange
{
    public int StartNumber { get; set; }
    public int EndNumber { get; set; }
}

public enum CursorTexturType
{
    Default,
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

public enum InventoryItems
{
    Bamboo,
    EmptyBucket,
    Flag,
    GrassDried,
    Shell,
    Matchbox,
    Juice,
    PlantRat,
    Watch,
    QardashMap,
    Rope
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
