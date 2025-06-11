using UnityEngine;

public class sarnaAnmation : MonoBehaviour
{
    [SerializeField] public string isLooking;
    [SerializeField] public string isRunning;

    [SerializeField] public float alertRadius = 10f;
    [SerializeField] public float runRadius = 6f;

    private Animator _animator;
    private Transform _player;

    private bool _hasRun = false;
    private bool _isEscapePlaying = false;

    private int _alertHash;
    private int _runHash;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;

        _alertHash = Animator.StringToHash(isLooking);
        _runHash = Animator.StringToHash(isRunning);
    }

    private void Update()
    {
        if (_player == null) return;

        float distance = Vector3.Distance(transform.position, _player.position);

        // Reakcja na ucieczkę
        if (distance <= runRadius && !_hasRun)
        {
            _hasRun = true;
            _animator.SetTrigger(_runHash);
            _isEscapePlaying = true; // zaczęto animację ucieczki
        }

        // Sprawdzenie, czy animacja ucieczki się zakończyła
        if (_isEscapePlaying)
        {
            AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsTag("Escape") && state.normalizedTime == 1f)
            {
                
                _isEscapePlaying = false;
                // gameObject.SetActive(false);
                Destroy(gameObject, 0.5f);
            }
        }

        // Reakcja na alert
        if (distance <= alertRadius)
            _animator.SetBool(_alertHash, true);
        else
            _animator.SetBool(_alertHash, false);


    }
}
