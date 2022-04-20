using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IFrogMessage
{
    public void NewFrogMessage(string message);
    public void NewFrogMessage(string message, bool isOrder);
    public void RepeatLastOrder();
}
