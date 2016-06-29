using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerManager : NetworkBehaviour {

    public GameObject barrierIndicator;
    public GameObject mainCamera;

    void Awake () {
        //Instantiate barrier
        barrierIndicator = (GameObject)Instantiate(barrierIndicator, new Vector3(1000, 0, 0), Quaternion.identity);

        //Set up main camera player reference
        mainCamera = GameObject.FindGameObjectWithTag(_Tags.mainCamera);
        mainCamera.GetComponent<SmoothCameraPlanet>().player = transform;
        mainCamera.GetComponent<SmoothCameraSpace>().player = transform;

    }
  
}
