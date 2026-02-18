using UnityEngine;

public class TrackFollower : MonoBehaviour
{
    [SerializeField] private float _folowerMaxSpeed;
    [SerializeField] private float _timeBeforeFullSpeed;
    [SerializeField] public TrackEndpoint LevelStartEndpoint;

    private float _accelTimer;
    private float _currentSpeed;

    private TrackEndpoint _trackStartingEndpoint; // TODO: struct for track informations and make two variables (current track and next track) so that when t >= 1, then current = next.
    private TrackEndpoint _targetEndpoint;
    private Vector3 _turningCircleCenter;
    private Track _currentTrack;
    private float _trackDist;
    private float _travelledDist;
    private bool _trackIsTurn;

    private bool _isMoving;
    private bool _reachedMaxSpeed;

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
        _trackStartingEndpoint = LevelStartEndpoint;

        // can be enclosed in func to pass from a track to another
        _currentTrack = _trackStartingEndpoint.GetTrack();
        _targetEndpoint = _trackStartingEndpoint.GetOtherEndEndpoint();

        if (_trackIsTurn = _currentTrack.IsTrackTurning(_trackStartingEndpoint))
        {
            _turningCircleCenter = _trackStartingEndpoint.CalculateExactCircleCenter(_targetEndpoint);
        }
        _trackDist = TrackEndpoint.GetDistToOtherEnd(_trackStartingEndpoint.transform.position, _targetEndpoint.transform.position, _trackIsTurn);
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

        _travelledDist += _currentSpeed * Time.deltaTime;
        float t = _travelledDist / _trackDist;
        if (_trackIsTurn)
        {
            Debug.Log("Is turn");
            // calculate next pos based on center of circle and slerp
            // TODO: Slerp to get the angle (0, 90) -> angle from OA to OB
            //* pos is just (O + angle * radius)
        }
        else // straight
        {
            transform.position = Vector3.Lerp(_trackStartingEndpoint.transform.position, _targetEndpoint.transform.position, t);
        }

        // TODO: remember to calculate follower forward so that is can facec the correct way when moving
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
}
