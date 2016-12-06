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
    SinglePlayerLobby,
    MultiplayerLobby,
    MultiNeutral,
    MultiPlayerOnePlanet,
    MultiPlayerTwoPlanet,
    SingleNeutral,
    SinglePlanetAttack,
    SinglePlanetDefend

}

//Constants

public class _Scenes
{
    public const string sceneMainMenu = "sceneMainMenu";
    public const string sceneSinglePlayerLobby = "sceneSinglePlayerLobby";
    public const string sceneMultiplayerLobby = "sceneMultiplayerLobby";
    public const string sceneSinglePlayer = "sceneSinglePlayer";
    public const string sceneMultiplayer = "sceneMultiplayer";
}

public class _Tags
{

    public const string mainCamera = "MainCamera";
    public const string playerOne = "PlayerOne";
    public const string player = "Player";
    public const string playerTwo = "PlayerTwo";
    public const string lobbyManager = "LobbyManager";
    public const string environment = "Environment";
}


