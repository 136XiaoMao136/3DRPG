using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance { get; private set; }

    public List<GameTaskSO> taskDB;
    public GameObject TaskUIPrefab;
    public Transform taskUIRoot;
    public  GameTaskState state;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(taskUIRoot);
    }
    public void AddTask(GameTaskSO gameTaskSO)
    {
        bool isFounnd = false;
        for (int i = 0; i < taskDB.Count; i++)//判断是否有相似的任务
        {
            if (taskDB[i] == gameTaskSO)
            {
                isFounnd = true;
                break;
            }
        }
        if (isFounnd)
        {
            Debug.Log("该任务已经在任务管理当中");
            return;//任务已经存在
        }
        else
        {//任务不存在，向管理器里面添加任务
            taskDB.Add(gameTaskSO);
            state = gameTaskSO.state;
            GameObject taskUIGO = Instantiate(TaskUIPrefab, taskUIRoot);
            taskUI taskUI = taskUIGO.GetComponent<taskUI>();
            taskUI.InitItem(gameTaskSO);
            taskMangaerUI.Instance.AddTask(gameTaskSO);
        }
    }
    public void RemoveTask(GameTaskSO gameTaskSO)
    {
        int index = taskDB.IndexOf(gameTaskSO);//当任务完成之后从管理器里面去掉任务
        taskDB.RemoveAt(index);
        taskUI[] alltaskUIs = taskUIRoot.GetComponentsInChildren<taskUI>();
        foreach (taskUI taskUI in alltaskUIs)
        {
            if (taskUI.gameTaskSO == gameTaskSO)
            {
                Destroy(taskUI.gameObject);
                break;
            }
        }
    }
    public void Update()
    {

    }
}
