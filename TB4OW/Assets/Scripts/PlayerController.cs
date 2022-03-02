
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // With 4.2 gravity scale
    public float MovementSpeed = 12;
    public float jumpForce = 14; 
    public ProjectileBehavior ProjectilePrefab;
    public Transform launchPoint;

    private Rigidbody2D _rigidbody;

    // Weapon
    public WeaponController curWeapon;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var movement = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;

        // Movement
        if(!Mathf.Approximately(0, movement)){
            transform.rotation = movement < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        }
        if(Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody.velocity.y) < 0.001f)
        {
            _rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        
        // Attacking
        if(Input.GetButtonDown("Fire1") && curWeapon != null)
        {
            curWeapon.Attack();
        }

        // TODO: pickup
        if(Input.GetButtonDown("Fire2"))
        {
            if(curWeapon != null)
            {
                // Drop weapon
            }
            else
            {
                // Check for weapon pickup
            }
        }
    }
}
