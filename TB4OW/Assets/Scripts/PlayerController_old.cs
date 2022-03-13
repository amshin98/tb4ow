using UnityEngine;

public class PlayerController_old : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float movementSpeed = 12f;
    public float jumpForce = 14f;
    public float knockbackSpeed = 10f;
    public float knockbackForce = 5f;
    public Transform launchPoint;
    public int knockBackFrames = 50;


    [Header("Weapon Interact Parameters")]
    public float pickupRange = 10f;


    [Header("Player Parameters")]
    public WeaponController curWeapon;
    public float curPercent = 100;
    public HealthBar healthBar;
    public bool isAI = false;


    [Header("Script References")]
    public ProjectileBehavior ProjectilePrefab;

    private Rigidbody2D _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!isAI)
        {
            var playerInput = Input.GetAxisRaw("Horizontal");

            // Movement
            MovePlayer(playerInput, Input.GetKey(KeyCode.W));

            // Attacking
            if (Input.GetKey(KeyCode.J) && curWeapon != null)
            {
                curWeapon.Attack();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                WeaponInteract();
            }
        }
    }

    public void MovePlayer(float playerInput, bool jump)
    {
        transform.position += movementSpeed * Time.deltaTime * new Vector3(playerInput, 0, 0);

        if (!Mathf.Approximately(0, playerInput))
        {
            transform.rotation = playerInput < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        }
        if (jump && Mathf.Abs(_rigidbody.velocity.y) < 0.001f)
        {
            _rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void WeaponInteract()
    {
        if (curWeapon != null)
        {
            // Drop weapon
            curWeapon.transform.parent = null;
            curWeapon = null;
        }
        else
        {
            // Check for weapon pickup
            LayerMask weaponLayerMask = LayerMask.GetMask("Weapons");

            Collider2D[] weaponsInRange = Physics2D.OverlapCircleAll(
            transform.position, pickupRange, weaponLayerMask);

            if (weaponsInRange.Length > 0)
            {


                // Pick up, equip, and active weapon
                curWeapon = weaponsInRange[0].gameObject.GetComponent<WeaponController>();
                Debug.Log(curWeapon);
                curWeapon.transform.parent = gameObject.transform;
                curWeapon.Equip();
                curWeapon.ToggleEquipped();
            }
        }
    }
    public void damage(float value)
    {
        curPercent += value;
        healthBar.setSliderHealth((int)curPercent);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

    }

}