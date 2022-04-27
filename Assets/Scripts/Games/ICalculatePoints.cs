using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICalculatePoints
{
    /// <summary>Calculates the puntuation</summary>
    /// <param name="success">Number of success</param>
    /// <param name="fails">Number of fails</param>
    public void Puntuation(int success, int fails);

    public CalculatePuntuation.Average GetAverage();
}
