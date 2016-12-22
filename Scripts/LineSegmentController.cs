using UnityEngine;
using System.Collections;

public class LineSegmentController : MonoBehaviour {

    public LineRenderer lineRenderer01;
    public LineRenderer lineRenderer02;

    private EdgeCollider2D edge;

    void Awake()
    {
        edge = GetComponent<EdgeCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == _Tags.player)
        {
           // Debug.Log("Hit player!");
        }
    }

    public void SetPositions(Vector3 pos1, Vector3 pos2, Vector3 pos3)
    {
        lineRenderer01.SetPosition(0, pos1);
        lineRenderer01.SetPosition(1, pos2);
        lineRenderer02.SetPosition(0, pos3);
        lineRenderer02.SetPosition(1, pos2);
        Vector2[] tempArray = { new Vector2(pos1.x, pos1.y), new Vector2(pos3.x, pos3.y) };
        edge.points = tempArray;
    }

    public void SetColors(Color startColor, Color endColor)
    {
        lineRenderer01.startColor = startColor;
        lineRenderer01.endColor = endColor;
        lineRenderer02.startColor = startColor;
        lineRenderer02.endColor = endColor;
    }

    public void SetWidths(float startWidth, float endWidth)
    {
        lineRenderer01.startWidth = startWidth;
        lineRenderer01.endWidth = endWidth;
        lineRenderer02.startWidth = startWidth;
        lineRenderer02.endWidth = endWidth;
    }

}
