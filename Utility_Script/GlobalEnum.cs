

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
    MAIN_MENU,
    NEW_GAME,
    SAVED_GAME,
    QUIT // Be sure to wait until save function is fully executed before totally closing the game.
}