using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceTimeCabin : MonoBehaviour
{
    [Header("Game Configuration")]
    public Vector2 _targetPoint;
    public int _level;
    SpaceTimeCabinDifficulty.dataDiffilcuty _dataDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.Instance.GetService<IGameTimeConfiguration>().StartGameTime();
        _dataDifficulty = GetComponent<SpaceTimeCabinDifficulty>().GenerateDataDifficulty(_level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Restart the game
    /// </summary>
    void RestartGame()
    {

    }

}
