using UnityEngine;

public abstract class StateBase
{
    protected Player player;
    protected PlayerFSM playerFSM;

    public bool isDone { get; protected set; }

    public StateBase(Player player, PlayerFSM playerFSM)
    {
        this.player = player;
        this.playerFSM = playerFSM;
    }

    public abstract void StateEnter();
    public abstract void StateUpdate();
    public abstract void StateExit();
}
