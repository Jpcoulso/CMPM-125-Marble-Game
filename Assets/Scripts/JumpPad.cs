using UnityEngine;
using System.Collections;

public class JumpPad : MonoBehaviour
{
    [Header("Jump Settings")]
    [SerializeField] private float _jumpForce = 500f;
    [SerializeField] private float _delayTime = 0.25f;

    [Header("Juice (FX Settings)")]
    [Tooltip("The child object or component that will scale and change color.")]
    [SerializeField] private GameObject _fxObject;
    [SerializeField] private Color _specialColor = Color.cyan;
    [SerializeField] private float _maxScale = 1.5f;

    private Color _originalColor;
    private Renderer _fxRenderer;
    private Vector3 _originalScale;
    private bool _isJumping = false;

    void Start()
    {
        if (_fxObject != null)
        {
            _fxRenderer = _fxObject.GetComponent<Renderer>();
            _originalScale = _fxObject.transform.localScale;
            
            if (_fxRenderer != null)
            {
                _originalColor = _fxRenderer.material.color;
            }
        }
        else
        {
            Debug.LogWarning("JumpPad: No FX Object assigned in the Inspector!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        marnoldMover mover = other.GetComponent<marnoldMover>();

        if (mover != null && !_isJumping)
        {
            StartCoroutine(ExecuteJuicyJump(mover));
        }
    }

    private IEnumerator ExecuteJuicyJump(marnoldMover mover)
    {
        _isJumping = true;
        float elapsed = 0f;

        while (elapsed < _delayTime)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsed / _delayTime);

            if (_fxObject != null)
            {
                // 1. Color Lerp (Start -> Special over full duration)
                if (_fxRenderer != null)
                {
                    _fxRenderer.material.color = Color.Lerp(_originalColor, _specialColor, normalizedTime);
                }

                // 2. Scale Lerp (1 -> 1.5 -> 1)
                // Using a sine wave or a simple 0-1-0 curve
                // first half (0 to 0.5) goes 0 to 1, second half (0.5 to 1) goes 1 to 0
                float scaleCurve = normalizedTime < 0.5f 
                    ? normalizedTime * 2f 
                    : 2f - (normalizedTime * 2f);
                
                _fxObject.transform.localScale = Vector3.Lerp(_originalScale, _originalScale * _maxScale, scaleCurve);
            }

            yield return null;
        }

        // 3. The Jump
        mover.Jump(_jumpForce);

        // 4. Instant Reset
        if (_fxObject != null)
        {
            if (_fxRenderer != null) _fxRenderer.material.color = _originalColor;
            _fxObject.transform.localScale = _originalScale;
        }

        _isJumping = false;
    }
}
