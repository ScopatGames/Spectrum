using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerCamera : NetworkBehaviour {

    public GameObject mainCamera;

    private SmoothCameraPlanet smoothCameraPlanet;
    private SmoothCameraSpace smoothCameraSpace;

    void Start() {
        if (isLocalPlayer)
        {
            smoothCameraPlanet = mainCamera.GetComponent<SmoothCameraPlanet>();
            smoothCameraSpace = mainCamera.GetComponent<SmoothCameraSpace>();
            mainCamera.transform.parent = null;
            mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -200f);

            EnableCameraSpace();
        }
        else
        {
            Destroy(mainCamera);
        }
    }

    public void EnableCameraSpace()
    {
        if (isLocalPlayer)
        {
            smoothCameraPlanet.enabled = false;
            smoothCameraSpace.enabled = true;
        }
    }

    public void EnableCameraPlanet()
    {
        if (isLocalPlayer)
        {
            smoothCameraSpace.enabled = false;
            smoothCameraPlanet.enabled = true;
        }
    }
}
