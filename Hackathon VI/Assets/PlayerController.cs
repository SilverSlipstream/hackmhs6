using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : MonoBehaviour
{
    public Vector3 _velocity = Vector3.zero;
    public Vector3 _rotation = Vector3.zero;
    [SerializeField]
    private string orientation = "Floor";

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;
    [SerializeField]
    private float jumpForce = 1000f;

    private PlayerMotor motor;
    public Rigidbody rb;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Player" && collision.gameObject.name != orientation)
        {
            orientation = gameObject.name;
            float x = rb.transform.rotation.x, y = rb.transform.rotation.y, z = rb.transform.rotation.z;
            switch (collision.gameObject.name)
            {
                case "WallN":
                    z = -90;
                    break;
                case "WallW":
                    x = 90;
                    break;
                case "WallS":
                    z = 90;
                    break;
                case "WallE":
                    x = -90;
                    break;
                case "Ceiling":
                    z = 180;
                    break;
                case "Floor":
                    z = 0;
                    break;
            }
            rb.transform.rotation = Quaternion.Euler(x, y, z);
            motor.jumping = -1;
        }
    }

    void Update()
    {
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        _velocity = (_movHorizontal + _movVertical).normalized * speed;

        motor.Move(_velocity);

        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f);

        motor.Rotate(_rotation);

        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRot * lookSensitivity;

        motor.RotateCamera(_cameraRotationX);

        Vector3 _jumpForce = Vector3.zero;

        if (Input.GetButtonDown("Jump") && motor.jumping == -1)
        {
            _jumpForce = rb.transform.up * jumpForce;
            motor.jumping = 0;
            motor.ApplyJump(_jumpForce);
        }
    }
}
