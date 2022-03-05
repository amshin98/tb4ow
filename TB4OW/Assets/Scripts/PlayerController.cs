using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // With 4.2 gravity scale
    public float MovementSpeed = 12;
    public float jumpForce = 14; 
    public ProjectileBehavior ProjectilePrefab;
    public Transform launchPoint;
    
    public float knockbackForce;

    private Rigidbody2D _rigidbody;

    // Weapon
    public WeaponController curWeapon;
    public float pickupRange = 10f;
    private Collider2D _collider;
    public int curHealth;
    public int maxHealth;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        healthBar.setMaxHealth(maxHealth);
        curHealth = maxHealth; 
    }

    // Update is called once per frame
    void Update()
    {

       // Debug.Log(curHealth);
        var movement = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;




        // Movement
        if(!Mathf.Approximately(0, movement)){
            transform.rotation = movement < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        }
        if(Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody.velocity.y) < 0.001f)
        {
            _rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            damage(20);
            
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
                curWeapon.transform.parent = null;
                curWeapon = null;
                Debug.Log("dropp");
            }
            else
            {
                // Check for weapon pickup
                LayerMask weaponLayerMask = LayerMask.GetMask("Weapons");

                Collider2D[] weaponsInRange = Physics2D.OverlapCircleAll(
                    transform.position, pickupRange, weaponLayerMask);

                if (weaponsInRange.Length > 0)
                {

                    // Find closest weapon
                    int minDistIdx = 0;
                    float minDist = _collider.Distance(weaponsInRange[0]).distance;

                    for (int i = 1; i < weaponsInRange.Length; i ++)
                    {
                        float curDist = _collider.Distance(weaponsInRange[i]).distance;
                        if (curDist < minDist)
                        {
                            minDistIdx = i;
                            minDist = curDist;
                        }
                    }

                    // Pick up, equip, and active weapon
                    curWeapon = weaponsInRange[minDistIdx].gameObject.GetComponent<WeaponController>();
                    curWeapon.transform.parent = gameObject.transform;
                    curWeapon.Equip();
                    curWeapon.ToggleEquipped();
                }
            }
        }
    }

        public void damage(int value){
            curHealth -= value;
            healthBar.setSliderHealth(curHealth);
    }

private void OnTriggerEnter2D(Collider2D other)
{
    if(other.tag == "Enemy")
    {
        Vector2 difference = (transform.position - other.transform.position).normalized;
        Vector2 force = difference * knockbackForce;
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
       
    }
}

}
