using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public int ID;
    [SerializeField] private List<TrackEndPoint> _trackPoints = new();

    [SerializeField] private float _rotationDuration = 0.5f;
    public bool IsAligned = true;

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

    public void RotateTile()
    {
        StartCoroutine(TileRotationAnimation());
    }

    private IEnumerator TileRotationAnimation()
    {
        Transform tileTrans = transform;

        IsAligned = false;

        Quaternion startRotation = tileTrans.rotation;
        Quaternion endRotation = tileTrans.rotation * Quaternion.Euler(0.0f, 90.0f, 0.0f);

        float elapsedTime = 0.0f;
        float duration = _rotationDuration;

        while (elapsedTime < duration)
        {
            tileTrans.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tileTrans.rotation = endRotation;
        IsAligned = true;
    }
}
