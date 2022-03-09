using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    public string pickupSound;
    public string attackSound;
    public string hitSound;
    public SpriteRenderer spriteRendererRef;
    public float _fireRate;
    private float _nextFire;

    private Vector3 _equipPos;

    private bool _equipped;

    public WeaponController(float fireRate, Vector3 equipPos, SpriteRenderer spriteRenderer)
    {
        this.spriteRendererRef = spriteRenderer;

        _fireRate = fireRate;
        _nextFire = 0.0f;

        _equipPos = equipPos;

        _equipped = false;
    }

    public void Awake(){
    }

    public void SetEquipped(bool equipped)
    {
        _equipped = equipped;
    }

    public bool GetEquipped()
    {
        return _equipped;
    }

    public abstract void ToggleEquipped();

    // Handles dealing damage/knockback. For now, destroy targets but in
    // future change to knock back player
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (_equipped)
        {

            string curTag = collider.gameObject.tag;

            Debug.Log(curTag);
            if (curTag == "destructable")
            {
                Destroy(collider.gameObject);
            }
            else if (curTag == "player")
            {
                Debug.Log("KNOCKBACK");
            }
            AdditionalTriggerStep(collider);
        }
    }

    // To be overridden. Additional step when OnTriggerEnter2D is invoked.
    // Used to destroy projectile.
    public virtual void AdditionalTriggerStep(Collider2D collider)
    {
        // Do nothing
    }

    // Handle attacking
    public void Attack()
    {
        if (Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;
            UseWeapon();
        }
    }

    // Handle moving the weapon to its starting position relative to player
    public void Equip()
    {
        gameObject.transform.localPosition = _equipPos;
    }


    // Handles moving the weapon for attacking
    public abstract void UseWeapon();
}
