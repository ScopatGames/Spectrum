using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerCamera : NetworkBehaviour {

    public Camera mainCamera;

    private SmoothCameraPlanet smoothCameraPlanet;
    private SmoothCameraSpace smoothCameraSpace;

	void Awake () {
        if (isLocalPlayer)
        {
            smoothCameraPlanet = GetComponent<SmoothCameraPlanet>();
            smoothCameraSpace = GetComponent<SmoothCameraSpace>();

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
