using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragParentGeneration : DragParentPropieties
{
    [Header("Properties")]
    public GameObject _dragItem;
    public DragManager _dndManager;
    public DragContainer.GEOMETRY_GEARS _generatedGeometry = DragContainer.GEOMETRY_GEARS.circulo;
    public List<Sprite> _sprite = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
                case DragContainer.GEOMETRY_GEARS.circulo:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[0];
                    break;
                case DragContainer.GEOMETRY_GEARS.triangulo:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[1];
                    break;
                case DragContainer.GEOMETRY_GEARS.cuadrado:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[2];
                    break;
                case DragContainer.GEOMETRY_GEARS.rombo:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[3];
                    break;
                case DragContainer.GEOMETRY_GEARS.pentagono:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[4];
                    break;
                case DragContainer.GEOMETRY_GEARS.hexagono:
                    newItem.GetComponent<SpriteRenderer>().sprite = _sprite[5];
                    break;
                default:
                    break;
            }

        }
    }

    public void SetGeometryGenerated(DragContainer.GEOMETRY_GEARS geometry)
    {
        _generatedGeometry = geometry;
    }
}
