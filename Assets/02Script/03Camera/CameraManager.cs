using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameObject targetObj;
    private Vector3 cameraOffsetDir;
    private float cameraOffset;

    private float zoomSpeed = 1f;

    private void Awake()
    {
        targetObj = GameObject.FindGameObjectWithTag("Player");
        cameraOffsetDir = new Vector3(0f, 15f, -15f);
        transform.rotation = Quaternion.Euler(30f, 0f, 0f);
        cameraOffset = cameraOffsetDir.sqrMagnitude;
    }


    private void CameraMove()
    {
        transform.LookAt(targetObj.transform.position);
        ZoomInOut();
    }

    private void ZoomInOut()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if(scrollInput != 0f)
        {
            cameraOffset += scrollInput * zoomSpeed;
        }        
    }

    private void LateUpdate()
    {
        CameraMove();
    }


    //public GameObject target;          // 캐릭터
    //public Vector3 offsetDirection = new Vector3(0, 5, -5);  // 시야 방향
    //public float zoomSpeed = 5f;
    //public float minDistance = 5f;
    //public float maxDistance = 500f;

    //private float currentDistance;

    //private void Awake()
    //{
    //    target = GameObject.FindGameObjectWithTag("Player");
    //}


    //void Start()
    //{
    //    currentDistance = offsetDirection.magnitude;
    //}

    //void LateUpdate()
    //{
    //    float scroll = Input.GetAxis("Mouse ScrollWheel");

    //    if (scroll != 0f)
    //    {
    //        currentDistance -= scroll * zoomSpeed;
    //        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
    //    }

    //    // 방향을 유지한 채 거리만 바꿈
    //    Vector3 direction = offsetDirection.normalized;
    //    transform.position = target.transform.position + direction * currentDistance;

    //    // 캐릭터 바라보기
    //    transform.LookAt(target.transform.position);
    //}
}
