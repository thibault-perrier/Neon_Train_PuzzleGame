using System;
using UnityEngine;

public class TrackFollower : MonoBehaviour
{
    [SerializeField] private float _folowerMaxSpeed;
    [SerializeField] private float _timeBeforeFullSpeed;
    [SerializeField] public TrackEndpoint LevelStartEndpoint;

    private float _accelTimer;
    private float _currentSpeed;

    [Serializable]
    private struct TrackInfo
    {
        public Track CurrentTrack;
        public Vector3 TrackMid_Pos;

        public TrackEndpoint TrackStartEP;
        public Vector3 TrackStartEP_Pos;

        public TrackEndpoint TargetEP;
        public Vector3 TargetEP_Pos;

        public float TrackDist;

        public bool TrackIsTurn;
    }

    //* Track infos 
    private TrackInfo _currentTrackInfo;
    private TrackInfo? _nextTrackInfo;


    //* Follower state tracking
    private bool _isMoving;
    private bool _reachedMaxSpeed;
    private float _travelledDist;
    private float _trackCompletion;


    // void Start()
    // {
    //     StartMoving();
    // }

    public void Init(TrackEndpoint endpoint)
    {
        LevelStartEndpoint = endpoint;
        transform.position = endpoint.transform.position;

        StartMoving();
    }


    public void StartMoving()
    {
        _currentTrackInfo = CreateEPInfo(LevelStartEndpoint);
        _isMoving = true;
    }

    public void StopMoving()
    {
        Debug.Log("Crash");
        _isMoving = false;
    }

    void Update()
    {
        if (!_isMoving) return;

        if (!_reachedMaxSpeed) Accelerate();
        else _currentSpeed = _folowerMaxSpeed;

        if (_trackCompletion >= 1.0f) // means the follower reached the end of the track 
        {
            // _travelledDist = 0.0f;
            _travelledDist -= _currentTrackInfo.TrackDist;

            if (_nextTrackInfo == null)
            {
                StopMoving();
                return;
            }

            _currentTrackInfo = (TrackInfo)_nextTrackInfo;
            _nextTrackInfo = null;
        }

        _travelledDist += _currentSpeed * Time.deltaTime;
        _trackCompletion = _travelledDist / _currentTrackInfo.TrackDist;
        if (_currentTrackInfo.TrackIsTurn)
        {
            // double interpolation: aIP = a to track center, bIP = track center to b, pos = aIP to bIP
            Vector3 aInterpolation = Vector3.Lerp(_currentTrackInfo.TrackStartEP_Pos, _currentTrackInfo.TrackMid_Pos, _trackCompletion);
            Vector3 bInterpolation = Vector3.Lerp(_currentTrackInfo.TrackMid_Pos, _currentTrackInfo.TargetEP_Pos, _trackCompletion);
            Vector3 pos = Vector3.Lerp(aInterpolation, bInterpolation, _trackCompletion);
            transform.position = pos;
        }
        else // straight
        {
            transform.position = Vector3.Lerp(_currentTrackInfo.TrackStartEP_Pos, _currentTrackInfo.TargetEP_Pos, _trackCompletion);
        }

        // TODO: remember to calculate follower forward so that is can face the correct way when moving
    }


    private void Accelerate()
    {
        _currentSpeed = Mathf.Lerp(0.0f, _folowerMaxSpeed, _accelTimer / _timeBeforeFullSpeed);

        if (_accelTimer < _timeBeforeFullSpeed)
            _accelTimer += Time.deltaTime;
        else
        {
            _reachedMaxSpeed = true;
            _accelTimer = 0;
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TrackEndpoint>() is TrackEndpoint endpoint && endpoint.GetTrack().ID != _currentTrackInfo.CurrentTrack.ID)
        {
            _nextTrackInfo = CreateEPInfo(endpoint);
        }
    }

    private TrackInfo CreateEPInfo(TrackEndpoint endpoint)
    {
        TrackInfo trackInfo = new()
        {
            TrackStartEP = endpoint,
            TrackStartEP_Pos = endpoint.transform.position,

            TargetEP = endpoint.GetOtherEndEndpoint(),
            // TargetEP_Pos = TargetEP.transform.position,

            CurrentTrack = endpoint.GetTrack(),
        };

        trackInfo.TargetEP_Pos = trackInfo.TargetEP.transform.position;

        Vector3 TrackRawPos = trackInfo.CurrentTrack.transform.position;
        trackInfo.TrackMid_Pos = new Vector3(TrackRawPos.x, trackInfo.TargetEP_Pos.y, TrackRawPos.z);

        trackInfo.TrackIsTurn = trackInfo.CurrentTrack.IsTrackTurning(trackInfo.TrackStartEP);
        trackInfo.TrackDist = TrackEndpoint.GetDistToOtherEnd(trackInfo.TrackStartEP_Pos, trackInfo.TargetEP_Pos, trackInfo.TrackIsTurn);

        return trackInfo;
    }
}

