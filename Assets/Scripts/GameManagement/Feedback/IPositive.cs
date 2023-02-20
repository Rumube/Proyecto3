using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPositive
{
    /// <summary>
    /// Start the feedback actions
    /// </summary>
    /// <param name="initPosition">Position to create the feedback</param>
    public void GenerateFeedback(Vector2 initPosition);
}
