using UnityEngine;

public class TrackFollower : MonoBehaviour
{
    [SerializeField] private float _folowerMaxSpeed = 5.0f;
    [SerializeField] private float _timeBeforeFullSpeed = 0.5f;
    private float _currentSpeed = 0.0f;
    private float _accelTimer = 0.0f;

    [SerializeField] private Track _currentTrack;
    private TrackEndPoint _startPoint;
    private TrackEndPoint _endPoint;

    private float _currentTrackCompletion = 0.0f;

    void Start()
    {
        if (_currentTrack == null)
            _currentTrack = TrackManager.Instance.Tracks[0];

        _startPoint = _currentTrack.GetStartPoint();
        _endPoint = _startPoint.GetTrackEndPoint();
    }

    // ask Track the position and rotation based on where it is on the track
    void Update()
    {
        Accelerate();

        // curr_d/d_max => v*t/d_max => d_max = 1 => v*t
        _currentTrackCompletion += Time.deltaTime * _currentSpeed;

        if (_currentTrackCompletion >= 1.0f)
        {
            _currentTrackCompletion = 0.0f;
            GetNextTrack();
        }

        if (_currentTrack != null)
        {
            Vector3 position = Vector3.Lerp(_startPoint.transform.position, _endPoint.transform.position, _currentTrackCompletion);
            // Quaternion rotation = _currentTrack.GetRotation(_currentTrackCompletion);

            transform.position = position;
            // transform.rotation = rotation;
            transform.forward = _endPoint.transform.position - _startPoint.transform.position;
        }
    }

    private void Accelerate()
    {
        if (_accelTimer < _timeBeforeFullSpeed)
        {
            _accelTimer += Time.deltaTime;
            _currentSpeed = Mathf.Lerp(0.0f, _folowerMaxSpeed, _accelTimer / _timeBeforeFullSpeed);
        }
        else
        {
            _currentSpeed = _folowerMaxSpeed;
        }
    }

    private void Decelerate()
    {
        if (_accelTimer < _timeBeforeFullSpeed)
        {
            _accelTimer += Time.deltaTime;
            _currentSpeed = Mathf.Lerp(_folowerMaxSpeed, 0.0f, _accelTimer / _timeBeforeFullSpeed);
        }
        else
        {
            _currentSpeed = 0.0f;
        }
    }


    private void GetNextTrack()
    {
        // ask if the track is aligned 
        // if it is not, derail the train
        // if it is, ask Track the closest track endPoint

        _startPoint = _endPoint.GetClosestEndPoint();
        if (_startPoint == null)
        {
            _currentTrack = null;
            return;
        }

        _endPoint = _startPoint.GetTrackEndPoint();

        _currentTrack = TrackManager.Instance.Tracks[_startPoint.trackID];
    }
}
