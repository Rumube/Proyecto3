using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    Text _text;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
        StartCoroutine(Blinking());
       
    }

    IEnumerator Blinking()
    {
        while (gameObject.activeInHierarchy)
        {
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b,Mathf.Sin(Time.time * 2));
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }
}
