using System;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public static TrackManager Instance { get; private set; }

    [SerializeField, NonSerialized] public Dictionary<int, Track> Tracks = new();

    void Awake()
    {
        if (TrackManager.Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            TrackManager.Instance = this;
        }

        // GetComponentInChildren<MapGen>().GenerateMap();

        foreach (Track track in GetComponentsInChildren<Track>())
        {
            Tracks.Add(track.ID, track);
        }
    }

}