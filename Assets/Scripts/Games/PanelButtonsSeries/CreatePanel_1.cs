using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePanel_1 : MonoBehaviour
{
    [Header("Table Configuration")]
    public int _column;
    public int _row;
    public int _gapX;
    public int _gapY;
    public float _offsetX;
    public float _offsetY;

    public GameObject _geometrySpawn;
    [Header("Geometry")]
    public GameObject[] _geometryForms;
    private ButtonCounter_1 _buttomCounter;
    public GameObject _GeometryButtons;
    public List<GameObject> _modifiedList = new List<GameObject>();
    //Game Configuration
    [SerializeField]
    private int _level;

    [SerializeField]
    CompleteThePanelDifficulty_1 _completeThePanel;
    CompleteThePanelDifficulty_1.dataDiffilcuty _currentDataDifficulty;


    void Start()
    {
        _completeThePanel = GetComponent<CompleteThePanelDifficulty_1>();

        _currentDataDifficulty = _completeThePanel.GenerateDataDifficulty(_level);
        _buttomCounter = GetComponent<ButtonCounter_1>();
        ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
        GeneratePanel();
    }

    /// <summary>
    /// Creates a panel with geometry.
    /// </summary>
    void GeneratePanel()
    {
        _modifiedList.Clear();
        


        for (int y = 0; y < _row; y++)
        {
            Geometry.Geometry_Type _var1;
            do
            {
                _var1 = (Geometry.Geometry_Type)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Geometry.Geometry_Type)).Length);

            } while (_var1 == Geometry.Geometry_Type.hexagon || _var1 == Geometry.Geometry_Type.rectangle);

            Geometry.Geometry_Type _var2;
            do
            {
                _var2 = (Geometry.Geometry_Type)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Geometry.Geometry_Type)).Length);

            } while (_var2 == Geometry.Geometry_Type.hexagon || _var2 == Geometry.Geometry_Type.rectangle || _var1 == _var2);

            for (int x = 0; x < _column; x++)
            {
                GameObject newGeometry;

                newGeometry = Instantiate(_GeometryButtons, _geometrySpawn.transform);
                print("col: " + y + " / row: " + x);
                if (x % 2 == 0)
                {
                    newGeometry.GetComponent<Geometry>()._geometryType = _var1;
                }
                else
                {
                    newGeometry.GetComponent<Geometry>()._geometryType = _var2;

                }
                newGeometry.GetComponent<ButtonCounter_1>().SetTrueGeometry(newGeometry.GetComponent<Geometry>()._geometryType);
                newGeometry.transform.localPosition = new Vector3(x, y, 0);
                _modifiedList.Add(newGeometry);
                if (x == 0 || x == 1 || x == 2)
                {
                    newGeometry.GetComponent<ButtonCounter_1>().EnableButtons(false);
                }
            }
        }
        ModifyGeomtry();
        Invoke("SendMessage", 1f);
    }
    public void ModifyGeomtry()
    {
        Geometry.Geometry_Type _var1;
        Geometry.Geometry_Type _var2;

        do
        {
            _var1 = (Geometry.Geometry_Type)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Geometry.Geometry_Type)).Length);

        } while (_var1 == Geometry.Geometry_Type.hexagon || _var1 == Geometry.Geometry_Type.rectangle);
        do
        {
            _var2 = (Geometry.Geometry_Type)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Geometry.Geometry_Type)).Length);

        } while (_var2 == Geometry.Geometry_Type.hexagon || _var2 == Geometry.Geometry_Type.rectangle);

        _modifiedList[UnityEngine.Random.Range(3, 6)].GetComponent<Geometry>()._geometryType = _var1;
        _modifiedList[UnityEngine.Random.Range(10, 13)].GetComponent<Geometry>()._geometryType = _var2;
    }

    public void CheckGeometry() 
    {
        bool correct = true;
        for (int i = 0; i < _modifiedList.Count; i++)
        {
            if (_modifiedList[i].GetComponent<ButtonCounter_1>().GetTrueGeometry() != _modifiedList[i].GetComponent<Geometry>()._geometryType)
            {
                correct = false;
            }
        }
        if (correct)
        {
            ServiceLocator.Instance.GetService<IPositive>().GenerateFeedback(Vector2.zero);
            Restart();
        }
        else
        {
            ServiceLocator.Instance.GetService<IError>().GenerateError();
        }
        
    }
    private void SendMessage()
    {
        ServiceLocator.Instance.GetService<IFrogMessage>().NewFrogMessage(_buttomCounter._GetTextGame(), true);
    }

   
    public void Restart()
    {
        foreach (GameObject geometry in _modifiedList)
        {
            Destroy(geometry);
        }
        GeneratePanel();
    }
}



