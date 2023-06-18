using Controls;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance { get; private set; }
    
    [SerializeField] private AudioClip _targetHitClip;

    private AudioSource _audioSource;
    
    private void Awake()
    {
        Instance = this;
        _audioSource = GetComponent <AudioSource>();
    }

    public void PlayTargetHit(Target target)
    {
        transform.position = target.transform.position;
        _audioSource.clip = _targetHitClip;
        _audioSource.Play();
    }
}
