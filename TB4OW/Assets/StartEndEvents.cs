using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class StartEndEvents : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private UnityEvent loadEvents;
    [SerializeField] private UnityEvent startBattleEvents;
    [SerializeField] private UnityEvent cleanupEvents;

    [SerializeField] private float startBattleDelay = 4f;
    public UnityEvent endEvents;
    private System.Random rand;
    void Start()
    {
        rand = new System.Random();
        loadEvents.Invoke();

        if(audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
        }

        PlayRandomBattleTheme("BattleTheme", 3);

        Invoke("InvokeStartEvents", startBattleDelay);
        Invoke("InvokeCleanupEvents", startBattleDelay + 1);
    }

    public void InvokeStartEvents()
    {
        startBattleEvents.Invoke();
    }

    public void InvokeCleanupEvents()
    {
        cleanupEvents.Invoke();
    }

    public void InvokeEndEvents()
    {
        endEvents.Invoke();
    }


    public void PlayRandomBattleTheme(string rootName, int numThemes)
    {
        audioManager.Play(rootName + rand.Next(1, numThemes + 1));
    }

 
}
