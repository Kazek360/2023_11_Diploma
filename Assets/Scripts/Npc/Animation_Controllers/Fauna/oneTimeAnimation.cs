using UnityEngine;

public class OneTimeAnimation : MonoBehaviour
{
    [SerializeField] private string triggerName;
    [SerializeField] public float alertRadius = 10f;

    private Animator _animator;
    private Transform _player;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (_player == null) return;

        float distance = Vector3.Distance(transform.position, _player.position);

        // Reakcja na zbliżenie się
        if (distance <= alertRadius)
        {
            _animator.SetTrigger(triggerName);
        }

    }
}
