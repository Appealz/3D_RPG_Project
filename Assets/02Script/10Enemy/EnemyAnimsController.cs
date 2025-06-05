using UnityEngine;

public class EnemyAnimsController : MonoBehaviour
{
    Animator anims;

    private int isMoveHash;
    private int isAttackHash;
    private int isRunHash;
    private int isHitHash;
    private int isDieHash;    

    private void Awake()
    {
        anims = GetComponent<Animator>();

        isMoveHash = Animator.StringToHash("IsMove");
        isRunHash = Animator.StringToHash("IsRun");
        isAttackHash = Animator.StringToHash("IsAttack");
        isHitHash = Animator.StringToHash("IsHit");
        isDieHash = Animator.StringToHash("IsDie");
        
    }

    public void PlayMove(bool isMoving)
    {
        anims.SetBool(isMoveHash, isMoving);
    }

    public void PlayRun(bool isRunning)
    {
        anims.SetBool(isRunHash, isRunning);
    }

    public void PlayHit()
    {
        anims.SetTrigger(isHitHash);
    }

    public void PlayDie()
    {
        anims.SetTrigger(isDieHash);
    }
    public void PlayAttack()
    {
        anims.SetTrigger(isAttackHash);
    }
}
