using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TaskDetailUI : MonoBehaviour
{
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI scriptText;
    public TextMeshProUGUI detailScriptText;
    private GameTaskSO taskSO;
    private taskUI taskUI;
    private void Start()
    {
        this.gameObject.SetActive(false);
    }
    public void UpdateTaskDetailUI(GameTaskSO _taskSO, taskUI _taskUI)
    {
        this.taskSO = _taskSO;
        this.taskUI = _taskUI;
        this.gameObject.SetActive(true);

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
        typeText.text = type;
        scriptText.text = taskSO.scriptText;
        detailScriptText.text = taskSO.detailScriptText;
    }

    public void OnYesButtonClick()
    {
        this.gameObject.SetActive(false);
    }
}
