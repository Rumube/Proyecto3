using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    TextMeshProUGUI _text;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(Blinking());     
    }

    /// <summary>Hide/unhide the text smoothly</summary>
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
