using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeConfiguration : MonoBehaviour
{
    [SerializeField]
    private float _maxTime;
    private float _currentTime;

    /*
     * @desc Playing time begins
     * **/
    public void StartGameTime()
    {
        _currentTime = _maxTime;
        StartCoroutine(TimeProgress());
    }

    /*
     * @desc Reduce the time of the game until it reaches 0
     * **/
    IEnumerator TimeProgress()
    {
        do
        {
            yield return new WaitForSeconds(1f);
            _currentTime--;
            // TODO: Update UI
        } while (_currentTime > 0f);
        // TODO: Send notice of game over
    }
}
