using UnityEngine;

public interface IInputHandle 
{
    // 1. 우클릭으로 이동할 좌표 (Ground hit)
    bool TryGetRightClickPosition(out Vector3 position);

    // 2. 우클릭으로 적 타겟 선택
    bool TryGetRightClickTarget(out Transform target);

    // 3. 공격 상태 진입 (예: A 키 입력)
    bool IsAttackKeyDown();

    // 4. 공격 상태 중, 좌클릭으로 타겟 선택
    bool TryGetAttackTargetClick(out Transform target);

    // 4-1. 공격 상태 중, 좌클릭으로 땅 클릭
    bool TryGetAttackGroundClick(out Vector3 groundPos);

    // 5. 스킬 키 바인딩 처리 (QWER 키 입력 확인)
    bool TryGetSkillKeyInput(out SkillType skillType);

    // 6. 스킬 사용시 타겟 및 좌표 정보
    bool TryGetSkillInput(out Transform target, out Vector3 position);
    // 7. 스킬/공격 모드 취소 (Esc 또는 우클릭 등)
    bool IsCancelInput();

    // 8. 정지(S 키) 입력
    bool IsStopRequested();

    // 9. 스킬 바인딩
    void BindKeyToSkill(KeyCode key, SkillType skillType);
}

