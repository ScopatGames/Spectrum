﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineSegmentRing : MonoBehaviour {

    public int radius;

    private int segments;
    private Color segmentStartColor;
    private Color segmentEndColor;
    private float segmentStartWidth;
    private float segmentEndWidth;
    private float expansionRate;
    private float maxLocalScale = 40f;

    public Pool lineSegmentPool;

    private List<GameObject> lineSegments = new List<GameObject>();
    private RingController ringController;

    private GameObject tempLineSegment;
    private LineSegmentController tempLSC;
    private Vector3 tempPos1 = Vector3.zero;
    private Vector3 tempPos2 = Vector3.zero;
    private Vector3 tempPos3 = Vector3.zero;
    private float segmentRadians;

    void Start()
    {
        ringController = GetComponentInParent<RingController>();
        lineSegmentPool = ringController.lineSegmentPool;
    }

    public void ExpandRing()
    {
        StartCoroutine("Expand");
    }

    private IEnumerator Expand()
    {
        GetComponent<Spin>().speed = 0;
        while(transform.localScale.x < maxLocalScale)
        {
            transform.localScale += new Vector3(expansionRate, expansionRate, 0f);
            yield return null;
        }
        transform.localScale = new Vector3(1f, 1f, 1f);
        DismantleRing();
        ringController.ReleaseRing(gameObject);
    }

    public void Setup(int segments_i, Color segmentStartColor_i, Color segmentEndColor_i, float segmentStartWidth_i, float segmentEndWidth_i, float expansionRate_i, float spin_i, Pool pool)
    {
        segments = segments_i;
        segmentStartColor = segmentStartColor_i;
        segmentEndColor = segmentEndColor_i;
        segmentStartWidth = segmentStartWidth_i;
        segmentEndWidth = segmentEndWidth_i;
        GetComponent<Spin>().speed = spin_i;
        transform.localScale = new Vector3(1f, 1f, 1f);
        expansionRate = expansionRate_i;

        BuildRing(pool);
    }

    private bool BuildRing(Pool segmentPool)
    {
        if (segments <= segmentPool.CheckInventory())
        {
            segmentRadians = 2 * Mathf.PI / segments;
            bool lastSegmentSkipped = true;

            for (int i = 0; i < (segments - 1); i++)
            {
                if (Random.Range(0, 10) < 2 || i == 0)
                {
                    tempLineSegment = segmentPool.CheckOut().gameObject;
                    tempLineSegment.transform.parent = transform;
                    tempLineSegment.transform.position = transform.position;
                    tempLineSegment.transform.rotation = Quaternion.identity;
                    tempLSC = tempLineSegment.GetComponent<LineSegmentController>();

                    //calculate tempPos1
                    if (lastSegmentSkipped)
                    {
                        tempPos1.x = radius * Mathf.Cos(i * segmentRadians);
                        tempPos1.y = radius * Mathf.Sin(i * segmentRadians);
                    }
                    else
                    {
                        tempPos1.x = tempPos3.x;
                        tempPos1.y = tempPos3.y;
                    }
                    //calculate tempPos3
                    tempPos3.x = radius * Mathf.Cos((i + 1) * segmentRadians);
                    tempPos3.y = radius * Mathf.Sin((i + 1) * segmentRadians);
                    //calculate tempPos2
                    tempPos2.x = (tempPos1.x + tempPos3.x) / 2;
                    tempPos2.y = (tempPos1.y + tempPos3.y) / 2;

                    tempLSC.SetPositions(tempPos1, tempPos2, tempPos3);
                    tempLSC.SetColors(segmentStartColor, segmentEndColor);
                    tempLSC.SetWidths(segmentStartWidth, segmentEndWidth);

                    lineSegments.Add(tempLineSegment);
                    lastSegmentSkipped = false;
                }
                else
                {
                    lastSegmentSkipped = true;
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DismantleRing()
    {
        foreach(GameObject go in lineSegments)
        {
            lineSegmentPool.CheckIn(go.GetComponent<PoolItem>());
        }
        lineSegments.Clear();
    }



	
}
