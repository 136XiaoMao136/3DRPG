using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventCenter : MonoBehaviour
{
    public static event Action<Enemy> OnEnemyDied;
    public  event Action OnDialogueEnd;
    public static event Action<InteractableObject> OnInteractableObject;
    public static void EnemyDied(Enemy enemy)
    {
        OnEnemyDied?.Invoke(enemy);
    }
    public static void InteractableObject(InteractableObject interactableObject)
    {
        OnInteractableObject?.Invoke(interactableObject);
    }
    public void EndDialogue()
    {
        // 触发对话结束事件
        OnDialogueEnd?.Invoke();
    }
}
