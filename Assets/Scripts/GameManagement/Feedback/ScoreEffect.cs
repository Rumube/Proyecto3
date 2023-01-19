using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreEffect : MonoBehaviour
{
    private AudioSource _audio;
    public AudioClip _clip;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector2(transform.localScale.x-0.5f * Time.deltaTime, transform.localScale.y - 0.5f * Time.deltaTime);
        if (transform.localScale.x <= 0)
            Destroy(gameObject);
    }

    public void InitValues(int value, float pitch)
    {
        _audio = GetComponent<AudioSource>();
        _audio.pitch = pitch;
        _audio.clip = _clip;
        _audio.Play();
        int degrees = Random.Range(0, 21);
        int negative = Random.Range(0, 2);

        if (negative == 0)
            transform.rotation = Quaternion.Euler(0, 0, degrees);
        else
            transform.rotation = Quaternion.Euler(0, 0, -degrees);

    }
}
