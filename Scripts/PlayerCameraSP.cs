using UnityEngine;
using System.Collections;

public class PlayerCameraSP : MonoBehaviour {

    public GameObject mainCamera;

    private SmoothCameraPlanet smoothCameraPlanet;
    private SmoothCameraSpace smoothCameraSpace;
    private Transform otherPlayerTransform;

    void Start()
    {
        smoothCameraPlanet = mainCamera.GetComponent<SmoothCameraPlanet>();
        smoothCameraPlanet.target = null;
        smoothCameraSpace = mainCamera.GetComponent<SmoothCameraSpace>();
        smoothCameraSpace.target = null;
        mainCamera.transform.parent = null;
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -200f);
        mainCamera.transform.rotation = Quaternion.Euler(Vector3.zero);
        otherPlayerTransform = transform;
    }

    public void EnableSpaceCamera()
    {
        smoothCameraPlanet.enabled = false;
        smoothCameraSpace.enabled = true;
        smoothCameraSpace.target = transform;
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -200);
        mainCamera.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void EnablePlanetCameraAttacker()
    {
        smoothCameraSpace.enabled = false;
        smoothCameraPlanet.enabled = true;
        smoothCameraPlanet.target = transform;
    }

    public void EnablePlanetCameraDefender()
    {
        smoothCameraSpace.enabled = false;
        smoothCameraPlanet.enabled = true;
        smoothCameraPlanet.target = otherPlayerTransform;
    }

    
}
