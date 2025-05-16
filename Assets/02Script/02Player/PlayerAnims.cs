using UnityEngine;

public class PlayerAnims : MonoBehaviour
{
    private Animator anims;

    private int isMove;
    private int isRun;
    private int isAttack;


    private void Awake()
    {
        TryGetComponent<Animator>(out anims);
        isMove = Animator.StringToHash("IsMove");
        isRun = Animator.StringToHash("isTargetMove");
        isAttack = Animator.StringToHash("isAttack");
    }

    public void MoveAnims(bool onAnims)
    {
        anims.SetBool(isMove, onAnims);
    }

    public void RunAnims(bool onAnims)
    {
        anims.SetBool(isRun, onAnims);
    }

    public void AttackAnims()
    {
        anims.SetTrigger(isAttack);
    }
}
