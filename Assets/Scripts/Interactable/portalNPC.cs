using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class portalNPC : InteractableObject
{
    public string npcName;
    public string[] contentList;
    public string[] contentList1;
    public GameTaskSO requiredTaskId; // 完成对话所需的任务 ID

    protected override void Interact()
    {
        if (IsTaskCompleted())
        {
            if (TryGetDialogueUI(out var dialogueUI))
            {
                // 注册对话结束事件
                dialogueUI.OnDialogueEnd += OnDialogueEnd;

                // 显示对话 UI
                dialogueUI.Show(npcName, contentList, OnDialogueEnd);
            }
        }
        else
        {
            DialogueUI.Instance.Show(npcName, contentList1);
        }
    }

    private void OnDialogueEnd()
    {
        try
        {
            if (TryGetDialogueUI(out var dialogueUI))
            {
                // 取消事件注册，避免重复触发
                dialogueUI.OnDialogueEnd -= OnDialogueEnd;
            }

            if (TryGetPortalUI(out var portalUI))
            {
                // 对话结束后显示传送门 UI
                portalUI.Show();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"OnDialogueEnd 方法中出现异常: {ex.Message}");
        }
    }

    private bool TryGetDialogueUI(out DialogueUI dialogueUI)
    {
        dialogueUI = DialogueUI.Instance;
        if (dialogueUI == null)
        {
            Debug.LogError("DialogueUI 实例未正确初始化");
            return false;
        }
        return true;
    }

    private bool TryGetPortalUI(out PortalUI portalUI)
    {
        portalUI = PortalUI.Instance;
        if (portalUI == null)
        {
            Debug.LogError("PortalUI 实例未正确初始化");
            return false;
        }
        return true;
    }

    private bool IsTaskCompleted()
    {
        if (requiredTaskId != null && requiredTaskId.state == GameTaskState.End)
        {
            return true;
        }
        return false;
    }
}