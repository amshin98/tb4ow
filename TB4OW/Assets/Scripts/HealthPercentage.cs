using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HealthPercentage : MonoBehaviour
{
    public PlayerController player;
    public TMP_Text healthText;

    // Update is called once per frame
    void Start(){
    healthText.text = "";
   
    }
    void Update()
    {
       
        healthText.text = player.curHealth.ToString() + "%";
    
    }
}
