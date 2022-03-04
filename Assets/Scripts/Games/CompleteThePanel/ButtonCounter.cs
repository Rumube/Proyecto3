using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCounter : MonoBehaviour
{
    public int _square;
    public int _triangle;
    public int _circle;
    void Start()
    {
     _square=0;
     _triangle=0;
     _circle=0;
}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CounterSquare()
    {
        _square ++;
    }
    public void CounterTriangle()
    {
        _triangle++;
    }
    public void CounterCircle()
    {
        _circle++;
    }
}
