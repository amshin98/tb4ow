using UnityEngine;

// Abstract to inherit the OnTriggerEnter2D
public class ProjectileBehavior : WeaponController
{

    public static float Speed = 15f;
    public static float _lifetime = 5f;

    ProjectileBehavior() : base(0) { }

    private void Start()
    {
        Destroy(gameObject, _lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * Speed; 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    override public void UseWeapon()
    {
        // Do nothing
    }
}
