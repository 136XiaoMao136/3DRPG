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
                // 取消事件注册，避免重复触发
                dialogueUI.OnDialogueEnd -= OnDialogueEnd;
                SceneManager.LoadScene("GameScene1");
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
}
  

