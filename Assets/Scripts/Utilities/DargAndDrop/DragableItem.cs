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

    private void Start()
    {
        _target = _initialParent;
        if(_initialParent == null)
        {
            _initialParent = transform.GetComponentInParent<Transform>();
        }
    }

    private void Update()
    {
        if (_isReturning)
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
        _isReturning = true;
        _returnVelocity = velocity;
        FallItem();
    }

    public void SetProperties()
    {
        _isCreated = true;
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
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DragParent")
        {
            if (collision.gameObject.GetComponent<DragParentPropieties>().canAddItem(gameObject))
            {
                _dndManager.OnItemEnter(collision);
                SetNewTarget(collision.transform);
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        print("Exit: " + collision.name);
        if (collision.gameObject.tag == "DragParent")
        {
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
        return _target;
    }
    /// <summary>
    /// Set <see cref="_target"/> equals to <see cref="_initialParent"/>
    /// </summary>
    public void SetInitialTarget()
    {
        _target = _initialParent;
    }
}
