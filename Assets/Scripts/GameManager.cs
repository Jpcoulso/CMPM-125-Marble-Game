using UnityEngine;
using UnityEngine.InputSystem;
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

    [Header("UI Elements")] 
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Canvas _pause;
    [SerializeField] private Canvas _victory;

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

        if (_timerText != null)
        {
            _timerText.text = $"Time: {Time.timeSinceLevelLoad:F2}s";
        }

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Pause();
        }
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

    public void Pause()
    {
        if (Time.timeScale > 0f)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
            _pause.gameObject.SetActive(!_pause.gameObject.activeSelf);
    }
}
