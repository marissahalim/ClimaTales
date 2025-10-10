using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.MUIP;

public class FCPTimeBtn : MonoBehaviour
{

    // SCENARIOS
    public ButtonManager bestCase;
    public ButtonManager worstCase;

    //CONTENT
    public ButtonManager absChange;
    public ButtonManager value;


    public void DisableEverythingElse()
    {
        bestCase.Interactable(false);
        worstCase.Interactable(false);
        absChange.Interactable(false);
        value.Interactable(false);
    }

    public void EnableEverythingElse()
    {
        bestCase.Interactable(true);
        worstCase.Interactable(true);
        absChange.Interactable(true);
        value.Interactable(true);
    }
}
