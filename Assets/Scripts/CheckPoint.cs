using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [Header("Respawn Point Settings")]
    [Tooltip("Target location for respawning. Fallback to this transform if null.")]
    [SerializeField] private Transform _respawnPoint;
    public Transform RespawnPoint => _respawnPoint != null ? _respawnPoint : transform;

    private bool _isActive = false;

    private void Awake()
    {
        if (_respawnPoint == null)
        {
            _respawnPoint = transform.Find("SpawnPoint");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActive) return;

        if (other.CompareTag("Player"))
        {
            ActivateCheckpoint();
        }
    }

    private void ActivateCheckpoint()
    {
        _isActive = true;
        Debug.Log($"Checkpoint Activated: {gameObject.name}");
        
        GameManager.Instance.CurrentCheckPoint = this;
    }
}
