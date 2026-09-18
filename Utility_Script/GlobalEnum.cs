
public enum NPCFollowTarget //Expand your choice here
{
    PLAYER,
    OBJECTS
}
public enum NPCState
{
    IDLE,
    WALK,
    RUN
}

public enum GameState
{
    NEW_GAME,
    MAIN_MENU,
    SHOW_SAVES,
    IN_GAME,
    SETTINGS,
    IN_GAME_MENU,
    QUIT // Be sure to wait until save function is fully executed before totally closing the game.
}