using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    // 修正拼写错误
    public GameObject menuCanvas;
    public GameObject uiCanvas;
    public GameObject saveMenu;
    public GameObject settingMenu;
    public GameObject menu;
    public bool isMenuOpen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(uiCanvas);
        DontDestroyOnLoad(menuCanvas);
    }

    private void Update()
    {
        Debug.Log("Update 方法执行，当前 isMenuOpen 状态: " + isMenuOpen);
        if (Input.GetKeyDown(KeyCode.Escape) && !isMenuOpen)
        {
            uiCanvas.SetActive(false);
            menuCanvas.SetActive(true);
            isMenuOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isMenuOpen)
        {
            menuCanvas.SetActive(false);
            uiCanvas.SetActive(true);
            isMenuOpen = false;
        }
    }
}

