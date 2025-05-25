using UnityEngine;

public class SkillReadyState : MonoBehaviour
{
    StateType currentReadySkill;

    public void SetSkillReady(SkillType skilltype)
    {

    }


    public StateType ReadyToSkillActive()
    {
        return currentReadySkill;
    }

}

