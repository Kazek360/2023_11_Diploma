using UnityEngine;

public class RandomSoundFromCertainDistance : MonoBehaviour
{
    [SerializeField] public AudioClip[] audioClip;
    [SerializeField] public float soundDistance = 30f;
    private Transform _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (_player == null) return;

        float distance = Vector3.Distance(transform.position, _player.position);

        while (distance <= soundDistance)
        {
            SoundFxManager.Instance.PlayRandomSoundFxClip(audioClip, transform, 1f);
        }
    }


}
