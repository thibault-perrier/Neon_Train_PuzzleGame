using System;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public int ID => GetInstanceID();

    [SerializeField] private TrackType _trackType;

    [SerializeField] protected List<TrackEndpoint> _endpoints = new();


    void Start()
    {
        foreach (TrackEndpoint ep in _endpoints) //! todo: check if id is important since getting the track is essential. 
            ep.SetTrackID(ID);
    }

    public virtual void DoTrackActivation()
    {
        Debug.Log("Normal track, nothing to activate...");
    }


    public bool IsTrackTurning(TrackEndpoint _startingTrackEndpoint)
    {
        TrackEndpoint otherEnd = _startingTrackEndpoint.GetOtherEndEndpoint();

        // projection of b onto a = (A dot B) / mag(A)
        Vector3 startToMiddleDir = Vector3.ProjectOnPlane(transform.position - _startingTrackEndpoint.transform.position, Vector3.up).normalized;
        Vector3 endToEndDir = Vector3.ProjectOnPlane(otherEnd.transform.position - _startingTrackEndpoint.transform.position, Vector3.up).normalized;

        // float dot = Vector3.Dot(startToMiddleDir, endToEndDir);
        // Debug.Log(dot);
        // return dot < 0.9f;
        return Vector3.Dot(startToMiddleDir, endToEndDir) < 0.9f;
    }


    public TrackEndpoint GetFstTrackEndpoint()
    {
        return _endpoints[0] ?? default;
    }






    #region Debug
    [ContextMenu("Debug/Check For Turn")]
    public void DebugCheckForTurn()
    {
        foreach (TrackEndpoint endpoint in _endpoints)
            Debug.Log($"Track {name}, endpoint {endpoint}, track is{(IsTrackTurning(endpoint) ? " " : " not ")}a turn");
    }
    #endregion
}

[Flags]
public enum TrackType
{
    None = 0,
    Straight = 1,
    Turn = 2,
    Spur = 4,
    Cross = 8,
    Normal = 16,
}