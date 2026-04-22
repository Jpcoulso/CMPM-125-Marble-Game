using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player Reference")]
    [SerializeField] private marnoldMover _player;

    [Header("Checkpoint System")]
    [SerializeField] private CheckPoint _currentCheckPoint;
    public CheckPoint CurrentCheckPoint
    {
        get => _currentCheckPoint;
        set => _currentCheckPoint = value;
    }

    [Header("Kill Settings")]
    [SerializeField] private float _fallThreshold = -10f;
    public float FallThreshold => _fallThreshold;

    private Vector3 _startPosition;

    [SerializeField] private TextMeshProUGUI _timerText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (_player == null)
        {
            _player = Object.FindFirstObjectByType<marnoldMover>();
        }

        if (_player != null)
        {
            _startPosition = _player.transform.position;
            if (!_player.CompareTag("Player"))
            {
                Debug.LogWarning("Player is missing the 'Player' tag! Checkpoints will not work.");
            }
        }
    }

    private void Update()
    {
        if (_player != null && _player.transform.position.y < _fallThreshold)
        {
            Respawn();
        }
        _timerText.text = $"Time: {Time.timeSinceLevelLoad:F2}s";
    }

    public void Respawn()
    {
        if (_player == null) return;

        Vector3 respawnPos = _currentCheckPoint != null
            ? _currentCheckPoint.RespawnPoint.position
            : _startPosition;

        _player.ResetToPosition(respawnPos);

        if (_currentCheckPoint == null)
        {
            Debug.Log("Respawning to start position (no checkpoint active).");
        }
    }

    public void LevelComplete()
    {
        Debug.Log("Level Complete! Implement level transition logic here.");
    }

}
