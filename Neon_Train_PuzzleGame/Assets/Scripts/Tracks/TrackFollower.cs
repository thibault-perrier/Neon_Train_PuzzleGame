using System;
using UnityEngine;

public class TrackFollower : MonoBehaviour
{
    [SerializeField] private float _folowerMaxSpeed;
    [SerializeField] private float _timeBeforeFullSpeed;
    [SerializeField] public TrackEndpoint LevelStartEndpoint;

    private float _accelTimer;
    private float _currentSpeed;


    private struct TrackInfo
    {
        public Track CurrentTrack;
        public TrackEndpoint TrackStartEP;
        public Vector3 TrackStartEP_Pos;
        public TrackEndpoint TargetEP;
        public Vector3 TargetEP_Pos;

        public Vector3 TurningCircleCenter;
        public float TurnCircleRadius;

        public float TrackDist;

        public bool TrackIsTurn;
    }

    //* Track infos 
    private TrackInfo _currentTrackInfo;
    private TrackInfo _nextTrackInfo;


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
        _currentTrackInfo.TrackStartEP = LevelStartEndpoint;
        _currentTrackInfo.TrackStartEP_Pos = _currentTrackInfo.TrackStartEP.transform.position;

        // can be enclosed in func to pass from a track to another
        _currentTrackInfo.CurrentTrack = _currentTrackInfo.TrackStartEP.GetTrack();
        _currentTrackInfo.TargetEP = _currentTrackInfo.TrackStartEP.GetOtherEndEndpoint();
        _currentTrackInfo.TargetEP_Pos = _currentTrackInfo.TargetEP.transform.position;

        if (_currentTrackInfo.TrackIsTurn = _currentTrackInfo.CurrentTrack.IsTrackTurning(_currentTrackInfo.TrackStartEP))
        {
            _currentTrackInfo.TurningCircleCenter = _currentTrackInfo.TrackStartEP.CalculateExactCircleCenter(_currentTrackInfo.TargetEP);
        }
        _currentTrackInfo.TrackDist = TrackEndpoint.GetDistToOtherEnd(_currentTrackInfo.TrackStartEP_Pos, _currentTrackInfo.TargetEP_Pos, _currentTrackInfo.TrackIsTurn);

        // until here


        _isMoving = true;
    }

    public void StopMoving()
    {
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
            _currentTrackInfo = _nextTrackInfo;
            _nextTrackInfo = new();
        }

        _travelledDist += _currentSpeed * Time.deltaTime;
        _trackCompletion = _travelledDist / _currentTrackInfo.TrackDist;
        if (_currentTrackInfo.TrackIsTurn)
        {
            //* pos is (O + R * dir)
            // R is |OA|
            // dir is (cos(teta), sin(teta))

            // S = teta * R
            // S is the displacement on the circle so _travelledDist
            // teta is S / R

            _currentTrackInfo.TurnCircleRadius = (_currentTrackInfo.TrackStartEP_Pos - _currentTrackInfo.TurningCircleCenter).magnitude;
            float teta = _travelledDist / _currentTrackInfo.TurnCircleRadius;

            Vector3 displacement = new(_currentTrackInfo.TurnCircleRadius * Mathf.Cos(teta), 0.0f, _currentTrackInfo.TurnCircleRadius * Mathf.Sin(teta));
            transform.position = _currentTrackInfo.TurningCircleCenter + displacement;
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
            SetNextEndpoint(endpoint);
        }
    }

    private void SetNextEndpoint(TrackEndpoint endpoint)
    {
        Debug.Log("Next track will be " + endpoint.GetTrack());
        throw new NotImplementedException();
    }
}
