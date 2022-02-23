using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "projectile")
        {
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }
    }
}
