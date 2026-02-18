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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.05f);
    }


    public float GetDistToOtherEnd(TrackEndpoint targetEndpoint)
    {
        if (GetTrack().IsTrackTurning(targetEndpoint))
        {
            return GetCurvedDistBasedOnCenter(transform.position, CalculateExactCircleCenter(targetEndpoint));
        }
        else
        {
            return Vector3.Distance(transform.position, GetOtherEndEndpoint().transform.position);
        }
    }

    public Vector3 CalculateExactCircleCenter(TrackEndpoint targetEndpoint)
    {
        Vector3 pointA = transform.position;
        Vector3 pointB = targetEndpoint.transform.position;

        Vector2 midpoint = (pointA + pointB) / 2f;
        Vector2 AB = pointB - pointA;
        Vector2 perpendicular = new Vector2(-AB.y, AB.x).normalized;

        // https://fr.wikipedia.org/wiki/Triangle_isocèle_rectangle: h=b/2, b being the hypotenuse
        float distanceToCenter = AB.magnitude / 2f;

        Vector3 rightA = transform.right; // TODO: check if all endpoints point inwards 
        int isOnRight = Vector3.Dot(rightA, AB.normalized) > 0 ? 1 : -1;


        return midpoint + isOnRight * distanceToCenter * perpendicular;
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
        // s = r * teta
        // r => get center, calculate dist to one of the points
        // teta is 90° since game is based on tiles
        float r = Vector3.Distance(pointO, pointA);
        float teta = 90.0f * Mathf.Deg2Rad;
        return r * teta;
    }

    public static Vector3 CalculateSimpleCircleCenter(Vector3 pointA, Vector3 pointB)
    {
        Vector2 midpoint = (pointA + pointB) / 2f;
        Vector2 AB = pointB - pointA;
        Vector2 perpendicular = new Vector2(-AB.y, AB.x).normalized;

        // https://fr.wikipedia.org/wiki/Triangle_isocèle_rectangle: h=b/2, b being the hypotenuse
        float distanceToCenter = AB.magnitude / 2f;

        return midpoint + distanceToCenter * perpendicular;
    }
}
