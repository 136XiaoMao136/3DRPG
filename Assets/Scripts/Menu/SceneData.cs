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
            Debug.LogError("SaveManager δ��ʼ�����޷����浱ǰ�������ݡ�");
            return;
        }

        try
        {
            currentSceneData = SaveManager.Instance.GetAllGameData();
            await SaveManager.Instance.SaveGameAsync(slotNumber);
        }
        catch (Exception ex)
        {
            Debug.LogError($"���浱ǰ��������ʧ�ܣ���λ {slotNumber}����{ex.Message}");
        }
    }

    public AllGameData LoadSceneData(int slotNumber)
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager δ��ʼ�����޷����س������ݡ�");
            return null;
        }

        try
        {
            return SaveManager.Instance.LoadData(slotNumber);
        }
        catch (Exception ex)
        {
            Debug.LogError($"���س�������ʧ�ܣ���λ {slotNumber}����{ex.Message}");
            return null;
        }
    }
}