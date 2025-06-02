using UnityEngine;

public class SpellIndicator : MonoBehaviour, IIndicator
{
    bool isActive = false;

    public void Hide()
    {
        gameObject.SetActive(false);
        isActive = false;
    }

    public void Show(Vector3 origin, Vector3 direction, float range, float angle = 0)
    {
        isActive = true;
        gameObject.SetActive(true);
        transform.localScale = Vector3.one * 0.5f;
    }


    private void LateUpdate()
    {
        if (!isActive) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 newPos = new Vector3(hit.point.x, hit.point.y+0.02f, hit.point.z);
            transform.position = newPos;            
        }
    }

}
