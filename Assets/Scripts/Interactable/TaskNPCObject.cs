using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class StringArrayElement
{
    public string[] strings;
}
public class TaskNPCObject : InteractableObject
{
   
    public string npcName;
    public List<GameTaskSO> gameTaskSO;

    public List<StringArrayElement> contentInTaskExecuting;
    public List<StringArrayElement> contentInTaskCompleted;
    public List<StringArrayElement> contentInTaskEnd;
    public int index = 0;

    public void Start()
    {
        List<GameTaskSO> List = TaskManager.Instance.taskDB;
        for (int i = 0; i < List.Count; i++)
        {
            for (int j = 0; j < gameTaskSO.Count; j++)
            {
                if (List[i] == gameTaskSO[j] && List[i].state != gameTaskSO[j].state)
                {
                    gameTaskSO[j].state = List[i].state;
                    return;
                }
                else
                {
                    gameTaskSO[j].state = TaskManager.Instance.state;
                }
            }
        }
    }
    public void End(int slot)
    {
        gameTaskSO[slot].state = GameTaskState.End;
    }
    protected override void Interact()
    {
        if (gameTaskSO == null)
        {
            return;
        }
      
            switch (gameTaskSO[index].state)
            {
                case GameTaskState.Waiting:
                    ShowDialogue(npcName, gameTaskSO[index].diague, OnDialogueEnd);
                    break;
                case GameTaskState.Executing:
                    ShowDialogue(npcName, contentInTaskExecuting[index].strings);
                    break;
                case GameTaskState.Completed:
                    ShowDialogue(npcName, contentInTaskCompleted[index].strings, OnDialogueEnd);
                    break;
                case GameTaskState.End:
                    ShowDialogue(npcName, contentInTaskEnd[index].strings, OnDialogueEnd);
                    break;
                default:
                    break;
            }
    }

    private void ShowDialogue(string name, string[] content, Action onDialogueEnd = null)
    {
        // 检查对话内容数组是否为空
        if (content == null || content.Length == 0)
        {
            return;
        }

        // 检查 DialogueUI 实例是否为空
        if (DialogueUI.Instance != null)
        {
            DialogueUI.Instance.Show(name, content, onDialogueEnd);
        }
        else
        {
            Debug.LogError("DialogueUI 实例为空，无法显示对话");
            return;
        }
    }

    public void OnDialogueEnd()
    {
        if (gameTaskSO == null)
        {
            return;
        }

        switch (gameTaskSO[index].state)
        {
            case GameTaskState.Waiting:
                HandleTaskStart();
                break;
            case GameTaskState.Completed:
                HandleTaskCompletion();
                break;
            case GameTaskState.Executing:
            case GameTaskState.End:
                if (gameTaskSO[index].kind == GameTaskKinds.EndTask)
                {
                    HandleTaskEnd();
                }
                else if (gameTaskSO[index].kind == GameTaskKinds.StartTask ){
                    HandleTaskEnd1();
                }else if(gameTaskSO[index].kind == GameTaskKinds.ContinueTask)
                {
                    HandleTaskEnd2();
                }
                break;
            default:
                break;
        }
    }

    private void HandleTaskEnd2()
    {
        SceneManager.LoadScene("GameScene2");
    }

    private void HandleTaskEnd1()
    {
        index++;
    }

    private void HandleTaskEnd()
    {
        SceneManager.LoadScene("GameEnd1");
    }

    private void HandleTaskStart()
    {
        if (gameTaskSO == null)
        {
            Debug.LogError("gameTaskSO 实例为空，无法开始任务");
            return;
        }
        gameTaskSO[index].Start();
        TaskManager.Instance.AddTask(gameTaskSO[index]);

        GiveRewardAndShowMessage(gameTaskSO[index].startReward, "你接受了一个任务！");
    }

    private void HandleTaskCompletion()
    {
        gameTaskSO[index].End();
        TaskManager.Instance.RemoveTask(gameTaskSO[index]);
        PlayerProperty.Instance.levelUP(gameTaskSO[index].exp);
        GiveRewardAndShowMessage(gameTaskSO[index].endReward, "任务已提交！");
    }

    private void GiveRewardAndShowMessage(ItemSO reward, string message)
    {
        // 给予奖励
        if (InventoryManager.Instance != null && reward != null)
        {
            InventoryManager.Instance.AddItem(reward);
        }

        // 显示提示信息
        if (MessageUI.Instance != null)
        {
            MessageUI.Instance.Show(message);
        }
    }
}