using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IFrogMessage
{
    public void NewFrogMessage(string message, float time, Text text);
}
