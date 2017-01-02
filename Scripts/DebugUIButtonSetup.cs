using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUIButtonSetup : MonoBehaviour {

    public void PlayerOne()
    {
        GameManagerMultiplayer.instance.ChangeGameStateMultiPlayerOnePlanet();
    }	

    public void PlayerTwo()
    {
        GameManagerMultiplayer.instance.ChangeGameStateMultiPlayerTwoPlanet();
    }

    public void Neutral()
    {
        GameManagerMultiplayer.instance.ChangeGameStateMultiNeutral();
    }
	
}
