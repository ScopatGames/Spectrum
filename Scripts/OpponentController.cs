using UnityEngine;
using System.Collections;

public class OpponentController : MonoBehaviour {

    public GameObject spaceContainer;
    public GameObject defensiveContainer;
    public GameObject offensiveContainer;

    public void EnableSpaceContainer()
    {
        spaceContainer.SetActive(true);
        defensiveContainer.SetActive(false);
        offensiveContainer.SetActive(false);
    }

    public void EnableDefensiveContainer()
    {
        spaceContainer.SetActive(false);
        defensiveContainer.SetActive(true);
        offensiveContainer.SetActive(false);
    }

    public void EnableOffensiveContainer()
    {
        spaceContainer.SetActive(false);
        defensiveContainer.SetActive(false);
        offensiveContainer.SetActive(true);
    }
}
