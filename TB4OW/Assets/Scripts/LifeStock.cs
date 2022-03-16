using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LifeStock : MonoBehaviour
{
    public GameObject player;
    public PlayerController forResWeapons;
   

    public Transform hammer;
    public Transform sword;
    public Transform bucket;
    //public GameObject sword;
    //public GameObject bucket;
    //public GameObject player2;
    //public GameObject player3;
    private Vector2 hammerPos;
    private Vector2 swordPos;
    private Vector2 bucketPos;
    public Camera cam;

    public GameObject spawnPoint;

    public GameObject playerHeart1, playerHeart2, playerHeart3, gameOver;
    public int health;
    public UnityEvent deathEvent;
    public UnityEvent gameOverEvent;

    public GameObject splashObject;

    // Use this for initialization
    private void Start()
    {
        hammerPos = hammer.transform.position; 
        swordPos = sword.transform.position; 
        bucketPos = bucket.transform.position; 
        health = 3;
        playerHeart1.gameObject.SetActive(true);
        playerHeart2.gameObject.SetActive(true);
        playerHeart3.gameObject.SetActive(true);
        gameOver.gameObject.SetActive(false);
    }

    private void ReturnToSS()
    {
        SceneManager.LoadScene("StartScreen");
    }

    private bool firstTime = true;
    // Update is called once per frame
    private void Update()
    {
        // health counter
        if (health > 3)
            health = 3;
        switch (health)
        {
            case 3:
                playerHeart1.gameObject.SetActive(true);
                playerHeart2.gameObject.SetActive(true);
                playerHeart3.gameObject.SetActive(true);
                break;
            case 2:
                playerHeart1.gameObject.SetActive(true);
                playerHeart2.gameObject.SetActive(true);
                playerHeart3.gameObject.SetActive(false);
                break;
            case 1:
                playerHeart1.gameObject.SetActive(true);
                playerHeart2.gameObject.SetActive(false);
                playerHeart3.gameObject.SetActive(false);
                break;
            case 0:
                if (firstTime)
                {
                    playerHeart1.gameObject.SetActive(false);
                    playerHeart2.gameObject.SetActive(false);
                    playerHeart3.gameObject.SetActive(false);
                    gameOver.gameObject.SetActive(true);
                    gameOverEvent.Invoke();
                    Invoke("ReturnToSS", 4.0f);
                    firstTime = false;
                }
                break;
        }

        // Checking if player is out of camera view
        var playerRender = player.GetComponent<Renderer>();

        if (!isVisible(cam, player))
        {
            MakeSplash(player.transform.position);
            deathEvent.Invoke();
            health -= 1;
            SpawnObject();
            //Destroy(player);
        }
    }

    private bool isVisible(Camera c, GameObject player)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(c); // planes bordering camera view
        var point = player.transform.position; // position of player

        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0) // if player is within sight of camera
            {
                return false;
                //return true;
            }
        }
        return true;
        //return false;
    }

    public void SpawnObject()
    {
        //Vector2 spawnPoint = new Vector2(0, 0);
        //Instantiate(player, spawnPoint, Quaternion.identity);
        player.transform.position = spawnPoint.transform.position;
        if(forResWeapons.curWeapon is HammerController){
            forResWeapons.curWeapon.ToggleEquipped();
            forResWeapons.curWeapon.transform.parent = null;
            forResWeapons.curWeapon = null;
            hammer.position = hammerPos;
        }
        if(forResWeapons.curWeapon is SwordController){
            forResWeapons.curWeapon.ToggleEquipped();
            forResWeapons.curWeapon.transform.parent = null;
            forResWeapons.curWeapon = null;
            sword.position = swordPos;
        }
        if(forResWeapons.curWeapon is RangedController){
            forResWeapons.curWeapon.ToggleEquipped();
            forResWeapons.curWeapon.transform.parent = null;
            forResWeapons.curWeapon = null;
            bucket.position = bucketPos;
        }
        // sword.transform.position = swordPos;
        // bucket.transform.position = bucketPos;
    }

    public void MakeSplash(Vector3 pos)
    {
        // x variable, when in viewport space clamp to 0 to 1. y should be 0 in viewport space
        var viewPos = Camera.main.WorldToViewportPoint(pos);
        viewPos.x = Mathf.Clamp(viewPos.x, 0f, 1f);
        viewPos.y = 0.0f;

        var camRot = Camera.main.transform.rotation;

        var splash = Instantiate(splashObject, Camera.main.ViewportToWorldPoint(viewPos), camRot);
        splash.transform.position += new Vector3(0f, 3.0f, 0f);
        splash.transform.up = new Vector3(0,1,0);
        Destroy(splash, splash.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
}
