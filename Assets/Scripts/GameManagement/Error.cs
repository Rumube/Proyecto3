using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Error : MonoBehaviour, IError
{
    public List<Material> _GUIMats;
    public Color _worngColor;
    public Color _correctColor;
    public float _failDuration;

    public void GenerateError()
    {
        GetComponent<CameraShake>().StartShake(_failDuration);
        //TODO: MIN MESSSAGE
        //TODO: AUDIO
        foreach (Material currentGUI in _GUIMats)
        {
            currentGUI.SetColor("_Color", _worngColor);
        }
        StartCoroutine(SetCorrectGUIColor());
    }

    IEnumerator SetCorrectGUIColor()
    {
        yield return new WaitForSeconds(_failDuration);
        foreach (Material currentGUI in _GUIMats)
        {
            currentGUI.SetColor("_Color", _correctColor);
        }
    }
}
