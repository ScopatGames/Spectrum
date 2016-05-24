using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PickupCounter : MonoBehaviour {
    public int targetNumber;
    public int levelToLoad;
    private int counter = 0;
    	
	

    public void incrementCounter()
    {
        counter++;

        if(counter == targetNumber)
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }

}
