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
        // ���Ի����������Ƿ�Ϊ��
        if (content == null || content.Length == 0)
        {
            return;
        }

        // ��� DialogueUI ʵ���Ƿ�Ϊ��
        if (DialogueUI.Instance != null)
        {
            DialogueUI.Instance.Show(name, content, onDialogueEnd);
        }
        else
        {
            Debug.LogError("DialogueUI ʵ��Ϊ�գ��޷���ʾ�Ի�");
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
            Debug.LogError("gameTaskSO ʵ��Ϊ�գ��޷���ʼ����");
            return;
        }
        gameTaskSO[index].Start();
        TaskManager.Instance.AddTask(gameTaskSO[index]);

        GiveRewardAndShowMessage(gameTaskSO[index].startReward, "�������һ������");
    }

    private void HandleTaskCompletion()
    {
        gameTaskSO[index].End();
        TaskManager.Instance.RemoveTask(gameTaskSO[index]);
        PlayerProperty.Instance.levelUP(gameTaskSO[index].exp);
        GiveRewardAndShowMessage(gameTaskSO[index].endReward, "�������ύ��");
    }

    private void GiveRewardAndShowMessage(ItemSO reward, string message)
    {
        // ���轱��
        if (InventoryManager.Instance != null && reward != null)
        {
            InventoryManager.Instance.AddItem(reward);
        }

        // ��ʾ��ʾ��Ϣ
        if (MessageUI.Instance != null)
        {
            MessageUI.Instance.Show(message);
        }
    }
}