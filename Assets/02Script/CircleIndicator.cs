using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CircleIndicator : MonoBehaviour, IIndicator
{
    public float radius = 3f;
    public Material indicatorMaterial;

    MeshFilter mf;
    MeshRenderer mr;

    bool isActive = false;
    void Awake()
    {
        mf = GetComponent<MeshFilter>();
        mr = GetComponent<MeshRenderer>();

        //mf.mesh = CircleMeshGenerator.CreateCircleMesh(radius);
        //mr.material = indicatorMaterial;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        isActive = false;
    }

    public void Show(Vector3 origin, Vector3 direction, float range, float angle = 0)
    {
        gameObject.SetActive(true);
        transform.position = origin;
        mf.mesh = CircleMeshGenerator.CreateCircleMesh(range);
        mr.material = indicatorMaterial;

        transform.localScale = Vector3.one * range;
        //gameObject.SetActive(true);

        //transform.position = origin + Vector3.up * 0.05f;


        //transform.localScale = new Vector3(range * 2f, 1f, range * 2f);


        //mf.mesh = CircleMeshGenerator.CreateCircleMesh(1f);

        //mr.material = indicatorMaterial;
    }


}
