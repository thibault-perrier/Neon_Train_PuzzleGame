using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrackRotator : MonoBehaviour
{
    [SerializeField] LayerMask InteractableMask;
    [SerializeField] private float _rotTimer = 0.5f;

    private List<RotInfo> _rotInfos = new();
    private bool _isInRotation;

    private struct RotInfo
    {
        public Track track;
        public Transform trackTrans;
        public float timer;
        public float startingAngle;
        public float targetAngle;
    }

    private void TryRotateTile(Vector2 clickPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(clickPos);
        if (Physics.Raycast(ray, out RaycastHit hit, 50.0f, InteractableMask))
        {
            Debug.Log("Touched " + hit.transform.name);
            CreateRotInfo(hit.transform);
        }
        else
            Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.red, 5f);
    }

    private void CreateRotInfo(Transform t)
    {
        Track newTrack = t.GetComponent<Track>();
        if (_rotInfos.Any(r => r.track.ID == newTrack.ID)) return;

        RotInfo info = new()
        {
            track = newTrack,
            trackTrans = t,
            timer = 0.0f,
            startingAngle = t.eulerAngles.y,
            targetAngle = t.eulerAngles.y + 90,
        };
        _rotInfos.Add(info);

        if (!_isInRotation)
            StartCoroutine(DoRotationAnim());
    }

    private IEnumerator DoRotationAnim()
    {
        _isInRotation = true;

        while (_rotInfos.Count > 0)
        {
            for (int i = _rotInfos.Count - 1; i >= 0; i--)
            {
                RotInfo info = _rotInfos[i];

                info.timer += Time.deltaTime;
                float completion = Mathf.Min(info.timer / _rotTimer, 1.0f);

                Vector3 angle = info.trackTrans.eulerAngles;
                angle.y = Mathf.Lerp(info.startingAngle, info.targetAngle, completion);
                info.trackTrans.eulerAngles = angle;

                if (info.timer >= _rotTimer)
                {
                    _rotInfos.RemoveAtSwapBack(i);
                }
            }

            yield return null;
        }

        _isInRotation = false;
    }

    public void OnRotateTileMouse(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        TryRotateTile(Input.mousePosition);
    }

    public void OnRotateTileTouch(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        TryRotateTile(context.ReadValue<Vector2>());
    }
}
