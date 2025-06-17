using UnityEngine;

public class MatkaAnimations : MonoBehaviour
{

    private static readonly int X = Animator.StringToHash("X");
    private static readonly int Y = Animator.StringToHash("Y");
    private static readonly float IsRunning = Animator.StringToHash("isRunning");

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

        // Różnica pozycji
        Vector3 directionToPlayer = -1 * (_player.position - transform.position);

        // Normalizacja, jeżeli potrzebujesz również kierunku w pionie (Y)
        directionToPlayer.Normalize();

        // Ustaw wartości blend tree
        _animator.SetFloat(X, directionToPlayer.x);
        //TODO do zmiany później
        _animator.SetFloat(Y, -1);
        
    }
}
