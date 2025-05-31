using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTaskState
{
    Waiting,
    Executing,
    Completed,
    End
}
public enum GameTaskKinds
{
    StartTask,
    ContinueTask,
    pauseTask,
    EndTask
}
public enum GameTaskType
{
    Mainquestskill,
    Mainquestspick,
    sidequests
}
[CreateAssetMenu()]
public class GameTaskSO:ScriptableObject
{
    public GameTaskState state;
    public GameTaskType type;
    public GameTaskKinds kind;
    public  string scriptText;
    public  string detailScriptText;
    public string[] diague;
   
    public ItemSO startReward;
    public ItemSO endReward;
    public int exp;

    public int enemyCountNeed = 1;
    public int pickNeed;
    public int currentEnemyCount = 0;
    public int currentPickCount = 0;

    public void Start()
    {
        currentEnemyCount = 0;
        currentPickCount = 0;
        state = GameTaskState.Executing;
  
        if (type == GameTaskType.Mainquestskill)
        {
            Debug.Log("���� OnEnemyDied �¼�");
            EventCenter.OnEnemyDied += OnEnemyDied;
        }else if (type == GameTaskType.Mainquestspick)
        {
            Debug.Log("���� On �¼�");
            EventCenter.OnInteractableObject += OnInteracterpick;
        }
         
       
    }

    private void OnInteracterpick(InteractableObject @object)
    {
        if (state == GameTaskState.Completed) return;
        currentPickCount++;
        if (currentPickCount >= pickNeed)
        {
            state = GameTaskState.Completed;
            MessageUI.Instance.Show("������ɣ���ǰȥ���ͣ�");
        }

    }

    private void OnEnemyDied(Enemy enemy)
    {
        Debug.Log("OnEnemyDied ���������ã���ǰ����״̬: " + state);
        if (state == GameTaskState.Completed) return;
        currentEnemyCount++;
        if(currentEnemyCount>= enemyCountNeed)
        {
            state = GameTaskState.Completed;
            MessageUI.Instance.Show("������ɣ���ǰȥ���ͣ�");
            
        }
    }


    public void End()
    {
        state = GameTaskState.End;
        Debug.Log("ȡ������ OnEnemyDied �¼�");
        EventCenter.OnEnemyDied -= OnEnemyDied;
    }

}
