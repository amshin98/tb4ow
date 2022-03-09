using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//https://www.youtube.com/watch?v=BLfNP4Sc_iA&ab_channel=Brackeys for reference 
public class HealthBar : MonoBehaviour
{

    public Slider slider;
    // Start is called before the first frame update
    

    public void setMaxHealth(int health){
        slider.maxValue = health;
        slider.value = health;
    }

    public void setSliderHealth(int health){
        slider.value = health;
    }
}
