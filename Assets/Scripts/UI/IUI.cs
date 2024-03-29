using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUI
{
    /// <summary>Quit the game</summary>
    public void QuitGame();
    /// <summary>Open/close the window credits</summary>
    public void CreditsMobile();
    /// <summary>
    /// Open/Close the window Tutorial
    /// </summary>
    public void TutorialMobile();
    /// <summary>Open/close the window credits</summary>
    public void CreditsTablet();
    /// <summary>Open the next window deppending on the position of the array and close the previous one</summary>
    public void OpenNextWindow();
    /// <summary>Open the previous window deppending on the position of the array and close the current one</summary>
    public void OpenPreviousWindow();

}
