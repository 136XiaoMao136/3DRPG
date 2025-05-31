using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance { get; private set; }

    [Header("UI 组件")]
    public Button backButton;
    public Slider masterVolumeSlider;
    public GameObject masterVolumeValueText;
    public Slider musicVolumeSlider;
    public GameObject musicVolumeValueText;
    public Slider effectsVolumeSlider;
    public GameObject effectsVolumeValueText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // 检查必要的 UI 组件是否赋值
        if (backButton == null || masterVolumeSlider == null || masterVolumeValueText == null ||
            musicVolumeSlider == null || musicVolumeValueText == null ||
            effectsVolumeSlider == null || effectsVolumeValueText == null)
        {
            Debug.LogError("SettingManager：部分 UI 组件未赋值，请检查 Inspector 面板。");
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        backButton.onClick.AddListener(SaveVolumeSettingsOnBack);
        StartCoroutine(LoadAndApplyVolumeSettings());
    }

    private IEnumerator LoadAndApplyVolumeSettings()
    {
        LoadAndSetVolume();
        yield return new WaitForSeconds(0.1f);
    }

    private void LoadAndSetVolume()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager 未初始化，无法加载音量设置。");
            return;
        }

        VolumeSettings volumeSettings = SaveManager.Instance.LoadVolumeSettings();
        if (volumeSettings != null)
        {
            masterVolumeSlider.value = volumeSettings.masterVolume;
            musicVolumeSlider.value = volumeSettings.musicVolume;
            effectsVolumeSlider.value = volumeSettings.sfxVolume;
        }
        else
        {
            Debug.LogError("加载的音量设置为空。");
        }
    }

    private void SaveVolumeSettingsOnBack()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager 未初始化，无法保存音量设置。");
            return;
        }

        VolumeSettings settings = new VolumeSettings
        {
            masterVolume = masterVolumeSlider.value,
            musicVolume = musicVolumeSlider.value,
            sfxVolume = effectsVolumeSlider.value
        };
        SaveManager.Instance.SaveVolumeSettings(settings);
    }

    private void Update()
    {
        UpdateVolumeText(masterVolumeSlider, masterVolumeValueText);
        UpdateVolumeText(musicVolumeSlider, musicVolumeValueText);
        UpdateVolumeText(effectsVolumeSlider, effectsVolumeValueText);
    }

    private void UpdateVolumeText(Slider slider, GameObject textObject)
    {
        if (textObject != null)
        {
            TextMeshProUGUI textComponent = textObject.GetComponent<TextMeshProUGUI>();
            if (textComponent != null && slider != null)
            {
                textComponent.text = slider.value.ToString("F2"); // 保留两位小数
            }
        }
    }

    private void OnDestroy()
    {
        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
        }
    }
}