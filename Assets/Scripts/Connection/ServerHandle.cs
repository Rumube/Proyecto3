using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle { 

    public static void ContinueGameTime()
    {
        ServiceLocator.Instance.GetService<MobileUI>()._continueGameTimer.interactable = true;
    }
}
