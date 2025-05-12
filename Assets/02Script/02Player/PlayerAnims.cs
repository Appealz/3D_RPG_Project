using UnityEngine;

public class PlayerAnims : MonoBehaviour
{
    private Animator anims;

    private int isMove;


    private void Awake()
    {
        TryGetComponent<Animator>(out anims);
        isMove = Animator.StringToHash("isMove");
    }

    public void MoveAnims(float newSpeed)
    {
        anims.SetFloat(isMove, newSpeed);
    }
}
