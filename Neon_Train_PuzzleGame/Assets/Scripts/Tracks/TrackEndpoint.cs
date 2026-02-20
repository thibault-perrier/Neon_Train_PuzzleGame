using System;
using System.Collections.Generic;
using UnityEngine;

public class TrackEndpoint : MonoBehaviour
{
    [SerializeField] private List<TrackEndpoint> _connectedEndpoints = new();
    private int _currConnection = 0;
    private int _trackID;

    public void SetTrackID(int id)
    {
        _trackID = id;
    }

    public TrackEndpoint GetOtherEndEndpoint()
    {
        return _connectedEndpoints[_currConnection];
    }

    public Track GetTrack()
    {
        return transform.parent.parent.GetComponent<Track>();
    }

    public void SwitchEndpoint()
    {
        _currConnection = (_currConnection + 1) % _connectedEndpoints.Count;
    }





    public static float GetDistToOtherEnd(Vector3 pointA, Vector3 pointB, bool trackIsATurn)
    {
        if (trackIsATurn)
            return GetCurvedDistBasedOnCenter(pointA, CalculateSimpleCircleCenter(pointA, pointB));
        else
            return Vector3.Distance(pointA, pointB);
    }

    public static float GetCurvedDistBasedOnCenter(Vector3 pointA, Vector3 pointO)
    {
        // s = length of an arc on a circle
        // s = r * teta 
        // r => get center, calculate dist to one of the points
        // teta is 90° since game is based on tiles (with teta in rads)
        float r = Vector3.Distance(pointO, pointA);
        float teta = 90.0f * Mathf.Deg2Rad;
        return r * teta;
    }

    public static Vector3 CalculateSimpleCircleCenter(Vector3 pointA, Vector3 pointB)
    {
        Vector3 midpoint = (pointA + pointB) * 0.5f;
        Vector3 AB = pointB - pointA;
        Vector3 perpendicular = new Vector3(-AB.z, AB.y, AB.x).normalized;

        // https://fr.wikipedia.org/wiki/Triangle_isocèle_rectangle: h=b/2, b being the hypotenuse
        float distanceToCenter = AB.magnitude / 2f;

        return midpoint + distanceToCenter * perpendicular;
    }













    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.05f);
    }
}
