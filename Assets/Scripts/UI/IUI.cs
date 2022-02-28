using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUI
{
    public void QuitGame();
    public void Credits();
    public abstract void OpenNextWindow();
    public abstract void OpenPreviousWindow();
}
