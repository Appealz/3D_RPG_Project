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

    // 6. 스킬 준비 상태인지
    bool IsSkillReady(out SkillType skillType);

    // 7-A. Q 스킬 (타겟 지정)
    bool TryGetSkillTarget(out Transform target);

    // 7-B. W 스킬 (방향 정보, 마우스 위치)
    bool TryGetSkillDirection(out Vector3 direction);

    // 7-C. R 스킬 (타겟 좌표)
    bool TryGetSkillPosition(out Vector3 position);

    // 8. 스킬/공격 모드 취소 (Esc 또는 우클릭 등)
    bool IsCancelInput();

    // 9. 정지(S 키) 입력
    bool IsStopRequested();
}
