using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] Skill Qskill;
    [SerializeField] ScriptableObject Wskill;
    [SerializeField] ScriptableObject Eskill;
    [SerializeField] ScriptableObject Rskill;

    PlayerController playerController;
    private void Awake()
    {

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        playerController.RegistSkill(KeyCode.Q, Qskill.GetInterface());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
