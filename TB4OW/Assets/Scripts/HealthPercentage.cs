using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HealthPercentage : MonoBehaviour
{
    public PlayerController player;
    public Text healthText;

    // Update is called once per frame
    void Start(){
    healthText.text = "0%";
    }
    void Update()
    {
        healthText.text = player.curPercent.ToString() + "%";
    
    }
}
