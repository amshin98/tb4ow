using UnityEngine;

public class MeleeController : WeaponController
{
    enum SwingStatus
    {
        IDLE,
        SWINGING,
        RETURNING
    }

    private SwingStatus _swingStatus;
    private float _swingStatusStartTime;
    private float _swingEndTime;

    private Vector3 _startRotation;
    private Vector3 _endRotation;

    private Vector3 _startPosition;
    private Vector3 _endPosition;

    private float _swingSpeed;

    private static float _returnTime = 0.08f;

    public MeleeController(float swingSpeed, float delay, Vector3 equipPos, SpriteRenderer spriteRenderer) :
        base(swingSpeed + delay, equipPos, spriteRenderer)
    {
        _swingSpeed = swingSpeed;

        _swingStatus = SwingStatus.IDLE;
        _swingStatusStartTime = 0.0f;
        _swingEndTime = 9999999f;

        _startPosition = equipPos;
    }

    private void Start()
    {
        _startRotation = new Vector3(0, 0, 0);
        _endRotation = new Vector3(0, 0, -140);

        _endPosition = _startPosition + new Vector3(0.12f, -0.35f, 0);
    }

    // Actually swing the sword. UseWeapon triggers the swing
    private void FixedUpdate()
    {
        if (_swingStatus == SwingStatus.SWINGING)
        {
            float lerpRatio = (Time.time - _swingStatusStartTime) /
                (_swingEndTime - _returnTime - _swingStatusStartTime);

            if (lerpRatio >= 1)
            {
                _swingStatus = SwingStatus.RETURNING;
                _swingStatusStartTime = Time.time;
            }
            else
            {
                transform.localPosition = Vector3.Lerp(_startPosition, _endPosition, lerpRatio);

                Quaternion newRotation = new Quaternion();
                newRotation.eulerAngles = Vector3.Lerp(_startRotation, _endRotation, lerpRatio);
                transform.localRotation = newRotation;
            }
        }
        else if (_swingStatus == SwingStatus.RETURNING)
        {
            float lerpRatio = (Time.time - _swingStatusStartTime) /
                (_swingEndTime - _swingStatusStartTime);

            if (lerpRatio >= 1)
            {
                _swingStatus = SwingStatus.IDLE;
            }
            else
            {
                transform.localPosition = Vector3.Lerp(_endPosition, _startPosition, lerpRatio);

                Quaternion newRotation = new Quaternion();
                newRotation.eulerAngles = Vector3.Lerp(_endRotation, _startRotation, lerpRatio);
                transform.localRotation = newRotation;
            }
        }
    }

    public override void ToggleEquipped()
    {
        base.SetEquipped(!base.GetEquipped());
    }

    override public void UseWeapon()
    {
        _swingStatus = SwingStatus.SWINGING;
        _swingStatusStartTime = Time.time;
        _swingEndTime = Time.time + _swingSpeed;
    }
}
