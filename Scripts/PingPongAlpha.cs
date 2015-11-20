using UnityEngine;
using System.Collections;

public class PingPongAlpha : MonoBehaviour {

    private Renderer rend;
    private Color startingColor;
    private float newAlpha;
    private Color newColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startingColor = rend.material.GetColor("_Color02");

    }
      
    void Update()
    {
        newAlpha = Mathf.PingPong(Time.time*10, 1.0f);
        newColor = new Color(startingColor.r, startingColor.g, startingColor.b, newAlpha);
        
        rend.material.SetColor("_Color02", newColor);
    }
}
