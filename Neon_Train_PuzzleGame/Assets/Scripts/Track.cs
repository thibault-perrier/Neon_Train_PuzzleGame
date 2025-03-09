using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public int ID;
    [SerializeField] private List<TrackEndPoint> _trackPoints = new();

    void Awake()
    {
        foreach (TrackEndPoint ep in GetComponentsInChildren<TrackEndPoint>())
        {
            _trackPoints.Add(ep);
            ep.trackID = ID;
        }
    }

    public TrackEndPoint GetStartPoint()
    {
        return _trackPoints[0];
    }
}
