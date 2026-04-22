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

    private Rigidbody _rb;
    private Vector2 _input;

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
        _input = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        // 1. Determine Target Velocity in World Space
        Vector3 targetVelocity = new Vector3(_input.x, 0, _input.y) * _maxSpeed;

        // 2. Calculate Velocity Delta
        Vector3 currentVelocity = _rb.linearVelocity;
        Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
        Vector3 velocityChange = targetVelocity - horizontalVelocity;

        // 3. Apply Acceleration/Braking Logic
        float accelerationRate = (_input.sqrMagnitude > 0.01f) ? _acceleration : _braking;
        Vector3 movementForce = velocityChange * accelerationRate;

        // 4. Apply the Force
        _rb.AddForce(movementForce, ForceMode.Force);
    }

    public void ResetToPosition(Vector3 position)
    {
        transform.position = position;
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    public void Jump(float force)
    {
        _rb.AddForce(new Vector3(0, force, 0), ForceMode.Impulse);
    }
}
