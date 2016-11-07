using UnityEngine;
using System.Collections;

public class LineSegmentController : MonoBehaviour {

    public LineRenderer lineRenderer01;
    public LineRenderer lineRenderer02;

    public void SetPositions(Vector3 pos1, Vector3 pos2, Vector3 pos3)
    {
        lineRenderer01.SetPosition(0, pos1);
        lineRenderer01.SetPosition(1, pos2);
        lineRenderer02.SetPosition(0, pos3);
        lineRenderer02.SetPosition(1, pos2);
    }

    public void SetColors(Color startColor, Color endColor)
    {
        lineRenderer01.SetColors(startColor, endColor);
        lineRenderer02.SetColors(startColor, endColor);
    }

    public void SetWidths(float startWidth, float endWidth)
    {
        lineRenderer01.SetWidth(startWidth, endWidth);
        lineRenderer02.SetWidth(startWidth, endWidth);
    }

}
