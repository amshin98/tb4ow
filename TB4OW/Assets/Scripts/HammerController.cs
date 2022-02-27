public class HammerController : MeleeController
{
    public static float _swingSpeed = 0.5f;
    public static float _delay = 0.2f;


    public HammerController() : base(_swingSpeed, _delay) { }
}
