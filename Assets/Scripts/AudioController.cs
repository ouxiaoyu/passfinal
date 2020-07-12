using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public Slider slider;
    public AudioSource click;
    public AudioSource effect;

    public void ChangeVolume()
    {
        click.volume = slider.value;
        if (slider.value < 0.5f)
        {
            effect.volume = slider.value;
        }
        else
        {
            effect.volume = 1;
        }
        
    }
}
