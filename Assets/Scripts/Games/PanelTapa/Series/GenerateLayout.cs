using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLayout : MonoBehaviour
{
    [Header("Dificulty parameters")]
    [Range(6, 9)]
    [SerializeField] private int _size;
    [SerializeField] private List<Geometry.Geometry_Type> _posibleGeometry = new List<Geometry.Geometry_Type>();
    [SerializeField] private int _errorNum;

    [Header("Scene references")]
    [SerializeField] private GameObject _layoutParent;
    [SerializeField] private DragContainer _container;
    [SerializeField] private GameObject _baseGear;

    [Header("Game references")]
    private List<Geometry.Geometry_Type> _correctSerie = new List<Geometry.Geometry_Type>();
    private List<Geometry.Geometry_Type> _userSerie = new List<Geometry.Geometry_Type>();

    private void Start()
    {
        InitLayout();
    }
    private void InitLayout()
    {
        StartCoroutine(GenerateParents());
        //for (int i = 0; i < _size; i++)
        //{
        //    GameObject newGear = Instantiate(_baseGear, _layoutParent.transform);
        //    _correctSerie.Add(newGear.GetComponent<DragableItem>().GetGeometry());
        //    newGear.GetComponent<DragableItem>().SetDragable(false);
        //    newGear.GetComponent<DragableItem>().SetGeometry(GenerateRandomGeometry());
        //    newGear.GetComponent<DragableItem>().SetCreated(false);
        //}
    }

    public IEnumerator GenerateParents()
    {
        yield return new WaitForSeconds(0.5f);
        float distPos = 1;
        float distNeg = -1;
        bool isPar = false;
        if (_size % 2 == 0)
        {
            isPar = true;
        }
        if (isPar)
        {
            for (int i = 0; i < _size; i++)
            {
                GameObject newGear = Instantiate(_baseGear, _layoutParent.transform);
                newGear.transform.localScale = new Vector2(0.5f, 0.5f);

                if (i == 0)
                {
                    newGear.transform.localPosition = new Vector2(distPos, 2);
                }
                else if (i == 1)
                {
                    newGear.transform.localPosition = new Vector2(distNeg, 2);
                }
                else if (i % 2 == 0)
                {
                    distPos += 2;
                    newGear.transform.localPosition = new Vector2(distPos, 2);
                }
                else if (i % 2 != 0)
                {
                    distNeg -= 2;
                    newGear.transform.localPosition = new Vector2(distNeg, 2);
                }
                newGear.GetComponent<DragableItem>().SetDragable(false);
                newGear.GetComponent<DragableItem>().SetGeometry(GenerateRandomGeometry());
                newGear.GetComponent<DragableItem>().SetCreated(false);
            }
        }
        else
        {
            for (int i = 0; i < _size; i++)
            {
                GameObject newGear = Instantiate(_baseGear, _layoutParent.transform);
                newGear.transform.localScale = new Vector2(0.5f, 0.5f);
                if (i == 0)
                {
                    newGear.transform.localPosition = new Vector2(0, 2);
                }
                else if (i % 2 == 0)
                {
                    distPos += 2;
                    newGear.transform.localPosition = new Vector2(distPos, 2);
                }
                else if (i % 2 != 0)
                {
                    distNeg -= 2;
                    newGear.transform.localPosition = new Vector2(distNeg, 2);
                }
                newGear.GetComponent<DragableItem>().SetDragable(false);
                newGear.GetComponent<DragableItem>().SetGeometry(GenerateRandomGeometry());
                newGear.GetComponent<DragableItem>().SetCreated(false);
            }
        }
    }

    private Geometry.Geometry_Type GenerateRandomGeometry()
    {
        return _posibleGeometry[Random.Range(0, _posibleGeometry.Count - 1)];
    }
}
