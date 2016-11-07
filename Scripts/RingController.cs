using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RingController : MonoBehaviour {

    public int numberOfRings;
    public int minSegments;
    public int maxSegments;
    public Color minSegmentStartColor;
    public Color maxSegmentStartColor;
    public Color minSegmentEndColor;
    public Color maxSegmentEndColor;
    public float minSegmentStartWidth;
    public float maxSegmentStartWidth;
    public float minSegmentEndWidth;
    public float maxSegmentEndWidth;
    public float minSpin;
    public float maxSpin;
    public float expansionRate;

    public Pool ringPool;
    public Pool lineSegmentPool;

    private List<GameObject> activeRings = new List<GameObject>();

    private int segments_i;
    private Color segmentStartColor_i;
    private Color segmentEndColor_i;
    private float segmentStartWidth_i;
    private float segmentEndWidth_i;
    private float expansionRate_i;
    private float spin_i;

    private GameObject tempRing;
    private LineSegmentRing tempLSR;

    void Awake()
    {
        InitializePools(numberOfRings, maxSegments);
        RetrieveRing(ringPool);
    }

    private void InitializePools(int numRings, int maxSegs)
    {
        int ringsInPool = ringPool.CheckInventory();
        
        if (ringsInPool < numRings)
        {
            ringPool.InstantiatePoolObjects(numRings - ringsInPool);
        }

        int segmentsInPool = lineSegmentPool.CheckInventory();
        if (segmentsInPool < (numRings * maxSegs))
        {
            lineSegmentPool.InstantiatePoolObjects(numRings * maxSegs - segmentsInPool);
        }
    }

    private void RandomizeInputs()
    {
        segments_i = Random.Range(minSegments, maxSegments+1);
        segmentStartColor_i = maxSegmentStartColor;
        segmentEndColor_i = maxSegmentEndColor;
        segmentStartWidth_i = maxSegmentStartWidth;
        segmentEndWidth_i = maxSegmentEndWidth;
        expansionRate_i = expansionRate;
        if(Random.Range(0, 2)==0)
        {
            spin_i = Random.Range(minSpin, (maxSpin+1));
        }
        else
        {
            spin_i = Random.Range(-maxSpin, (-minSpin + 1));
        }
    }

    public void ReleaseRing(GameObject ring)
    {
        activeRings.Remove(ring);
        ringPool.CheckIn(ring);
        RetrieveRing(ringPool);
    }

    public void RetrieveRing(Pool ringPool)
    {
        if (ringPool.CheckInventory() > 0)
        {
            tempRing = ringPool.CheckOut();
            tempRing.transform.parent = transform;
            tempRing.transform.position = transform.position;
            tempRing.transform.rotation = Quaternion.identity;
            tempLSR = tempRing.GetComponent<LineSegmentRing>();
            RandomizeInputs();
            tempLSR.Setup(segments_i, segmentStartColor_i, segmentEndColor_i, segmentStartWidth_i, segmentEndWidth_i, expansionRate_i, spin_i, lineSegmentPool);
            activeRings.Add(tempRing);
        }
    }

}
