using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CheckPointScript : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audio_singing_point;
    [SerializeField]
    public AudioSource _audio_open_point;
    public float _delay_before_destroy = 1f;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _audio_singing_point = GetComponent<AudioSource>();
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _audio_singing_point.Stop();
            _animator.SetInteger("State", 1);
            _audio_open_point.Play();

            yield return new WaitForSeconds(_delay_before_destroy);

            Destroy(transform.parent.gameObject);
        }
    }
}