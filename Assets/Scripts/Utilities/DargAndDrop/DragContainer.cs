using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragContainer : MonoBehaviour
{
    [Header("References")]
    public GameObject _parentGenerator;
    public List<GEOMETRY_GEARS> _geometryList = new List<GEOMETRY_GEARS>();

    public enum GEOMETRY_GEARS
    {
        circulo,
        triangulo,
        cuadrado,
        rombo,
        pentagono,
        hexagono
    }
    public GEOMETRY_GEARS _posibleGeometry = GEOMETRY_GEARS.circulo;
    private List<GameObject> _parentList = new List<GameObject>();
    private void Start()
    {
        StartCoroutine(GenerateParents(_geometryList));
    }

    public IEnumerator GenerateParents(List<GEOMETRY_GEARS> geometry)
    {
        yield return new WaitForSeconds(0.5f);
        float distPos = 1;
        float distNeg = -1;
        bool isPar = false;
        if(geometry.Count%2 == 0)
        {
            isPar = true;
        }
        if (isPar)
        {
            for (int i = 0; i < geometry.Count; i++)
            {
                GameObject newParent = Instantiate(_parentGenerator, transform);
                newParent.transform.localScale = new Vector2(0.5f, 0.5f);

                if (i == 0)
                {
                    newParent.transform.localPosition = new Vector2(distPos, 2);
                }else if(i == 1)
                {
                    newParent.transform.localPosition = new Vector2(distNeg, 2);
                }
                else if (i % 2 == 0)
                {
                    distPos += 2;
                    newParent.transform.localPosition = new Vector2(distPos, 2);
                }
                else if (i % 2 != 0)
                {
                    distNeg -= 2;
                    newParent.transform.localPosition = new Vector2(distNeg, 2);
                }
                newParent.GetComponent<DragParentGeneration>().SetGeometryGenerated(geometry[i]);
                _parentList.Add(newParent);
            }
        }
        else
        {
            for (int i = 0; i < geometry.Count; i++)
            {
                GameObject newParent = Instantiate(_parentGenerator, transform);
                newParent.transform.localScale = new Vector2(0.5f, 0.5f);
                if(i == 0)
                {
                    newParent.transform.localPosition = new Vector2(0, 2);
                }else if (i % 2 == 0)
                {
                    distPos += 2;
                    newParent.transform.localPosition = new Vector2(distPos, 2);
                }else if(i%2 != 0)
                {
                    distNeg -= 2;
                    newParent.transform.localPosition = new Vector2(distNeg, 2);
                }
                newParent.GetComponent<DragParentGeneration>().SetGeometryGenerated(geometry[i]);
                _parentList.Add(newParent);
            }
        }
    }
}
