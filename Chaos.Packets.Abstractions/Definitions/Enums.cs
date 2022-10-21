namespace Chaos.Packets.Abstractions.Definitions;

public enum ClientOpCode : byte
{
    ConnectionInfoRequest = 0,
    CreateCharRequest = 2,
    Login = 3,
    CreateCharFinalize = 4,
    RequestMapData = 5,
    ClientWalk = 6,
    Pickup = 7,
    ItemDrop = 8,
    ExitRequest = 11,
    DisplayObjectRequest = 12,
    Ignore = 13,
    PublicMessage = 14,
    SpellUse = 15,
    ClientRedirected = 16,
    Turn = 17,
    SpaceBar = 19,
    RequestWorldList = 24,
    Whisper = 25,
    UserOptionToggle = 27,
    ItemUse = 28,
    Emote = 29,
    SetNotepad = 35,
    GoldDrop = 36,
    PasswordChange = 38,
    ItemDroppedOnCreature = 41,
    GoldDroppedOnCreature = 42,
    RequestProfile = 45,
    GroupRequest = 46,
    ToggleGroup = 47,
    SwapSlot = 48,
    RequestRefresh = 56,
    PursuitRequest = 57,
    DialogResponse = 58,
    BoardRequest = 59,
    SkillUse = 62,
    WorldMapClick = 63,
    Click = 67,
    Unequip = 68,
    HeartBeat = 69,
    RaiseStat = 71,
    Exchange = 74,
    NoticeRequest = 75,
    BeginChant = 77,
    Chant = 78,
    Profile = 79,
    ServerTableRequest = 87,
    SequenceChange = 98,
    HomepageRequest = 104,
    SynchronizeTicks = 117,
    SocialStatus = 121,
    MetafileRequest = 123
}

public enum ServerOpCode : byte
{
    ConnectionInfo = 0,
    LoginMessage = 2,
    Redirect = 3,
    Location = 4,
    UserId = 5,
    DisplayVisibleObject = 7,
    Attributes = 8,
    ServerMessage = 10,
    ConfirmClientWalk = 11,
    CreatureWalk = 12,
    PublicMessage = 13,
    RemoveObject = 14,
    AddItemToPane = 15,
    RemoveItemFromPane = 16,
    CreatureTurn = 17,
    HealthBar = 19,
    MapInfo = 21,
    AddSpellToPane = 23,
    RemoveSpellFromPane = 24,
    Sound = 25,
    BodyAnimation = 26,
    Notepad = 27,
    MapChangeComplete = 31,
    LightLevel = 32,
    RefreshResponse = 34,
    Animation = 41,
    AddSkillToPane = 44,
    RemoveSkillFromPane = 45,
    WorldMap = 46,
    Menu = 47,
    Dialog = 48,
    BulletinBoard = 49,
    Door = 50,
    DisplayAisling = 51,
    Profile = 52,
    WorldList = 54,
    Equipment = 55,
    Unequip = 56,
    SelfProfile = 57,
    Effect = 58,
    HeartBeatResponse = 59,
    MapData = 60,
    Cooldown = 63,
    Exchange = 66,
    CancelCasting = 72,
    ProfileRequest = 73,
    ForceClientPacket = 75,
    ConfirmExit = 76,
    ServerTable = 86,
    MapLoadComplete = 88,
    LoginNotification = 96,
    GroupRequest = 99,
    LoginControls = 102,
    MapChangePending = 103,
    SynchronizeTicks = 104,
    Metafile = 111,
    AcceptConnection = 126
}