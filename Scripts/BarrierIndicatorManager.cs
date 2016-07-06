using UnityEngine;
using System.Collections;

public class BarrierIndicatorManager : MonoBehaviour {

    public GameObject barrierIndicator;

    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private bool isEnabled = true;

    void Start()
    {
        spriteRenderer = barrierIndicator.GetComponentInChildren<SpriteRenderer>();
        anim = barrierIndicator.GetComponentInChildren<Animator>();
        DisableIndicator();
    }

    public void EnableIndicator ()
    {
        if (!isEnabled)
        {
            spriteRenderer.enabled = true;
            anim.enabled = true;
            isEnabled = true;
        }
        UpdateRotation();
    }

    public void DisableIndicator()
    {
        if (isEnabled)
        {
            spriteRenderer.enabled = false;
            anim.enabled = false;
            isEnabled = false;
        }
    }

    private void UpdateRotation()
    {
        barrierIndicator.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(-transform.position.x, transform.position.y));
    }

}
