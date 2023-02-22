using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IFrogMessage
{
    /// <summary>
    /// Start the messages process
    /// </summary>
    /// <param name="message">The message text</param>
    public void NewFrogMessage(string message);
    /// <summary>
    /// Start the messages process and save the message in <see cref="_lastOrder"/>
    /// </summary>
    /// <param name="message">The message text</param>
    /// <param name="isOrder">The message is an order</param>
    public void NewFrogMessage(string message, bool isOrder);
    /// <summary>
    /// Repeat the last order gived
    /// </summary>
    public void RepeatLastOrder();
    /// <summary>
    /// Stop the message
    /// </summary>
    public void StopFrogSpeaker();
    /// <summary>
    /// Returns if there are a message activated
    /// </summary>
    /// <returns><see cref="FrogMessage._messageAtive"/></returns>
    public bool GetMessageAtive();
}
