using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterBar : MonoBehaviour
{
    public Slider slider;
    
    public void SetAmmo(int ammo){
        slider.value = ammo;
    }

    public void SetMaxAmmo(int maxAmmo){
        slider.maxValue = maxAmmo;
        slider.value = maxAmmo;
    }
}
