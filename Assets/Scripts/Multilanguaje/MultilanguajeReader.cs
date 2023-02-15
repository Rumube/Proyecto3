using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultilanguajeReader : MonoBehaviour
{
    public TextAsset _json;
    private MultilanguajeManager _manager;
    public enum LANGUAJE
    {
        es,
        en
    }
    public LANGUAJE language = LANGUAJE.es;
    // Start is called before the first frame update
    void Start()
    {
        _manager = JsonUtility.FromJson<MultilanguajeManager>(_json.text);
        Debug.Log(GetText(0));
    }
    public string GetText(int id)
    {
        switch (language)
        {
            case LANGUAJE.es:
                return _manager.es[id];
                break;
            case LANGUAJE.en:
                return _manager.en[id];
                break;
            default:
                return _manager.en[id];
                break;
        }
    }
}
