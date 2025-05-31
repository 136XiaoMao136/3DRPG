using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    public Button exitButton;
    private void Awake()
    {
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    public  void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
