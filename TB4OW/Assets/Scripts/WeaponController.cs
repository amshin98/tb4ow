using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private string pickupSound;
    [SerializeField] private string attackSound;
    [SerializeField] private string hitSound;

    [Header("References")]
    public string label;
    public SpriteRenderer spriteRendererRef;
    public AudioManager audioManager = null;

    [Header("Behaviors")]
    [SerializeField] private float _fireRate;
    public bool isFacingRight;
    [SerializeField] protected bool attacking = false;
    [SerializeField] private bool _equipped;




    private float _nextFire;
    private Vector3 _equipPos;
    protected Vector2 launchVector;
    protected float percentDamage = 10f;

    public WeaponController(float fireRate, Vector3 equipPos, SpriteRenderer spriteRenderer, string label)
    {
        this.spriteRendererRef = spriteRenderer;
        this.label = label;

        _fireRate = fireRate;
        _nextFire = 0.0f;
        _equipPos = equipPos;
        _equipped = false;
    }

    public void Awake(){
        // _collider = GetComponent<Collider2D>();
        if (audioManager == null){
            audioManager = FindObjectOfType<AudioManager>();
        }
    }

    public void SetEquipped(bool equipped)
    {
        _equipped = equipped;
        // if(_equipped){
        // }
    }

    public bool GetEquipped()
    {
        return _equipped;
    }

    public abstract void ToggleEquipped();

    // Handles dealing damage/knockback
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (_equipped && attacking && other.CompareTag("Player"))
        {
            PlayerController otherPC = other.GetComponent<PlayerController>();
            float playerPercent = otherPC.curPercent;
            Rigidbody2D otherRigidbody = other.GetComponent<Rigidbody2D>();
            playerPercent += percentDamage;

            Vector2 scaledKnockback = launchVector * GetLaunchScale(playerPercent);
            Debug.Log(scaledKnockback);
            if (isFacingRight)
            {
                otherRigidbody.AddForce(scaledKnockback);
            }
            else
            {
                otherRigidbody.AddForce(new Vector2(scaledKnockback.x * -1, scaledKnockback.y));
            }

            otherPC.curPercent = playerPercent;

            audioManager.Play(hitSound);
        }
    }

    private float GetLaunchScale(float healthPercent)
    {
        return .1f * (Mathf.Pow(healthPercent, 2)) + 10f;
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
            audioManager.Play(attackSound);
            UseWeapon();
        }
    }

    // Handle moving the weapon to its starting position relative to player
    public void Equip()
    {
        gameObject.transform.localPosition = _equipPos;
        audioManager.Play(pickupSound);

    }


    // Handles moving the weapon for attacking
    public abstract void UseWeapon();
}
