using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometryButton : Geometry
{
    public bool _isSimon = false;
    public bool _isPresed = false;
    public bool _simonGame;
    public GameObject _light;
    // Start is called before the first frame update
    void Start()
    {
        if (_simonGame==true)
        {
            _light.SetActive(!_isSimon);
        }
      
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
                EDebug.Log("Luz");
                _light.SetActive(true);
                GenerateSound();
            }
        }
    }
    #endregion
}
