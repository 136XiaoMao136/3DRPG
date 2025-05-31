// 简单示例定义 VolumeSettings 类
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VolumeSettings :MonoBehaviour
{
    public static VolumeSettings Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }
    /// <summary>
    /// 主音量（全局音量）
    /// </summary>
    public float masterVolume = 1.0f;

    /// <summary>
    /// 音乐音量
    /// </summary>
    public float musicVolume = 1.0f;

    /// <summary>
    /// 音效音量（SFX）
    /// </summary>
    public float sfxVolume = 1.0f;

    /// <summary>
    /// 初始化默认音量设置
    /// </summary>
    public VolumeSettings()
    {
        masterVolume = 1.0f;
        musicVolume = 1.0f;
        sfxVolume = 1.0f;
    }

    /// <summary>
    /// 复制构造函数（深拷贝）
    /// </summary>
    public VolumeSettings(VolumeSettings other)
    {
        masterVolume = other.masterVolume;
        musicVolume = other.musicVolume;
        sfxVolume = other.sfxVolume;
    }
}