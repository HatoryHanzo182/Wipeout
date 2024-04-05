using JetBrains.Annotations;
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
    private GameObject _new_point_message;
    private TMPro.TextMeshProUGUI _new_point_text;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _audio_singing_point = GetComponent<AudioSource>();
        _new_point_message = GameObject.Find("NewPointMessage");
        _new_point_text = _new_point_message.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        _new_point_message.SetActive(false);
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _audio_singing_point.Stop();
            _animator.SetInteger("State", 1);
            _audio_open_point.Play();

            GameState.ChackPointNum++;
            _new_point_text.text = $"{GameState.ChackPointNum} open checkpoints!";
            _new_point_message.SetActive(true);

            yield return new WaitForSeconds(3f);

            _new_point_message.SetActive(false);

            Destroy(transform.parent.gameObject);
        }
    }
}