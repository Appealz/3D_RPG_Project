using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameObject targetObj;
    [SerializeField]
    private Vector3 cameraOffsetDir;
    private float cameraOffset;
    //private float minOffset;
    [SerializeField]
    private Vector3 cameraAngle;


    [SerializeField]
    private float zoomSpeed;
    private float scrollInput;

    private void Awake()
    {
        targetObj = GameObject.FindGameObjectWithTag("Player");
        InitSetting();
    }

    private void InitSetting()
    {
        zoomSpeed = 2f;
        cameraOffset = 10f;
        cameraOffsetDir = new Vector3(0f, cameraOffset, -cameraOffset * 0.8f);
        transform.position = targetObj.transform.position + cameraOffsetDir;
        cameraAngle = new Vector3(40f, 0f, 0f);
        
    }

    private void CameraMove()
    {
        transform.rotation = Quaternion.Euler(cameraAngle);
        transform.position = targetObj.transform.position + cameraOffsetDir;        
    }

    private void ZoomInOut()
    {
        cameraOffset -= scrollInput * zoomSpeed;
        cameraOffset = Mathf.Clamp(cameraOffset, 5f, 15f);
        cameraOffsetDir = new Vector3(0f, cameraOffset, -cameraOffset * 0.8f);
    }

    private void LateUpdate()
    {
        CameraMove();
        scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            ZoomInOut();
        }
    }
}
