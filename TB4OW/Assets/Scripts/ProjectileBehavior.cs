using UnityEngine;

//used https://www.youtube.com/watch?v=8TqY6p-PRcs&ab_channel=DistortedPixelStudios for reference and inspiration

public class ProjectileBehavior : WeaponController
{
    public static float Speed = 15f;
    public static float _lifetime = 5f;


    ProjectileBehavior() : base(0, new Vector3(), null)
    {
        base.SetEquipped(true);
    }

    private void Start()
    {
        gameObject.transform.parent = null;
        Destroy(gameObject, _lifetime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 moveVec = transform.right;
        moveVec.z = 0;
        transform.position -= moveVec * Time.deltaTime * Speed; 
    }

    override public void AdditionalTriggerStep(Collider2D collider)
    {
        if (collider.gameObject.layer != 8)
        {
            Destroy(gameObject);
        }
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
