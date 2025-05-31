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
    public GameTaskSO requiredTaskId; // ��ɶԻ���������� ID

    protected override void Interact()
    {
        if (IsTaskCompleted())
        {
            if (TryGetDialogueUI(out var dialogueUI))
            {
                // ע��Ի������¼�
                dialogueUI.OnDialogueEnd += OnDialogueEnd;

                // ��ʾ�Ի� UI
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
                // ȡ���¼�ע�ᣬ�����ظ�����
                dialogueUI.OnDialogueEnd -= OnDialogueEnd;
            }

            if (TryGetPortalUI(out var portalUI))
            {
                // �Ի���������ʾ������ UI
                portalUI.Show();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"OnDialogueEnd �����г����쳣: {ex.Message}");
        }
    }

    private bool TryGetDialogueUI(out DialogueUI dialogueUI)
    {
        dialogueUI = DialogueUI.Instance;
        if (dialogueUI == null)
        {
            Debug.LogError("DialogueUI ʵ��δ��ȷ��ʼ��");
            return false;
        }
        return true;
    }

    private bool TryGetPortalUI(out PortalUI portalUI)
    {
        portalUI = PortalUI.Instance;
        if (portalUI == null)
        {
            Debug.LogError("PortalUI ʵ��δ��ȷ��ʼ��");
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