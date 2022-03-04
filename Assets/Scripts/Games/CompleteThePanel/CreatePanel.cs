using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePanel : MonoBehaviour
{
    public int _column;
    public int _row;
    public int _gap;
 
    public GameObject _geometry;
    void Start()
    {
        for (int x = 0; x < _column; x++)
        {
            for (int y = 0; y < _row; y++)
            {
              
                Instantiate(_geometry, new Vector3(x, y, 0) * _gap, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
