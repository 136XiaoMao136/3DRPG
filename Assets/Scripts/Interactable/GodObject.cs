using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GodObject : InteractableObject
{
    public string npcName;
    public string[] contentList;

    protected override void Interact()
    {
        if(TryGetDialogueUI(out var dialogueUI))
        {

            dialogueUI.OnDialogueEnd += OnDialogueEnd;
            dialogueUI.Show(npcName, contentList,OnDialogueEnd);
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
                SceneManager.LoadScene("GameScene1");
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
}
  

