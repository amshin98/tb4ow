using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStock : MonoBehaviour
{
    //Viewport.Bounds??
    //Camera.Bounds??

    public GameObject player;
    //public GameObject player2;
    //public GameObject player3;
    public Camera cam;

    public GameObject spawnPoint;

    public GameObject heart1, heart2, heart3, gameOver;
    public static int health;

    // Use this for initialization
    private void Start()
    {
        health = 3;
        heart1.gameObject.SetActive(true);
        heart2.gameObject.SetActive(true);
        heart3.gameObject.SetActive(true);
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
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                heart3.gameObject.SetActive(true);
                break;
            case 2:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(true);
                heart3.gameObject.SetActive(false);
                break;
            case 1:
                heart1.gameObject.SetActive(true);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                break;
            case 0:
                heart1.gameObject.SetActive(false);
                heart2.gameObject.SetActive(false);
                heart3.gameObject.SetActive(false);
                gameOver.gameObject.SetActive(true);
                break;
        }

        // Checking if player is out of camera view
        var playerRender = player.GetComponent<Renderer>();

        if (!isVisible(cam, player))
        {
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
