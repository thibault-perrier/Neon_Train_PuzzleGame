using System.Collections.Generic;
using UnityEngine;

public class TrackEndpoint : MonoBehaviour
{
    [SerializeField] private List<TrackEndpoint> _connectedEndpoints = new();
    private int _currConnection = 0;

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
}
