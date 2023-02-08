using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    [Header("Parents")]
    private Transform _dragParent;

    [Header("Conditions")]
    public LayerMask _layer;
    [SerializeField]
    private bool _inDragParent = false;

    [Header("References")]
    public GameObject _itemDraging;

    [Header("Properties")]
    public float _returnVelocity = 3;


    // Start is called before the first frame update
    void Start()
    {
        _dragParent = transform;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
    }
    /// <summary>
    /// Detect the input and the GameObject 
    /// the player interact
    /// </summary>
    private void UpdateInput()
    {
        if (ServiceLocator.Instance.GetService<IGameManager>().GetClientState() == IGameManager.GAME_STATE_CLIENT.playing)
        {
            AndroidInputAdapter.Datos newInput = ServiceLocator.Instance.GetService<IInput>().InputTouch();
            if (newInput.phase != TouchPhase.Canceled)
            {
                switch (newInput.phase)
                {
                    case TouchPhase.Began:
                        OnClick(newInput);
                        break;
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        OnDrag(newInput);
                        break;
                    case TouchPhase.Ended:
                        onDrop(newInput);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    /// <summary>
    /// Get the pint where user click and add the
    /// GameObject to <see cref="_itemDraging"/> if has
    /// the tag "DragObject"
    /// </summary>
    /// <param name="touch">Input data <see cref="AndroidInputAdapter.Datos"/></param>
    private void OnClick(AndroidInputAdapter.Datos touch)
    {
        Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.pos);
        Collider2D target = Physics2D.OverlapCircle(touchPos, 0.1f, _layer);

        if (target != null && target.transform.gameObject.tag == "DragObject")
        {
            _itemDraging = target.transform.gameObject;
            _itemDraging.transform.SetParent(_dragParent);
            _itemDraging.GetComponent<DragableItem>().SetDndManager(this);
        }
    }
    /// <summary>
    /// Update the item's position using the <see cref="AndroidInputAdapter.Datos.pos"/>
    /// </summary>
    /// <param name="touch">The input data <see cref="AndroidInputAdapter.Datos"/></param>
    private void OnDrag(AndroidInputAdapter.Datos touch)
    {
        if (_itemDraging != null)
        {
            _itemDraging.GetComponent<DragableItem>().InitDragContainer();
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.pos);
            Vector3 newPos = Vector3.zero;
            //SET X
            if (_itemDraging.GetComponent<DragableItem>()._onlyVertical)
            {
                newPos = new Vector3(_itemDraging.transform.position.x, touchPos.y, 0);
            }
            else
            {
                newPos = new Vector3(touchPos.x, touchPos.y, 0);
            }
            //SET Y

            _itemDraging.transform.position = newPos;
        }
    }
    /// <summary>
    /// Checks if the item can put in the current position
    /// if can't, return to the last parent
    /// </summary>
    /// <param name="touch">The input data <see cref="AndroidInputAdapter.Datos"/></param>
    private void onDrop(AndroidInputAdapter.Datos touch)
    {
        if (_itemDraging != null)
        {
            if (_inDragParent)//IN PARENT
            {
                Transform target = _itemDraging.GetComponent<DragableItem>().GetTarget();
                if (!target.GetComponent<DragParentPropieties>().CheckCanPut(_itemDraging.transform))
                {
                    _itemDraging.GetComponent<DragableItem>().SetInitialTarget();
                    SetReturning();
                }
                else
                {
                    _itemDraging.transform.parent = target;
                    _itemDraging.transform.position = target.position;
                    _itemDraging = null;
                }
            }
            else
            {
                _itemDraging.GetComponent<DragableItem>().SetInParent();
                SetReturning();
            }
        }
    }
    /// <summary>
    /// Set the values to return
    /// </summary>
    private void SetReturning()
    {
        _itemDraging.GetComponent<DragableItem>().ReturnToTarget(_returnVelocity);
        _itemDraging = null;
    }

    public void OnItemEnter(Collider2D collision)
    {
        _inDragParent = true;
    }

    public void OnItemExit()
    {
        _inDragParent = false;
    }
}
