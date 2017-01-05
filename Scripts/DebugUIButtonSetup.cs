using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUIButtonSetup : MonoBehaviour {

    public void PlayerOne()
    {
        GameManagerMultiplayer.instance.CmdChangeGameStateMultiPlayerOnePlanet();
    }	

    public void PlayerTwo()
    {
        GameManagerMultiplayer.instance.CmdChangeGameStateMultiPlayerTwoPlanet();
    }

    public void Neutral()
    {
        GameManagerMultiplayer.instance.CmdChangeGameStateMultiNeutral();
    }
	
}
