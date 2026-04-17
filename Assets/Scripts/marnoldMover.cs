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
        // Forward (W/S) maps to World Z, Strafe (A/D) maps to World X
        Vector3 targetVelocity = new Vector3(_input.x, 0, _input.y) * _maxSpeed;

        // 2. Smooth Velocity Interpolation (The Envelope)
        // We use 'acceleration' if pushing, and 'braking' if releasing keys
        float currentSpeedLimit = (_input.sqrMagnitude > 0.01f) ? _acceleration : _braking;

        Vector3 currentVelocity = _rb.linearVelocity;
        // We only want to affect the horizontal (X/Z) velocity
        Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
        
        Vector3 newHorizontalVelocity = Vector3.MoveTowards(
            horizontalVelocity, 
            targetVelocity, 
            currentSpeedLimit * Time.fixedDeltaTime
        );

        // 3. Apply the new velocity while preserving gravity/vertical movement
        _rb.linearVelocity = new Vector3(newHorizontalVelocity.x, currentVelocity.y, newHorizontalVelocity.z);
    }

    public void Jump(float force)
    {
        // Preserving the user's ForceMode.Force preference
        _rb.AddForce(new Vector3(0, force, 0), ForceMode.Force);
    }
}
