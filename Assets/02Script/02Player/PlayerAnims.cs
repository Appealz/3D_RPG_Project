using UnityEngine;

public class PlayerAnims : MonoBehaviour
{
    private Animator anims;

    private int isMove;
    private int isRun;


    private void Awake()
    {
        TryGetComponent<Animator>(out anims);
        isMove = Animator.StringToHash("isMove");
        isRun = Animator.StringToHash("isTargetMove");
    }

    public void MoveAnims(float newSpeed)
    {
        anims.SetFloat(isMove, newSpeed);
    }

    public void RunAnims(bool onAnims)
    {
        anims.SetBool(isRun, onAnims);
    }
}
