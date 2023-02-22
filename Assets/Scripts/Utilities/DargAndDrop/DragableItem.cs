using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragableItem : MonoBehaviour
{
    [Header("References")]
    public Transform _initialParent = null;
    private DragManager _dndManager;
    [SerializeField]
    private Transform _target;
    private bool _isReturning = false;
    private float _returnVelocity = 0f;
    private bool _isCreated = false;
    [Header("DragConfiguration")]
    public bool _onlyVertical;
    private GameObject _container;

    private void Start()
    {
        SetInitialTarget();
        if(_initialParent == null)
        {
            _initialParent = transform.GetComponentInParent<Transform>();
        }
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
    /// <summary>
    /// Set the manager
    /// </summary>
    /// <param name="manager">Manager</param>
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
    /// <summary>
    /// Set the dragable items properties
    /// </summary>
    /// <param name="initialParent">Inital Parent</param>
    public void SetProperties(Transform initialParent)
    {
        _isCreated = true;
        _initialParent = initialParent;
        SetInitialTarget();
    }
    /// <summary>
    /// Set a new target to move
    /// </summary>
    /// <param name="newTrasnform">New target</param>
    public void SetNewTarget(Transform newTrasnform)
    {
        _target = newTrasnform;
    }
    /// <summary>
    /// Deactivate the rigidbody so that the item falls into the void.
    /// </summary>
    public void FallItem()
    {
        if (_isCreated)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DragParent")
        {
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
            collision.gameObject.GetComponent<DragParentPropieties>().removeItem(gameObject);
            _dndManager.OnItemExit();
            SetInitialTarget();
        }
    }
    /// <summary>
    /// Puts the current target as a parent
    /// </summary>
    public void SetInParent()
    {
        transform.SetParent(_target);
    }
    /// <summary>
    /// Returns the current target
    /// </summary>
    /// <returns><see cref="_target"/></returns>
    public Transform GetTarget()
    {
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
        Destroy(gameObject);
    }
    /// <summary>
    /// Initializes the variable <see cref="_container"/> with the given container
    /// </summary>
    /// <param name="container">The new container</param>
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
}
