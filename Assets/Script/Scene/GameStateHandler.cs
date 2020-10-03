using System;

public delegate void GameStateEvent(GameStateEnum gameStateEnum);
public sealed class GameStateHandler
{
    public GameStateEvent GameStateEvent;
    public GameStateHandler()
    {
    }
}