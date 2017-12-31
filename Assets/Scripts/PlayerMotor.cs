using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMotor : MonoBehaviour {

    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    private Vector3 thrusterForce = Vector3.zero;

    [SerializeField]
    private float cameraRotationLimit = 85f;
    private Rigidbody rb;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }

    //gets a movement vector
    public void move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    //gets a rotational vector
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    //gets a rotational vector for the camera
    public void RotateCamera(float _cameraRotationX)
    {
        cameraRotationX = _cameraRotationX;
    }

    //get a force vector or our thruster    
    public void ApplyThruster(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }

    //run every physics iteration
    public void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    //perform movement based on velocity variable
    public void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        if (thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    //perform rotation
    void PerformRotation()
    {   //"quaternion" is really hard to understand, I don't understand it much myself. But Euler angles, are the
        //angles that we know (in the x, y, and z axis).
        //Basically Quaternion.Euler multiplies the rotation inputted through the mouse, and converts it
        //into a Quaternion.
        //The guy explaining himself said it doesn't truly matter if we understand it, the language takes care of 
        //this itself.
        rb.MoveRotation(rb.rotation * Quaternion.Euler (rotation));
        if (cam != null)
        { //set our rotation, and clamp it
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, + cameraRotationLimit);

          //apply our rotation to the transform of our camera
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f); 
        }
    }
}