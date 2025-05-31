using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadSlot : MonoBehaviour
{
    [Header("UI 组件")]
    [SerializeField] private Button loadButton;
    [SerializeField] private TextMeshProUGUI slotNameText;

    public int slotNumber;

    private void Awake()
    {
        if (loadButton == null)
        {
            loadButton = GetComponent<Button>();
            if (loadButton == null)
            {
                Debug.LogError($"LoadSlot（槽位 {slotNumber}）：未找到按钮组件，请检查挂载的 GameObject 是否包含 Button 组件。");
                enabled = false;
                return;
            }
        }

        if (slotNameText == null)
        {
            Transform textTransform = transform.Find("Text (TMP)");
            if (textTransform != null)
            {
                slotNameText = textTransform.GetComponent<TextMeshProUGUI>();
            }
            if (slotNameText == null)
            {
                Debug.LogError($"LoadSlot（槽位 {slotNumber}）：未找到 Text (TMP) 组件，请检查子物体是否存在该组件。");
                enabled = false;
                return;
            }
        }
    }

    private void Start()
    {
        loadButton.onClick.AddListener(OnLoadButtonClicked);
    }

    private void Update()
    {
        if (SaveManager.Instance == null)
        {
            slotNameText.text = "初始化中...";
            return;
        }

        if (SaveManager.Instance.IsSlotEmpty(slotNumber))
        {
            slotNameText.text = "空存档";
        }
        else
        {
            slotNameText.text = PlayerPrefs.GetString($"Slot{slotNumber}_Name", $"存档 {slotNumber}");
        }
    }

    public void OnLoadButtonClicked()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("无法加载：存档管理器未初始化");
            return;
        }

        if (SaveManager.Instance.IsSlotEmpty(slotNumber))
        {
            Debug.LogWarning($"槽位 {slotNumber} 为空，无法加载");
            return;
        }

        LoadGameDataAndScene();
    }

    private void LoadGameDataAndScene()
    {
        var savedData = SaveManager.Instance.LoadData(slotNumber);
        if (savedData == null)
        {
            string filePath = SaveManager.Instance.GetSaveFilePath(slotNumber, SaveManager.Instance.useJsonSave ? "json" : "bin");
            if (!File.Exists(filePath))
            {
                Debug.LogError($"槽位 {slotNumber} 的存档文件不存在，加载失败");
            }
            else
            {
                try
                {
                    if (SaveManager.Instance.useJsonSave)
                    {
                        string json = File.ReadAllText(filePath);
                        savedData = JsonUtility.FromJson<AllGameData>(json);
                        if (savedData == null)
                        {
                            Debug.LogError($"槽位 {slotNumber} JSON 数据解析失败，加载失败。可能原因：文件格式错误、数据结构不匹配。");
                        }
                    }
                    else
                    {
                        using (var stream = new FileStream(filePath, FileMode.Open))
                        {
                            savedData = (AllGameData)new BinaryFormatter().Deserialize(stream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"槽位 {slotNumber} 数据解析失败，加载失败。可能原因：文件格式错误、数据结构不匹配。详细错误信息：{ex.Message}");
                }
            }

            if (savedData == null)
            {
                return;
            }
        }

        if (TaskManager.Instance != null && savedData.taskStatus != null)
        {
            foreach (var gameTask in savedData.taskStatus)
            {
                TaskManager.Instance.AddTask(gameTask);
            }
        }
        else if (TaskManager.Instance == null)
        {
            Debug.LogWarning("TaskManager 未初始化，无法加载任务状态");
        }

        if (!string.IsNullOrEmpty(savedData.currentScene))
        {
            if (SceneUtility.GetBuildIndexByScenePath(savedData.currentScene) == -1)
            {
                Debug.LogError($"槽位 {slotNumber} 的场景 {savedData.currentScene} 未添加到构建设置中，加载失败");
            }
            else
            {
                SceneManager.LoadScene(savedData.currentScene);
            }
        }
        else
        {
            Debug.LogError($"槽位 {slotNumber} 场景名称为空，加载失败");
        }
        SaveManager.Instance.LoadGame(slotNumber);
        SaveManager.Instance.DeselectButton();
        
    }

    private void OnDestroy()
    {
        if (loadButton != null)
        {
            loadButton.onClick.RemoveAllListeners();
        }
    }
}