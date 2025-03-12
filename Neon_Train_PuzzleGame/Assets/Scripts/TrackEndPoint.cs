using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackEndPoint : MonoBehaviour
{
    public int trackID;
    [SerializeField] private List<TrackEndPoint> _connectedEndPoints = new();

    public bool _isTurn = false;
    private TrackEndPoint _endPoint;

    public void Init(int tid)
    {
        trackID = tid;
        _endPoint = _connectedEndPoints[0];
        _isTurn = IsTurnTile();

    }

    public TrackEndPoint GetTrackEndPoint()
    {
        return _endPoint;
    }

    private bool IsTurnTile()
    {
        // 1 if in the same direction, 0 if perpendicular
        return Mathf.Abs(Vector3.Dot(transform.forward, _endPoint.transform.forward)) < 0.5f;
    }

    public Vector3 EvaluatePosition(float t)
    {
        Vector3 aInterpolation = Vector3.Lerp(transform.position, transform.parent.transform.position, t);
        Vector3 bInterpolation = Vector3.Lerp(transform.parent.transform.position, _endPoint.transform.position, t);
        return Vector3.Lerp(aInterpolation, bInterpolation, t);
    }

    public Vector3 EvaluateRotation(float t)
    {
        if (!_isTurn)
        {
            return (_endPoint.transform.position - transform.position).normalized;
        }

        if (t > 0.9f)
        {
            return -_endPoint.transform.forward;
        }
        else if (t < 0.1f)
        {
            return transform.forward;
        }

        float t1 = t + 0.1f;
        Vector3 p1aInterpolation = Vector3.Lerp(transform.position, transform.parent.transform.position, t1);
        Vector3 p1bInterpolation = Vector3.Lerp(transform.parent.transform.position, _endPoint.transform.position, t1);
        Vector3 p1 = Vector3.Lerp(p1aInterpolation, p1bInterpolation, t1);

        float t2 = t - 0.1f;
        Vector3 p2aInterpolation = Vector3.Lerp(transform.position, transform.parent.transform.position, t2);
        Vector3 p2bInterpolation = Vector3.Lerp(transform.parent.transform.position, _endPoint.transform.position, t2);
        Vector3 p2 = Vector3.Lerp(p2aInterpolation, p2bInterpolation, t2);

        return (p1 - p2).normalized;
    }

    public void SwitchTrack()
    {
        TrackEndPoint fst = _connectedEndPoints[0];
        _connectedEndPoints.RemoveAt(0);
        _connectedEndPoints.Add(fst);
        _endPoint = _connectedEndPoints[0];

        _isTurn = IsTurnTile();
    }
}
