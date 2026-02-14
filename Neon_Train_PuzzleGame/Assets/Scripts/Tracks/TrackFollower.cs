using UnityEngine;

public class TrackFollower : MonoBehaviour
{
    [SerializeField] private float _folowerMaxSpeed;
    [SerializeField] private float _timeBeforeFullSpeed;

    private float _accelTimer;
    private float _currentSpeed;

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
}
