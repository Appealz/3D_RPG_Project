using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FanIndicator : MonoBehaviour, IIndicator
{
    public float radius = 5f;
    public float angle = 90f;
    public int segments = 30;
    public Material indicatorMaterial;
    MeshFilter mf;
    MeshRenderer mr;

    bool isActive = false;
    private void Awake()
    {
        mf = GetComponent<MeshFilter>();
        mr = GetComponent<MeshRenderer>();
    }
    public void Show(Vector3 origin, Vector3 direction, float range, float angle = 0)
    {        
        gameObject.SetActive(true);
        transform.position = origin;
        mf.mesh = FanMeshGenerator.CreateFanMesh(range, angle > 0 ? angle : this.angle, segments);
        mr.material = indicatorMaterial;

        transform.localScale = Vector3.one * range;
        isActive = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        isActive = false;
    }



    private void LateUpdate()
    {
        if (!isActive) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 dir = (hit.point - transform.position);
            dir.y = 0f; // Y축 회전만 적용

            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Euler(0f, targetRot.eulerAngles.y, 0f);
        }
    }

}
