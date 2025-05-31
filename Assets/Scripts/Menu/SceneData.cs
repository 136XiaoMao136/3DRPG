using UnityEngine;
using System.Threading.Tasks;
using System;

public class SceneData : MonoBehaviour
{
    private AllGameData currentSceneData;

    public async Task SaveCurrentSceneDataAsync(int slotNumber)
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager 未初始化，无法保存当前场景数据。");
            return;
        }

        try
        {
            currentSceneData = SaveManager.Instance.GetAllGameData();
            await SaveManager.Instance.SaveGameAsync(slotNumber);
        }
        catch (Exception ex)
        {
            Debug.LogError($"保存当前场景数据失败（槽位 {slotNumber}）：{ex.Message}");
        }
    }

    public AllGameData LoadSceneData(int slotNumber)
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager 未初始化，无法加载场景数据。");
            return null;
        }

        try
        {
            return SaveManager.Instance.LoadData(slotNumber);
        }
        catch (Exception ex)
        {
            Debug.LogError($"加载场景数据失败（槽位 {slotNumber}）：{ex.Message}");
            return null;
        }
    }
}