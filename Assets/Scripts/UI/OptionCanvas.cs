using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionCanvas : MonoBehaviour
{
    public Dropdown GraphicsDropdown;
    public UnityEngine.Audio.AudioMixer MasterVolume;

    public void SetMainMixer(float volume)
    {
        MasterVolume.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void Exit()
    {
        gameObject.SetActive(false);
    }
}
