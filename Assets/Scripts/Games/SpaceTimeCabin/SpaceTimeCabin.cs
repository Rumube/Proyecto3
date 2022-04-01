using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceTimeCabin : MonoBehaviour
{
    [Header("Game Configuration")]
    public Vector2 _targetPoint;

    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
