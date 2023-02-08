using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDeposit : MonoBehaviour
{
    [Header("References")]
    public GameObject _parentGenerator;
    public List<GEOMETRY_GEARS_PIECES> _geometryList = new List<GEOMETRY_GEARS_PIECES>();

    public enum GEOMETRY_GEARS_PIECES
    {
        circulo_arriba_dr,
        circulo_arriba_izq,
        circulo_abajo_izq,
        circulo_abajo_dr,
        triangulo_abajo_dr,
        triangulo_arriba_dr,
        triangulo_arriba_izq,
        triangulo_abajo_izq,
        cuadrado_abajo_izq,
        cudrado_arriba_izq,
        cuadrado_arriba_dr,
        cuadrado_abajo_dr,
        rombo_abajo_dr,
        rombo_arriba_dr,
        rombo_arriba_izq,
        rombo_abajo_izq,
        pentagono_abajo_izq,
        pentagono_arriba_izq,
        pentagono_arriba_dr,
        pentagono_abajo_dr,
        hexagono_abajo_dr,
        hexagono_arriba_dr,
        hexagono_arriba_izq,
        hexagono_abajo_izq
    }
    public GEOMETRY_GEARS_PIECES _posibleGeometry = GEOMETRY_GEARS_PIECES.circulo_arriba_dr;
    private List<GameObject> _parentList = new List<GameObject>();
    private void Start()
    {
        StartCoroutine(GenerateParents(_geometryList));
    }

    public IEnumerator GenerateParents(List<GEOMETRY_GEARS_PIECES> geometry)
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
                newParent.GetComponent<DragParentGenerationPieces>().SetGeometryGenerated(geometry[i]);
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
                newParent.GetComponent<DragParentGenerationPieces>().SetGeometryGenerated(geometry[i]);
                _parentList.Add(newParent);
            }
        }
    }
}
