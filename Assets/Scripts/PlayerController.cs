using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 500000f;
    [SerializeField]
    private float lookSensitivity = 3f;

    [SerializeField]
    private float thrusterForce = 1000f;

 
    [Header("Spring settings:")]
    [SerializeField]
    private float jointSpring =20f;
    [SerializeField]
    private float jointMaxForce = 40f;


    //component caching  
    private PlayerMotor motor;
    private ConfigurableJoint joint;
    private Animator animator;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent < ConfigurableJoint>();
        animator = GetComponent<Animator>(); 
        SetJointSettings(jointSpring);
    }

    void Update()
    {
        //calculate our movement velocity as a 3d vector
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        //Final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical) * speed;


        //animate movement
        animator.SetFloat("ForwardVelocity", _zMov);
        //Apply our movement
        motor.move(_velocity);

        //calculate player rotation as a 3d Vector(moving around)
        float _yRot = Input.GetAxisRaw("Mouse X");
        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        //apply rotation
        motor.Rotate(_rotation);

        //calculate camera rotation as a 3d Vector: turning around the camera
        float _xRot = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRot * lookSensitivity;

        //apply camera rotation
        motor.RotateCamera(_cameraRotationX);

        //calculate the thruster force based on user input
        Vector3 _thrusterForce = Vector3.zero;
        
        if (Input.GetButton("Jump"))
        {
            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(0f);
        }else
        {
            SetJointSettings(jointSpring);
        }
        //apply thruster force  
        motor.ApplyThruster(_thrusterForce);
    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive
        {
          positionSpring = _jointSpring,
          maximumForce = jointMaxForce
        };
    }
}
