using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager2 : DontDestroySingleton<GameManager>
{
    #region _ManagerField_
    private CursorManager cursorManager;
    private IInputHandle curInputHandler;
    private UIManager uiManager;
    private DataManager2 dataManager;
    private Player player;
    private PlayerActionController playerActionController;
    #endregion
    private List<ManagerBase> managerList = new List<ManagerBase>();
    
    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<Player>();
        if(player == null)        
            Debug.Log("player is not ref");
        else
            managerList.Add(player);

        cursorManager = GameObject.FindAnyObjectByType<CursorManager>();
        if (cursorManager == null)
            Debug.Log("cursorManager is not ref");
        else
            managerList.Add(cursorManager);

        playerActionController = GameObject.FindAnyObjectByType<PlayerActionController>();
        if (playerActionController == null)
            Debug.Log("playerActionController is not ref");
        else
            managerList.Add(playerActionController);

        uiManager = GameObject.FindAnyObjectByType<UIManager>();
        if (uiManager == null)
            Debug.Log("uiManager is not ref");
        else
            managerList.Add(uiManager);

        dataManager = GameObject.FindAnyObjectByType<DataManager2>();
        if (dataManager == null)
            Debug.Log("dataManager is not ref");
        else
            managerList.Add(dataManager);

        curInputHandler = GameObject.FindAnyObjectByType<InputManager>();
        if (curInputHandler == null)
            Debug.Log("curInputHandler is not ref");

        uiManager.SetPlayer(player.gameObject);
    }

    private void Start()
    {
        playerActionController?.BindToInputHandler(curInputHandler, player);

        // 데이터 주입(세팅)
        var (Q, W, E, R) = dataManager.GetAllSkillData();
        uiManager.SetSkillData(Q, W, E, R);
        SkillSetting(SkillFactory.CreateSkill(Q), KeyCode.Q);
        SkillSetting(SkillFactory.CreateSkill(W), KeyCode.W);
        SkillSetting(SkillFactory.CreateSkill(E), KeyCode.E);
        SkillSetting(SkillFactory.CreateSkill(R), KeyCode.R);
        player.PlayerDataSetting(dataManager.GetPlayerData());

        for(int i = 0; i < managerList.Count; i++)
        {
            managerList[i]?.StartGame();
        }
    }

    private void Update()
    {
        for (int i = 0; i < managerList.Count; i++)
        {
            managerList[i]?.CustomUpdate();
        }
    }

    public void StopGame()
    {
        for (int i = 0; i < managerList.Count; i++)
        {
            managerList[i]?.StopGame();
        }
    }

    /// <summary>
    /// inputHandler, player, skill객체 있어야 가능.
    /// </summary>
    /// <param name="newSkill"></param>
    /// <param name="newKey"></param>
    private void SkillSetting(ISkill newSkill, KeyCode newKey)
    {        
        player.RegistSkill(newSkill);
        curInputHandler.BindKeyToSkill(newKey, newSkill.myType);
    }
}
