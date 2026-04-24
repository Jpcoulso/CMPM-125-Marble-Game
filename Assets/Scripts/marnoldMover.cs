using System.Numerics;
//using System.Threading.Tasks.Dataflow;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class marnoldMover : MonoBehaviour
{
    [Header("Movement (Target Velocity)")]
    [Tooltip("Maximum rolling speed.")]
    [SerializeField] private float _maxSpeed = 10f;
    [Tooltip("How fast it reaches max speed (Ramp).")]
    [SerializeField] private float _acceleration = 30f;
    [Tooltip("How fast it slows down (Decay).")]
    [SerializeField] private float _braking = 20f;

    [SerializeField] Transform cameraTransform;

    private Rigidbody _rb;
    private UnityEngine.Vector2 _input;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        // Physics cleanup for marbles
        _rb.linearDamping = 0.5f;
        _rb.angularDamping = 0.5f;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    // Input System Message
    void OnMove(InputValue value)
    {
        _input = value.Get<UnityEngine.Vector2>();
    }

    void FixedUpdate()
    {

        // calculate direction to move based on camera position
        UnityEngine.Vector3 forward = cameraTransform.forward;
        UnityEngine.Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        UnityEngine.Vector3 moveDirection = (forward * _input.y) + (right * _input.x);

        // 1. Determine Target Velocity in World Space
        UnityEngine.Vector3 targetVelocity = moveDirection * _maxSpeed;

        // 2. Calculate Velocity Delta
        UnityEngine.Vector3 currentVelocity = _rb.linearVelocity;
        UnityEngine.Vector3 horizontalVelocity = new UnityEngine.Vector3(currentVelocity.x, 0, currentVelocity.z);
        UnityEngine.Vector3 velocityChange = targetVelocity - horizontalVelocity;

        // 3. Apply Acceleration/Braking Logic
        float accelerationRate = (_input.sqrMagnitude > 0.01f) ? _acceleration : _braking;
        UnityEngine.Vector3 movementForce = velocityChange * accelerationRate;

        // 4. Apply the Force
        _rb.AddForce(movementForce, ForceMode.Force);
    }

    public void ResetToPosition(UnityEngine.Vector3 position)
    {
        transform.position = position;
        _rb.linearVelocity = UnityEngine.Vector3.zero;
        _rb.angularVelocity = UnityEngine.Vector3.zero;
    }

    public void Jump(float force)
    {
        _rb.AddForce(new UnityEngine.Vector3(0, force, 0), ForceMode.Impulse);
    }
}
