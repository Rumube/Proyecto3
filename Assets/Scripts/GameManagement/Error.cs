using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Error : MonoBehaviour, IError
{
    public List<Material> _GUIMats;
    public Color _worngColor;
    public Color _correctColor;
    public float _failDuration;
    public float _shakeAmount = 0.7f;


    public void GenerateError()
    {
        GetComponent<CameraShake>().StartShake(_failDuration, _shakeAmount);
        //TODO: MIN MESSSAGE
        //TODO: AUDIO
        StartCoroutine(SetCorrectGUIColor());
    }

    IEnumerator SetCorrectGUIColor()
    {
        yield return new WaitForSeconds(_failDuration);
    }
}
