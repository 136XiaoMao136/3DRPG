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
                type = "主线击杀任务"; break;
            case GameTaskType.Mainquestspick:
                type = "主线捡取任务"; break;
            case GameTaskType.sidequests:
                type = "支线任务"; break;
        }

        taskType.text = type;
        taskScript.text = taskSO.scriptText;
        this.gameTaskSO = taskSO;
    }
}
