using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class marnoldMover : MonoBehaviour
{
    //[Header("Movement Envelope Settings")]
    //[Tooltip("Force applied at full sustain for input")]
    //[SerializeField] private float _max_force = 20f;
    //[Tooltip("Seconds to reach max force during sustained input")]
    //[SerializeField] private float _ramp_time = 0.5f;
    //[Tooltip("Seconds to decay to 0 force after input ends")]
    //[SerializeField] private float _decay_time = 0.5f;
    //[Tooltip("Handling")]
    //[SerializeField] private float _turnrate = 0.15f;
    [SerializeField] private float _speeeeed = 3f;
    private Rigidbody _rigidbody;
    private Vector2 _raw_input;
    private float _movement_x;
    private float _movement_y; 

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //_rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        //_rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        //_rigidbody.angularDamping = 0.5f;
        //_rigidbody.linearDamping = 0.1f;
    }
    void OnMove(InputValue value)
    {
        _raw_input = value.Get<Vector2>();
        _movement_x = _raw_input.x;
        _movement_y = _raw_input.y;
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(_movement_x, 0.0f, _movement_y) * _speeeeed;
        _rigidbody.AddForce(movement);
    }

    public void Jump(float force)
    {
        _rigidbody.AddForce(new Vector3(0, force, 0));
    }
}
