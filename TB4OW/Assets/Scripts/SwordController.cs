public class SwordController : MeleeController
{
    public static float _swingSpeed = 0.2f;
    public static float _delay = 0.2f;

    public SwordController() : base(_swingSpeed, _delay) {}
}
