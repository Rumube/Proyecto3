using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragParentGenerationPieces : DragParentPropieties
{
    [Header("Properties")]
    public GameObject _dragItem;
    public DragManager _dndManager;
    public DragDeposit.GEOMETRY_GEARS_PIECES _generatedGeometry = DragDeposit.GEOMETRY_GEARS_PIECES.circulo_arriba_dr;
    public List<Sprite> _sprite = new List<Sprite>();

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount <= 0)
        {
            GameObject newItem = Instantiate(_dragItem, transform);
            newItem.transform.position = transform.position;
            newItem.GetComponent<DragableItem>().SetProperties(transform);
            newItem.GetComponent<DragableItem>().SetDndManager(_dndManager);
            newItem.GetComponent<Animator>().Play("GetBigger_Anim");

            switch (_generatedGeometry)
            {
                case DragDeposit.GEOMETRY_GEARS_PIECES.circulo_arriba_dr:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[0];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.circulo_arriba_izq:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[1];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.circulo_abajo_izq:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[2];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.circulo_abajo_dr:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[3];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.triangulo_abajo_dr:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[4];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.triangulo_arriba_dr:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[5];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.triangulo_arriba_izq:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[6];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.triangulo_abajo_izq:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[7];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.cuadrado_abajo_izq:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[8];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.cudrado_arriba_izq:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[9];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.cuadrado_arriba_dr:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[10];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.cuadrado_abajo_dr:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[11];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.rombo_abajo_dr:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[12];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.rombo_arriba_dr:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[13];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.rombo_arriba_izq:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[14];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.rombo_abajo_izq:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[15];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.pentagono_abajo_izq:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[16];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.pentagono_arriba_izq:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[17];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.pentagono_arriba_dr:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[18];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.pentagono_abajo_dr:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[19];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.hexagono_abajo_dr:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[20];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.hexagono_arriba_dr:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[21];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.hexagono_arriba_izq:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[22];
                    break;
                case DragDeposit.GEOMETRY_GEARS_PIECES.hexagono_abajo_izq:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[23];
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// Change the value of <see cref="_generatedGeometry"/>
    /// </summary>
    /// <param name="geometry">The new <see cref="DragContainer.GEOMETRY_GEARS"/></param>
    public void SetGeometryGenerated(DragDeposit.GEOMETRY_GEARS_PIECES geometry)
    {
        _generatedGeometry = geometry;
    }
}
