using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI contentText;
    private Button continueButton;

    private List<string> contentList;
    private int contentIndex = 0;

    private GameObject uiGameObject;

    public Action OnDialogueEnd;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        nameText = transform.Find("UI/NameTextBg/NameText")?.GetComponent<TextMeshProUGUI>();
        if (nameText == null)
        {
            Debug.LogError("NameText 组件未找到！");
        }
       
        contentText = transform.Find("UI/ContentText")?.GetComponent<TextMeshProUGUI>();
        if (contentText == null)
        {
            Debug.LogError("ContentText 组件未找到！");
        }

        continueButton = transform.Find("UI/ContinueButton")?.GetComponent<Button>();
        if (continueButton == null)
        {
            Debug.LogError("ContinueButton 组件未找到！");
        }
        else
        {
            continueButton.onClick.AddListener(this.OnContinueButtonClick);
        }

        uiGameObject = transform.Find("UI")?.gameObject;
        if (uiGameObject == null)
        {
            Debug.LogError("UI 游戏对象未找到！");
        }
        Hide();
    }

    public void Show()
    {
        uiGameObject.SetActive(true);
    }

    public void Show(string name, string[] content, Action onDialogueEndCallback = null)
    {
        if (content == null || content.Length == 0)
        {
            Debug.LogError("传入的对话内容为空！");
            return;
        }

        // 保存回调函数
        OnDialogueEnd = onDialogueEndCallback;

        nameText.text = name;
        contentList = new List<string>();
        contentList.AddRange(content);
        contentIndex = 0;
        contentText.text = contentList[0];
        uiGameObject.SetActive(true);
        Debug.Log($"显示新对话，名称: {name}，内容数量: {contentList.Count}");
    }

    public void Hide()
    {
        uiGameObject.SetActive(false);
        Debug.Log("对话 UI 已隐藏");
    }

    private void OnContinueButtonClick()
    {
        Debug.Log($"点击继续按钮，当前索引: {contentIndex}，内容数量: {contentList.Count}");
        contentIndex++;
        if (contentIndex >= contentList.Count)
        {
            Debug.Log("对话内容已显示完，触发结束回调");
            OnDialogueEnd?.Invoke();
            Hide();
            return;
        }

        contentText.text = contentList[contentIndex];
    }
}