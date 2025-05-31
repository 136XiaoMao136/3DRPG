using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;


public class taskUI : MonoBehaviour
{
    public TextMeshProUGUI taskType;
    public TextMeshProUGUI taskScript;
    public GameTaskSO gameTaskSO;
   public void OnClick()
    {
        taskMangaerUI.Instance.OnTaskClick(gameTaskSO, this);
    }
    public void InitItem(GameTaskSO taskSO)
    {
        string type = "";
        switch (taskSO.type)
        {
            case GameTaskType.Mainquestskill:
                type = "���߻�ɱ����"; break;
            case GameTaskType.Mainquestspick:
                type = "���߼�ȡ����"; break;
            case GameTaskType.sidequests:
                type = "֧������"; break;
        }

        taskType.text = type;
        taskScript.text = taskSO.scriptText;
        this.gameTaskSO = taskSO;
    }
}
