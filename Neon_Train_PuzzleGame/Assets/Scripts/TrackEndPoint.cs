using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackEndPoint : MonoBehaviour
{
    public int trackID;
    [SerializeField] private List<TrackEndPoint> _connectedEndPoints = new();
    [SerializeField] private float _closestCheckRadius = 0.25f;

    public TrackEndPoint GetClosestEndPoint()
    {
        List<TrackEndPoint> closestEndPoints = Physics.OverlapSphere(transform.position, _closestCheckRadius).
                                                        Select(c => c.GetComponent<TrackEndPoint>()).
                                                        Where(c => c != null && c != this).
                                                        ToList();
        if (closestEndPoints.Count == 0)
        {
            return null;
        }

        return closestEndPoints.OrderBy(c => Vector3.Distance(c.transform.position, transform.position)).FirstOrDefault();
    }

    public TrackEndPoint GetTrackEndPoint()
    {
        return _connectedEndPoints[0];
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _closestCheckRadius);
    }
}
