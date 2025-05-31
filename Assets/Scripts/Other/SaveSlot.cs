using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class SaveSlot : MonoBehaviour
{
    [Header("UI 组件")]
    public  Button saveButton;
    public  TextMeshProUGUI slotNameText;
    public GameObject overrideAlert;
    public Button yesButton;
    public  Button noButton;

    [Header("存档配置")]
    public int slotNumber;

    private void Awake()
    {
        // 检查并确保所有必要的 UI 组件已正确赋值
        if (saveButton == null || slotNameText == null || overrideAlert == null || yesButton == null || noButton == null)
        {
            Debug.LogError($"SaveSlot {slotNumber}：UI 组件未正确赋值，请检查 Inspector 面板");
            enabled = false;
            return;
        }

        // 绑定按钮点击事件
        saveButton.onClick.AddListener(OnSaveButtonClicked);
        yesButton.onClick.AddListener(OnConfirmSaveClicked);
        noButton.onClick.AddListener(OnCancelSaveClicked);
    }

    private void Start()
    {
        UpdateSlotDisplayName();
    }

    public void OnSaveButtonClicked()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("存档管理器未初始化，无法操作");
            return;
        }

        if (SaveManager.Instance.IsSlotEmpty(slotNumber))
        {
            PerformSaveGame();
        }
        else
        {
            ShowOverrideConfirmation();
        }
    }

    public  void ShowOverrideConfirmation()
    {
        overrideAlert.SetActive(true);
    }

    public void HideOverrideConfirmation()
    {
        overrideAlert.SetActive(false);
    }

    public void OnConfirmSaveClicked()
    {
        Debug.Log($"JSON 存档路径：{SaveManager.Instance.jsonPath}");
        HideOverrideConfirmation();
        PerformSaveGame();
    }

    public  void OnCancelSaveClicked()
    {
        HideOverrideConfirmation();
    }

    private async void PerformSaveGame()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("存档管理器未初始化，无法保存");
            return;
        }

        try
        {
            await SaveManager.Instance.SaveGameAsync(slotNumber);
            UpdateSaveSlotName();
            SaveManager.Instance.DeselectButton();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"存档保存失败（槽位 {slotNumber}）：{ex.Message}");
        }
    }

    private void UpdateSaveSlotName()
    {
        string saveTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        string saveName = $"存档 {slotNumber} - {saveTime}";
        PlayerPrefs.SetString($"Slot{slotNumber}_Name", saveName);
        slotNameText.text = saveName;
    }

    private void UpdateSlotDisplayName()
    {
        if (SaveManager.Instance == null)
        {
            slotNameText.text = "初始化中...";
            return;
        }

        slotNameText.text = SaveManager.Instance.IsSlotEmpty(slotNumber)
           ? "空存档"
            : PlayerPrefs.GetString($"Slot{slotNumber}_Name", "未命名存档");
    }

    private void OnDestroy()
    {
        // 移除按钮点击事件监听，防止内存泄漏
        saveButton.onClick.RemoveAllListeners();
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
    }
}