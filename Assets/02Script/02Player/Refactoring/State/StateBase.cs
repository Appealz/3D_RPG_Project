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
    public virtual void Cancel() { isDone = true; }
    public virtual void Finish() { isDone = true; playerFSM.ChangeState(StateType.Idle, force: true);
    }
    public virtual void InjectContext(IStateContext context) { }
}
