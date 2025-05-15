using UnityEngine;

public class PlayerControl : ManagerBase
{
    PlayerState playerState;
    private IInputHandler inputHandler;

    public void CurrentInputHandler(IInputHandler curHandler)
    {
        inputHandler = curHandler;
    }


    public override void CustomUpdate()
    {
        base.CustomUpdate();
        //playerState.Handle_UpdateState();
      
    }


}
