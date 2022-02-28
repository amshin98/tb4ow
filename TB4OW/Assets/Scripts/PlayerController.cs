
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // With 4.2 gravity scale
    public float MovementSpeed = 12;
    public float jumpForce = 14; 
    public ProjectileBehavior ProjectilePrefab;
    public Transform launchPoint;
    private bool canShoot; 
    private bool bucketPickUp; 

    public float fireRate = 1f;
    private float nextFire = 0.0f;

    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
       
        bucketPickUp = false; 
    }

    // Update is called once per frame
    void Update()
    {
        var movement = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;


        if(!Mathf.Approximately(0, movement)){
            transform.rotation = movement < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        }
        if(Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody.velocity.y) < 0.001f)
        {
            _rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        
        if(canShoot){
        if(Input.GetButtonDown("Fire1")  && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(ProjectilePrefab, launchPoint.position, transform.rotation);
        }
        }

        if(Input.GetButtonDown("Fire2") && bucketPickUp){
            canShoot = true;
        }
        Debug.Log(bucketPickUp);

      
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
  
        if (collision.gameObject.name == "Bucket") //checks if its on bucket 
        {
          
          bucketPickUp = true; 
         
        }  


    }

    void OnCollisionExit2D(Collision2D collision)
    {
  
        if (collision.gameObject.name == "Bucket") //checks if its on bucket 
        {
          
          bucketPickUp = false; 
        }  


    }
    

}
