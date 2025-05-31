using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    // 存档文件路径
    private const string SaveFilePathFormat = "{0}{1}{2}.{3}";
    private string binaryPath;
    public string jsonPath;

    // 数据版本号
    private const int DataVersion = 1;

    [Header("存档配置")]
    [SerializeField] public bool useJsonSave = true; // 是否使用JSON保存（可在Inspector配置）

    // 用于线程同步的锁对象
    private readonly object _fileLock = new object();

    private void Awake()
    {
        // 单例初始化（线程安全）
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDirectories();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 初始化存档路径
        binaryPath = Path.Combine(Application.persistentDataPath, "Saves", "Binary");
        jsonPath = Path.Combine(Application.persistentDataPath, "Saves", "Json");
        CreateDirectoryIfNotExists(binaryPath);
        CreateDirectoryIfNotExists(jsonPath);
    }

    private void InitializeDirectories()
    {
        string saveRoot = Path.Combine(Application.persistentDataPath, "Saves");
        CreateDirectoryIfNotExists(saveRoot);
    }

    private void CreateDirectoryIfNotExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    /// <summary>
    /// 检查存档槽是否为空（通过文件存在性判断）
    /// </summary>
    public bool IsSlotEmpty(int slotNumber)
    {
        string jsonPath = GetSaveFilePath(slotNumber, "json");
        string binaryPath = GetSaveFilePath(slotNumber, "bin");
        return !File.Exists(jsonPath) && !File.Exists(binaryPath);
    }

    #region 保存系统
    /// <summary>
    /// 异步保存游戏数据
    /// </summary>
    public async Task SaveGameAsync(int slotNumber)
    {
        try
        {
            var data = GetAllGameData();
            if (data == null)
            {
                Debug.LogError($"槽位 {slotNumber} 保存失败：游戏数据获取失败");
                return;
            }
            await Task.Run(() =>
            {
                lock (_fileLock)
                {
                    SaveData(data, slotNumber);
                }
            });
        }
        catch (Exception ex)
        {
            Debug.LogError($"槽位 {slotNumber} 保存失败：{ex.Message}");
        }
    }

    public AllGameData GetAllGameData()
    {
        if (PlayerProperty.Instance == null ||
            InventoryManager.Instance == null ||
            TaskManager.Instance == null)
        {
            Debug.LogError("核心管理器未初始化，无法保存数据");
            return null;
        }

        return new AllGameData
        {
            version = DataVersion,
            playerData = new PlayerData(
                GetPlayerStats(),
                GetPlayerPositionAndRotation(),
                new List<ItemSO>(InventoryManager.Instance.itemList)
            ),
            currentScene = SceneManager.GetActiveScene().name,
            taskStatus = TaskManager.Instance.taskDB
        };
    }

    private float[] GetPlayerStats()
    {
        return new float[]
        {
            PlayerProperty.Instance.hpValue,
            PlayerProperty.Instance.defenseValue,
            PlayerProperty.Instance.mentalValue,
            PlayerProperty.Instance.level,
            PlayerProperty.Instance.currentExp
        };
    }

    private float[] GetPlayerPositionAndRotation()
    {
        return new float[]
        {
            PlayerProperty.Instance.transform.position.x,
            PlayerProperty.Instance.transform.position.y,
            PlayerProperty.Instance.transform.position.z,
            PlayerProperty.Instance.transform.rotation.x,
            PlayerProperty.Instance.transform.rotation.y,
            PlayerProperty.Instance.transform.rotation.z
        };
    }

    private void SaveData(AllGameData data, int slotNumber)
    {
        if (useJsonSave)
        {
            SaveAsJson(data, slotNumber);
        }
        else
        {
            SaveAsBinary(data, slotNumber);
        }
    }

    private void SaveAsJson(AllGameData data, int slotNumber)
    {
        string json = JsonUtility.ToJson(data, true);
        string filePath = GetSaveFilePath(slotNumber, "json");
        lock (_fileLock)
        {
            File.WriteAllText(filePath, json);
        }
    }

    private void SaveAsBinary(AllGameData data, int slotNumber)
    {
        string filePath = GetSaveFilePath(slotNumber, "bin");
        lock (_fileLock)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                new BinaryFormatter().Serialize(stream, data);
            }
        }
    }
    #endregion

    #region 加载系统
    /// <summary>
    /// 加载游戏数据
    /// </summary>
    public void LoadGame(int slotNumber)
    {
        if (IsSlotEmpty(slotNumber))
        {
            Debug.LogError($"槽位 {slotNumber} 无存档");
            return;
        }

        AllGameData data;
        lock (_fileLock)
        {
            data = LoadData(slotNumber);
        }

        if (data == null || data.version != DataVersion)
        {
            Debug.LogError($"槽位 {slotNumber} 存档无效或版本不匹配（当前版本：{DataVersion}）");
            return;
        }

        ApplyLoadedData(data);
        SceneManager.LoadScene(data.currentScene);
    }

    public AllGameData LoadData(int slotNumber)
    {
        lock (_fileLock)
        {
            return useJsonSave ? LoadFromJson(slotNumber) : LoadFromBinary(slotNumber);
        }
    }

    private AllGameData LoadFromJson(int slotNumber)
    {
        try
        {
            string filePath = GetSaveFilePath(slotNumber, "json");
            lock (_fileLock)
            {
                string json = File.ReadAllText(filePath);
                return JsonUtility.FromJson<AllGameData>(json);
            }
        }
        catch (FileNotFoundException ex)
        {
            Debug.LogError($"JSON 文件未找到：{ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Debug.LogError($"JSON 加载失败：{ex.Message}");
            return null;
        }
    }

    private AllGameData LoadFromBinary(int slotNumber)
    {
        try
        {
            string filePath = GetSaveFilePath(slotNumber, "bin");
            lock (_fileLock)
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    return (AllGameData)new BinaryFormatter().Deserialize(stream);
                }
            }
        }
        catch (FileNotFoundException ex)
        {
            Debug.LogError($"二进制文件未找到：{ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Debug.LogError($"二进制加载失败：{ex.Message}");
            return null;
        }
    }

    private void ApplyLoadedData(AllGameData data)
    {
        if (PlayerProperty.Instance == null || InventoryManager.Instance == null)
        {
            Debug.LogError("玩家/背包管理器未初始化，无法应用存档数据");
            return;
        }

        // 应用玩家属性
        PlayerProperty.Instance.hpValue = data.playerData.playerStats[0];
        PlayerProperty.Instance.defenseValue = (int)data.playerData.playerStats[1];
        PlayerProperty.Instance.mentalValue = data.playerData.playerStats[2];
        PlayerProperty.Instance.level = data.playerData.playerStats[3];
        PlayerProperty.Instance.currentExp = data.playerData.playerStats[4];

        // 应用位置和旋转
        PlayerProperty.Instance.transform.position = new Vector3(
            data.playerData.playerPositionAndRotation[0],
            data.playerData.playerPositionAndRotation[1],
            data.playerData.playerPositionAndRotation[2]
        );

        // 应用背包数据
        InventoryManager.Instance.itemList.Clear();
        InventoryManager.Instance.itemList.AddRange(data.playerData.inventoryContent);

        //任务数据
        TaskManager.Instance.taskDB.Clear();
        TaskManager.Instance.taskDB.AddRange(data.taskStatus);
    }
    #endregion

    #region 音量设置（优化后）
    /// <summary>
    /// 保存音量设置（独立文件保存，使用JSON）
    /// </summary>
    public void SaveVolumeSettings(VolumeSettings settings)
    {
        try
        {
            string savePath = Path.Combine(jsonPath, "VolumeSettings.json");
            string json = JsonUtility.ToJson(settings, true); // 格式化JSON便于阅读
            lock (_fileLock)
            {
                File.WriteAllText(savePath, json);
            }
            Debug.Log("音量设置已保存");
        }
        catch (Exception ex)
        {
            Debug.LogError($"保存音量设置失败：{ex.Message}");
        }
    }

    /// <summary>
    /// 加载音量设置（从独立文件加载）
    /// </summary>
    public VolumeSettings LoadVolumeSettings()
    {
        string savePath = Path.Combine(jsonPath, "VolumeSettings.json");
        if (File.Exists(savePath))
        {
            try
            {
                lock (_fileLock)
                {
                    string json = File.ReadAllText(savePath);
                    return JsonUtility.FromJson<VolumeSettings>(json);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"加载音量设置失败：使用默认值 - {ex.Message}");
            }
        }
        // 返回默认设置
        return new VolumeSettings();
    }
    #endregion

    #region 辅助方法
    public  string GetSaveFilePath(int slotNumber, string ext)
    {
        return Path.Combine(ext == "json" ? jsonPath : binaryPath, $"save_{slotNumber}.{ext}");
    }

    public void DeselectButton()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    /// <summary>
    /// 加载任务状态（公开方法，修复之前的缺失）
    /// </summary>
    public List<GameTaskSO> LoadTaskStates(int slotNumber)
    {
        lock (_fileLock)
        {
            var data = LoadData(slotNumber);
            return data != null ? data.taskStatus : new List<GameTaskSO>();
        }
    }

    /// <summary>
    /// 保存当前场景（使用PlayerPrefs或存档文件）
    /// </summary>
    public void SaveCurrentScene(int slotNumber, string sceneName)
    {
        // 建议保存到AllGameData中，而不是单独使用PlayerPrefs
        // 此处示例：更新存档中的场景名并重新保存
        // （实际应在SaveGameAsync中统一处理）
    }
    #endregion
}
