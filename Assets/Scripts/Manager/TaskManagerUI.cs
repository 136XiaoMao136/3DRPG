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
        for (int i = 0; i < taskDB.Count; i++)//�ж��Ƿ������Ƶ�����
        {
            if (taskDB[i] == gameTaskSO)
            {
                isFounnd = true;
                break;
            }
        }
        if (isFounnd)
        {
            Debug.Log("�������Ѿ������������");
            return;//�����Ѿ�����
        }
        else
        {//���񲻴��ڣ�������������������
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
        int index = taskDB.IndexOf(gameTaskSO);//���������֮��ӹ���������ȥ������
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
