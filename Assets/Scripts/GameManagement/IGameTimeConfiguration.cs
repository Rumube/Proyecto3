using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameTimeConfiguration
{
    /// <summary>
    /// Playing time begins.
    /// </summary>
    public void StartGameTime();
    /// <summary>
    /// Set the value of <see cref="_canStartTime"/>
    /// </summary>
    /// <param name="state">The new value</param>
    public void SetStartTime(bool state);
    /// <summary>
    /// Returns the value of <see cref="_currentTime"/>
    /// </summary>
    /// <returns><see cref="GameTimeConfiguration._currentTime"/></returns>
    public float GetCurrentTime();
    /// <summary>
    /// Time at which the current game will end
    /// </summary>
    /// <returns><see cref="GameTimeConfiguration._finishTime"/></returns>
    public float GetFinishTime();
}
