using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragableItem : MonoBehaviour
{
    [Header("Item parameters")]
    private Geometry.Geometry_Type _geometry_type;
    [SerializeField] private List<Sprite> _gearsSprite = new List<Sprite>();

    [Header("References")]
    public Transform _initialParent = null;
    private DragManager _dndManager;
    [SerializeField]
    private Transform _target;
    private bool _isReturning = false;
    private float _returnVelocity = 0f;
    private bool _isCreated = false;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [Header("DragConfiguration")]
    public bool _onlyVertical;
    private GameObject _container;
    [SerializeField] private bool _dragable = true;
    private bool _isFallen = false;

    private void Start()
    {
        if (_dragable)
        {
            SetInitialTarget();
            if (_initialParent == null)
            {
                _initialParent = transform.GetComponentInParent<Transform>();
            }
        }
        else
        {
            tag = "Untagged";
        }
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_isReturning && _target != null)
        {
            if(_initialParent != _target)
            {
                _initialParent = _target;
                SetInParent();
            }
            transform.position = Vector2.Lerp(transform.position, _target.position, _returnVelocity * Time.deltaTime);
            if (transform.position == _target.position)
            {
                _isReturning = false;
            }
        }
    }
    public void SetDndManager(DragManager manager)
    {
        _dndManager = manager;
    }
    /// <summary>
    /// Init the process to move the item to the target parent
    /// </summary>
    /// <param name="velocity">Movement speed</param>
    public void ReturnToTarget(float velocity)
    {
        if (_isCreated)
        {
            FallItem();
        }
        else
        {
            _isReturning = true;
            _returnVelocity = velocity;
        }
    }

    public void SetProperties(Transform initialParent)
    {
        _isCreated = true;
        _initialParent = initialParent;
        SetInitialTarget();
    }
    public void SetNewTarget(Transform newTrasnform)
    {
        _target = newTrasnform;
    }


    public void FallItem()
    {
        if (_isCreated)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            _isFallen = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DragParent")
        {
            print("Enter: " + collision.name);

            if (collision.gameObject.GetComponent<DragParentPropieties>().canAddItem(gameObject))
            {
                if(_dndManager == null)
                {
                    _dndManager = GameObject.FindGameObjectWithTag("DNDManager").GetComponent<DragManager>();
                }
                _dndManager.OnItemEnter(collision);
                SetNewTarget(collision.transform);
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DragParent")
        {
            print("Exit: " + collision.name);

            collision.gameObject.GetComponent<DragParentPropieties>().removeItem(gameObject);
            _dndManager.OnItemExit();
            SetInitialTarget();
        }
    }
    public void SetInParent()
    {
        transform.SetParent(_target);
    }
    public Transform GetTarget()
    {
        print("GET TARGET:" + _target);
        return _target;
    }
    /// <summary>
    /// Set <see cref="_target"/> equals to <see cref="_initialParent"/>
    /// </summary>
    public void SetInitialTarget()
    {
        SetNewTarget(_initialParent);
    }
    private void OnBecameInvisible()
    {
        if (_isCreated && _isFallen)
        {
            Destroy(gameObject);
        }
    }

    public void InitContainer(GameObject container)
    {
        _container = container;
    }

    public void InitDragContainer()
    {
        if(_container != null)
        {
            //ANIM CLOSE
        }
    }

    public void SetDragable(bool dragable)
    {
        _dragable = dragable;
        if (_dragable)
        {
            tag = "DragObject";
        }
        else
        {
            tag = "Untagged";
        }
    }
    public void SetGeometry(Geometry.Geometry_Type geometry_Type)
    {
        _geometry_type = geometry_Type;
        SetSprite();
    }
    private void SetSprite()
    {
        if(_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        switch (_geometry_type)
        {
            case Geometry.Geometry_Type.circle:
                _spriteRenderer.sprite = _gearsSprite[0];
                break;
            case Geometry.Geometry_Type.triangle:
                _spriteRenderer.sprite = _gearsSprite[1];
                break;
            case Geometry.Geometry_Type.square:
                _spriteRenderer.sprite = _gearsSprite[2];
                break;
            case Geometry.Geometry_Type.diamond:
                _spriteRenderer.sprite = _gearsSprite[3];
                break;
            case Geometry.Geometry_Type.pentagon:
                _spriteRenderer.sprite = _gearsSprite[4];
                break;
            case Geometry.Geometry_Type.hexagon:
                _spriteRenderer.sprite = _gearsSprite[5];
                break;
        }
    }
    public Geometry.Geometry_Type GetGeometry()
    {
        return _geometry_type;
    }public void SetCreated(bool value)
    {
        _isCreated = value;
    }
}
