using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCounter : MonoBehaviour
{
    public int _nSquare;
    public int _nTriangle;
    public int _nCircle;

    public int _squareCounter;
    public int _triangleCounter;
    public int _circleCounter;

    int _totalGeometry;
    int _goodGeometry;
    public Text _mission;
    private bool readText;
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
       
        _mission.text = GeometryNumberText(_nCircle, "círculo")+  GeometryNumberText(_nTriangle, "triángulo")+ GeometryNumberText(_nSquare, "cuadrado");
    }

    public string GeometryNumberText(int nGeometry,string geometryName)
    {
        if (nGeometry==0)
        {
            return "";
        }
        else if (nGeometry>1)
        {
            return nGeometry+" "+geometryName + "s ";
        }
        else
        {

            return nGeometry+" "+ geometryName+" ";
        }
    }
    public void Compare()
    {
        CheckGeometry(_nCircle, _circleCounter);
        CheckGeometry(_nSquare, _squareCounter);
        CheckGeometry(_nTriangle, _triangleCounter);
        EDebug.Log(_goodGeometry+"/"+_totalGeometry);
        if (_goodGeometry==_totalGeometry)
        {
            EDebug.Log("Bien hecho");
        }
        else
        {
            _totalGeometry = 0;
            _goodGeometry = 0;
        }
    }

    public void CheckGeometry(int nGeometry, int counter)
    {
        if (nGeometry > 0)
        {
            _totalGeometry += 1;
            if (nGeometry == counter)
            {
                _goodGeometry += 1;
            };
        }
       
    }
    public void CounterSquare()
    {
        _squareCounter ++;
    }
    public void CounterTriangle()
    {
        _triangleCounter++;
    }
    public void CounterCircle()
    {
        _circleCounter++;
    }
}
