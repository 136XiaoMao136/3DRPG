using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class taskMangaerUI : MonoBehaviour
{
    public static taskMangaerUI Instance { get; private set; }
    private GameObject uiGameObject;
    private GameObject content;
    public GameObject taskPrefab;
    private bool isShow = false;

    public TaskDetailUI taskDetailUI;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
        }
        Instance = this;
    }
    void Start()
    {

        uiGameObject = transform.Find("UI").gameObject;
        content = transform.Find("UI/BG/Scroll View/Viewport/Content").gameObject;
        Hide();
    }
    public void Hide()
    {
        uiGameObject.SetActive(false);
    }
    public void Show()
    {
        uiGameObject.SetActive(true);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (isShow)
            {
                Hide();
                isShow = false;
            }
            else
            {
                Show();
                isShow = true;
            }
        }
    }
    public void AddTask(GameTaskSO taskSO)
    { 
        taskUI taskUI = taskPrefab.GetComponent<taskUI>();
        taskUI.InitItem(taskSO);
    }
    public void OnTaskClick(GameTaskSO taskSO, taskUI taskUI)
    {
        taskDetailUI.UpdateTaskDetailUI(taskSO, taskUI);
    }
  
}
