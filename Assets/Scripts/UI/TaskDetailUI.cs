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
                type = "���߻�ɱ����"; break;
            case GameTaskType.Mainquestspick:
                type = "���߼�ȡ����"; break;
            case GameTaskType.sidequests:
                type = "֧������"; break;
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
