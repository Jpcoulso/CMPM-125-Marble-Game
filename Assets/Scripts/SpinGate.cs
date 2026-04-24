using UnityEngine;

public class SpinGate : MonoBehaviour
{
    private Transform _transform;
    [SerializeField] private float _rotationSpeed = 40f;
    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }
}
