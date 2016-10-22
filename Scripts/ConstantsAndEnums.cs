//Enums

public enum _Colors
{
    White,
    Black,
    Redpurple,
    Orange,
    Yellow,
    Lime,
    Green,
    Deepblue
}

public enum _ColorType
{
    PlayerShipSpace,
    PlayerShipPlanet,
    BaseMain,
    BaseRim
}

public enum _GameState
{
    MainMenu,
    Lobby,
    MultiNeutral,
    MultiPlayerOnePlanet,
    MultiPlayerTwoPlanet,
    SingleNeutral,
    SinglePlanet

}

//Constants

public class _Scenes
{
    public const string sceneBattle = "sceneBattle";
    public const string sceneBattleSpace = "sceneBattleSpace";
    public const string sceneBattlePlanet = "sceneBattlePlanet";
    public const string sceneMainMenu = "sceneMainMenu";
    public const string sceneLobby = "sceneLobby";
}

public class _Tags
{

    public const string mainCamera = "MainCamera";
    public const string playerOne = "PlayerOne";
    public const string player = "Player";
    public const string playerTwo = "PlayerTwo";
    public const string lobbyManager = "LobbyManager";
}


