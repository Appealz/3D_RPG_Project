using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CircleIndicator : MonoBehaviour, IIndicator
{
    public float radius = 3f;
    public Material indicatorMaterial;

    MeshFilter mf;
    MeshRenderer mr;

    
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
    }

    public void Show(Vector3 origin, Vector3 direction, float range, float angle = 0)
    {
        gameObject.SetActive(true);
        transform.position = origin;
        //mf.mesh = CircleMeshGenerator.CreateCircleMesh(range);
        mf.mesh = CircleMeshGenerator.CreateCircleMesh(1f);
        mr.material = indicatorMaterial;

        transform.localScale = Vector3.one * range;
    }


}
