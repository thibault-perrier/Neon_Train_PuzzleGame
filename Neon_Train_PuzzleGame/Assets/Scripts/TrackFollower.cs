using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackFollower : MonoBehaviour
{
    [Header("Train Parameters")]
    [SerializeField] private float _folowerMaxSpeed = 5.0f;
    [SerializeField] private float _timeBeforeFullSpeed = 0.5f;
    private float _currentSpeed = 0.0f;
    private float _accelTimer = 0.0f;
    [SerializeField] private float _closestCheckRadius = 0.25f;
    [SerializeField] private float _derailAnimationTime = 0.5f;

    [Header("For Level Initializer")]
    [SerializeField] private Track _currentTrack;
    private TrackEndPoint _startPoint;
    private TrackEndPoint _endPoint;

    private bool _isRunning = true;
    private bool _isDerailing = false;
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
            if (GetNextTrack())
                _currentTrackCompletion = 0.0f;
            else
                return;
        }

        if (_currentTrack != null)
        {
            transform.position = _startPoint.EvaluatePosition(_currentTrackCompletion);
            // transform.rotation = rotation;
            transform.forward = _startPoint.EvaluateRotation(_currentTrackCompletion);
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


    private bool GetNextTrack()
    {
        // MAKE THE GET NEXT TRACK FROM THE TRAIN AND NOT THE TILE

        // ask if the track is aligned 
        // if it is not, derail the train
        // if it is, ask Track the closest track endPoint

        _startPoint = GetClosestEndPoint();
        if (!_isRunning || !_currentTrack.IsAligned || _startPoint == null || !TrackManager.Instance.Tracks[_startPoint.trackID].IsAligned)
        {
            _isRunning = false;
            DerailTrain();
            return false;
        }

        _endPoint = _startPoint.GetTrackEndPoint();

        _currentTrack = TrackManager.Instance.Tracks[_startPoint.trackID];

        return true;
    }

    public TrackEndPoint GetClosestEndPoint()
    {
        List<TrackEndPoint> closestEndPoints = Physics.OverlapSphere(transform.position, _closestCheckRadius).
                                                        Select(c => c.GetComponent<TrackEndPoint>()).
                                                        Where(c => c != null && c.trackID != _currentTrack.ID).
                                                        OrderBy(c => Vector3.Distance(c.transform.position, transform.position)).
                                                        ToList();
        if (closestEndPoints.Count == 0)
        {
            return null;
        }

        return closestEndPoints[0];
    }

    private void DerailTrain()
    {
        if (_isDerailing)
            return;

        Debug.Log("Derailing Train");
        StartCoroutine(DerailTrainAnimation(transform.right));
    }

    private IEnumerator DerailTrainAnimation(Vector3 derailDir)
    {
        _isDerailing = true;

        Vector3 originUp = transform.up;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.FromToRotation(originUp, derailDir) * transform.rotation;

        float elapsedTime = 0.0f;

        while (elapsedTime < _derailAnimationTime)
        {
            float t = elapsedTime / _derailAnimationTime;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // S'assurer que la rotation finale est bien appliquée
        transform.rotation = targetRotation;
        _isDerailing = false;

        this.enabled = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _closestCheckRadius);
    }
}
