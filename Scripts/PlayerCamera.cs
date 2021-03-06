﻿using UnityEngine;
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
            smoothCameraPlanet.target = null;
            smoothCameraSpace = mainCamera.GetComponent<SmoothCameraSpace>();
            smoothCameraSpace.target = null;
            mainCamera.transform.parent = null;
            mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -200f);
            mainCamera.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else
        {
            Destroy(mainCamera);
        }
    }

    public void EnableSpaceCamera()
    {
        if (isLocalPlayer)
        {
            smoothCameraPlanet.enabled = false;
            smoothCameraSpace.enabled = true;
            smoothCameraSpace.target = transform;
            mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -200);
            mainCamera.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void EnablePlanetCameraAttacker()
    {
        if (isLocalPlayer)
        {
            smoothCameraSpace.enabled = false;
            smoothCameraPlanet.enabled = true;
            smoothCameraPlanet.target = transform;
        }
    }

    public void EnablePlanetCameraDefender()
    {
        if (isLocalPlayer)
        {
            smoothCameraSpace.enabled = false;
            smoothCameraPlanet.enabled = true;
            smoothCameraPlanet.target = transform;
        }
    }

}
