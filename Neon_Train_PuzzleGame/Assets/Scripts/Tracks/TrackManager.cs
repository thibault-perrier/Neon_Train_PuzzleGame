using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    [SerializeField] private Transform _mapHolder;
    [SerializeField] private List<Track> _tracks = new();
    [SerializeField] private List<TrackEndpoint> _startingTrackEndpoints = new();

    [SerializeField] private Transform _trackFollowerHolder;
    [SerializeField] private TrackFollower _trackFollowerPrefab;
    private List<TrackFollower> _trackFollowers = new();

    void Awake()
    {
        if (_tracks.Count == 0)
        {
            GetTracksInLevel();
        }

        if (_startingTrackEndpoints.Count == 0)
        {
            _startingTrackEndpoints.Add(_tracks[0].GetFstTrackEndpoint());
        }

        if (_trackFollowers.Count != _startingTrackEndpoints.Count)
        {
            SpawnTrackFollowers();
        }
    }

    private void SpawnTrackFollowers()
    {
        for (int i = _trackFollowers.Count - 1; i >= 0; i--)
            Destroy(_trackFollowers[i]);
        _trackFollowers.Clear();

        foreach (TrackEndpoint endpoint in _startingTrackEndpoints)
        {
            TrackFollower follower = Instantiate(_trackFollowerPrefab, _trackFollowerHolder);
            follower.Init(endpoint);
            _trackFollowers.Add(follower);
        }
    }

    private void GetTracksInLevel()
    {
        _tracks = _mapHolder.GetComponentsInChildren<Track>().ToList();
    }

    public void StartTrains()
    {
        Debug.Log("Starting Trains");
        foreach (TrackFollower f in _trackFollowers)
            f.StartMoving();
    }

    //     void OnEnable()
    //     {
    //         if (_startingTrackEndpoints.Count == 0)
    //         {
    //             _startingTrackEndpoints.Add(_tracks[0].GetFstTrackEndpoint());
    //         }
    //     }

    //     void OnDisable()
    //     {
    //         _startingTrackEndpoints.Clear();
    //     }
}
