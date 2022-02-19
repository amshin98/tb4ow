using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float MovementSpeed = 3;
    public float jumpForce = 1; 
    public ProjectileBehavior ProjectilePrefab;
    public Transform launchPoint;


    public float fireRate = 1f;
    private float nextFire = 0.0f;

    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;

        if(Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody.velocity.y) < 0.001f)
        {
            _rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        
        if(Input.GetButtonDown("Fire1")  && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
           Instantiate(ProjectilePrefab, launchPoint.position, transform.rotation);
        }
    }
}
