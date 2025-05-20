using UnityEngine;

public class PlayerAnims : MonoBehaviour
{
    private Animator anims;

    private int isMove;
    private int isRun;
    private int isAttack;
    private int isQSkill;
    private int isWSkill;
    private int isESkill;
    private int isRSkill;

    private void Awake()
    {
        TryGetComponent<Animator>(out anims);
        isMove = Animator.StringToHash("IsMove");
        isRun = Animator.StringToHash("isTargetMove");
        isAttack = Animator.StringToHash("isAttack");
        isQSkill = Animator.StringToHash("isQ_Skill");
        isWSkill = Animator.StringToHash("isW_Skill");
        isESkill = Animator.StringToHash("isE_Skill");
        isRSkill = Animator.StringToHash("isR_Skill");
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

    public void QSkillAnims()
    {
        anims.SetTrigger(isQSkill);
    }
    public void WSkillAnims()
    {
        anims.SetTrigger(isWSkill);
    }
    public void ESkillAnims()
    {
        anims.SetTrigger(isESkill);
    }
    public void RSkillAnims()
    {
        anims.SetTrigger(isRSkill);
    }

}
