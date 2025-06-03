using UnityEngine;

public class PlayerBar : MonoBehaviour
{
    [SerializeField] private GameObject target;                 // 따라갈 대상
    [SerializeField] private Vector3 offset = new Vector3(0, 2.2f, 0);  // 기본 머리 위 위치
    [SerializeField] private bool scaleWithDistance = true;    // 줌에 따라 크기 보정 여부
    [SerializeField] private float scaleFactor = 0.05f;        // 거리 → 크기 변환 비율

    private Camera mainCam;
    private RectTransform rectTransform;
    private float baseOffsetY; // 원래 offset.y 저장용

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        mainCam = Camera.main;
        baseOffsetY = offset.y; //  초기 offset.y 저장
    }

    private void LateUpdate()
    {
        if (target == null || mainCam == null) return;

        float distance = Vector3.Distance(mainCam.transform.position, target.transform.position);

        //  distance에 따라 y 오프셋만 따로 계산
        float dynamicOffsetY = Mathf.Clamp(baseOffsetY + (distance - 10f) * 0.1f, 2.2f, 3f);
        Vector3 worldPos = target.transform.position + new Vector3(offset.x, dynamicOffsetY, offset.z);

        // 화면 좌표 변환 및 HUD 위치 이동
        Vector3 screenPos = mainCam.WorldToScreenPoint(worldPos);
        rectTransform.position = screenPos;

        //  HUD 스케일 보정 (줌 대응)
        if (scaleWithDistance)
        {
            float scale = Mathf.Clamp(distance * scaleFactor, 1.5f, 1.7f);
            rectTransform.localScale = new Vector3(scale, scale, 1f);
        }
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }
}
