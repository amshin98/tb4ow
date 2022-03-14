using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifeStock : MonoBehaviour
{
    //Viewport.Bounds??
    //Camera.Bounds??

    public GameObject player;
    //public GameObject player2;
    //public GameObject player3;
    public Camera cam;

    public GameObject spawnPoint;

    public GameObject playerHeart1, playerHeart2, playerHeart3, gameOver;
    public int health;
    public UnityEvent deathEvent;

    // Use this for initialization
    private void Start()
    {
        health = 3;
        playerHeart1.gameObject.SetActive(true);
        playerHeart2.gameObject.SetActive(true);
        playerHeart3.gameObject.SetActive(true);
        gameOver.gameObject.SetActive(false);
    }

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
                playerHeart1.gameObject.SetActive(false);
                playerHeart2.gameObject.SetActive(false);
                playerHeart3.gameObject.SetActive(false);
                gameOver.gameObject.SetActive(true);
                break;
        }

        // Checking if player is out of camera view
        var playerRender = player.GetComponent<Renderer>();

        if (!isVisible(cam, player))
        {
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
    }
}
