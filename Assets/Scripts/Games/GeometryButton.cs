using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometryButton : Geometry
{
    public bool _isSimon = false;
    private bool _isPresed = false;
    public GameObject _light;
    // Start is called before the first frame update
    void Start()
    {
        _light.SetActive(!_isSimon);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #region Button
    public void OnClickButton()
    {
        if (!_isSimon)
        {
            _isPresed = !_isPresed;
            if (_isPresed && _light.activeSelf)
            {
                _light.SetActive(false);
                GenerateSound();
            }
            else if (!_isPresed && !_light.activeSelf)
            {
                _light.SetActive(true);
                GenerateSound();
            }
        }
        else
        {
            //TODO: ENCENDER Y APAGAR EN SIMON
        }
    }
    #endregion
}
