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
            Debug.LogError("NameText ���δ�ҵ���");
        }
       
        contentText = transform.Find("UI/ContentText")?.GetComponent<TextMeshProUGUI>();
        if (contentText == null)
        {
            Debug.LogError("ContentText ���δ�ҵ���");
        }

        continueButton = transform.Find("UI/ContinueButton")?.GetComponent<Button>();
        if (continueButton == null)
        {
            Debug.LogError("ContinueButton ���δ�ҵ���");
        }
        else
        {
            continueButton.onClick.AddListener(this.OnContinueButtonClick);
        }

        uiGameObject = transform.Find("UI")?.gameObject;
        if (uiGameObject == null)
        {
            Debug.LogError("UI ��Ϸ����δ�ҵ���");
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
            Debug.LogError("����ĶԻ�����Ϊ�գ�");
            return;
        }

        // ����ص�����
        OnDialogueEnd = onDialogueEndCallback;

        nameText.text = name;
        contentList = new List<string>();
        contentList.AddRange(content);
        contentIndex = 0;
        contentText.text = contentList[0];
        uiGameObject.SetActive(true);
        Debug.Log($"��ʾ�¶Ի�������: {name}����������: {contentList.Count}");
    }

    public void Hide()
    {
        uiGameObject.SetActive(false);
        Debug.Log("�Ի� UI ������");
    }

    private void OnContinueButtonClick()
    {
        Debug.Log($"���������ť����ǰ����: {contentIndex}����������: {contentList.Count}");
        contentIndex++;
        if (contentIndex >= contentList.Count)
        {
            Debug.Log("�Ի���������ʾ�꣬���������ص�");
            OnDialogueEnd?.Invoke();
            Hide();
            return;
        }

        contentText.text = contentList[contentIndex];
    }
}