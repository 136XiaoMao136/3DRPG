using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortalUI : MonoBehaviour
{
    public static PortalUI Instance { get; private set; }
    public GameObject portalUIGameObject;
    public Button yesButton;
    public Button noButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
  
        Instance = this;
    }

    public void Show()
    {
        if (portalUIGameObject != null)
        {
            portalUIGameObject.SetActive(true);
            PortalConfirm();
        }
        else
        {
            Debug.LogError("portalUIGameObject 未正确赋值");
        }
    }

    public void PortalConfirm()
    {
        if (yesButton != null)
        {
            // 先移除已有的监听器（如果有的话）
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(() =>
            {
 
                SceneManager.LoadScene("GameScreen3");
             
            });
        }
        else
        {
            Debug.LogError("yesButton 未正确赋值");
        }
    }

}