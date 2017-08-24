using UnityEngine;
using System.Collections;

public class SpaceFlightController : MonoBehaviour
{

    /* This is identical to the Flight Sim script, I just optimized it for realistic space flight
    In space there is no gravity or lift, and no need to bank into a turn.
    */


    // Componants of the flyer
    public GameObject flyer;

    // Various control variables. These mostly control realism settings, change as you see fit.
    public float throttleDelta = 1;            // This defines how fast the throttle value changes
    public float accelerateConst = 10;
    public float decelerateConst = 0;
    public float smoothRotation = 1.5f;
    public int speedConst = 250;
    public int throttleConst = 30;
    public float maxSpeed = 100;
    public float liftConst;                    // Another arbitrary constant, change it as you see fit.
    public float dragConst;                // Note that this is NOT the same as the rigidbody drag...
    public float gravityConst = 9.8f;             // An arbitrary gravity constant, there is no particular reason it has to be 9.8f...


    // Rotation Variables, these change how your plane turns.
    public bool lockedConst;       // If this is checked, it locks pitch roll and yaw const to the var rotationConst.
    public int rotationConst = 100;
    public int pitchConst = 80;
    public int rollConst = 80;
    public int yawConst = 80;
    public float pitchDelta = 1.05f;
    public float rollDelta = 1.05f;
    public float yawDelta = 1.05f;


    // Private variables that dont need to clutter the inspector panel.
    private float trueSmooth;
    private float truePitch;
    private float trueRoll;
    private float trueYaw;

    // Airplane Aerodynamics - don't alter these... Anything that is preceded by "true" is directly plugged into a movement or rotation line.
    private float thrust;
    private float trueLift = 0;
    private float trueThrust;
    private float trueDrag;
    private float trueGrav;



    // Let the games begin!
    void Start()
    {
        flyer.GetComponent< Rigidbody > ().drag = 1;
        if (lockedConst == true)
        {
            pitchConst = rotationConst;
            rollConst = rotationConst;
            yawConst = rotationConst;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        // This section of code handles the plane's rotation.
        var pitch = -Input.GetAxis("Pitch") * pitchConst;
        var roll = Input.GetAxis("Roll") * rollConst;
        var yaw = -Input.GetAxis("Yaw") * yawConst;

        pitch *= pitchDelta * Time.deltaTime;
        roll *= -rollDelta * Time.deltaTime;
        yaw *= yawDelta * Time.deltaTime;

        // Smothing Rotations...
        trueSmooth = Mathf.Lerp(trueSmooth, smoothRotation, 5 * Time.deltaTime);
        truePitch = Mathf.Lerp(truePitch, pitch, trueSmooth * Time.deltaTime);
        trueRoll = Mathf.Lerp(trueRoll, roll, trueSmooth * Time.deltaTime);
        trueYaw = Mathf.Lerp(trueYaw, yaw, trueSmooth * Time.deltaTime);


        // * * This next block handles the thrust and drag.
        // This block sets the value of the joystick throttle ( float value between 0 and 1)
        var throttle = (-(Input.GetAxis("Throttle")) + 1) / 2 * throttleConst;
        throttle *= throttleDelta * Time.deltaTime;
        if (throttle >= trueThrust)
        {
            trueThrust = Mathf.SmoothStep(trueThrust, throttle, accelerateConst * Time.deltaTime);
        }
        if (throttle < trueThrust)
        {
            trueThrust = Mathf.Lerp(trueThrust, throttle, decelerateConst * Time.deltaTime);
        }

        // This is a airbrake, this increases drag, lowering your speed
        //	if (Input.GetButtonDown ("Airbrake"))
        //	{
        // Do such
        //	}

        // * * Now we are applying lift and gravity to airplane.

    }  


    // Now we apply what we calculated...	
    void FixedUpdate()
    {
        // Seperated addRelativeForce so we have better controll over when we want them to run.
        if (trueThrust <= maxSpeed)
        {
            // Horizontal Force
            flyer.GetComponent< Rigidbody > ().AddRelativeForce(0, 0, trueThrust * speedConst);
            //transform.Translate (0,0,trueThrust);
        }

        flyer.GetComponent< Rigidbody > ().AddRelativeForce(0, trueLift, 0);
        transform.Rotate(truePitch, -trueYaw, trueRoll);
    }
}