using UnityEngine;

// Abstract to inherit the OnTriggerEnter2D
public class ProjectileBehavior : WeaponController
{

    public static float Speed = 15f;
    public static float _lifetime = 5f;

    ProjectileBehavior() : base(0, new Vector3()) { }

    private void Start()
    {
        gameObject.transform.parent = null;
        Destroy(gameObject, _lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveVec = transform.right;
        moveVec.z = 0;
        transform.position -= moveVec * Time.deltaTime * Speed; 
    }

    override public void AdditionalTriggerStep(Collider2D collider)
    {
        Debug.Log("FJLKASDJFKLAJDSF");
        Destroy(gameObject);
    }

    override public void ToggleEquipped()
    {
        // Do nothing
    }

override public void UseWeapon()
    {
        // Do nothing
    }
}
