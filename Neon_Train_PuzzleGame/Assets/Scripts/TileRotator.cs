using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRotator : MonoBehaviour
{
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private float _rotationDuration = 0.5f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 50.0f, _interactableMask))
            {
                StartCoroutine(TileRotationAnimation(hit.transform));
                Debug.DrawRay(ray.origin, ray.direction * 50.0f, Color.green, 1.0f);
            }
            else
                Debug.DrawRay(ray.origin, ray.direction * 50.0f, Color.red, 1.0f);
        }
    }


    private IEnumerator TileRotationAnimation(Transform tileTrans)
    {
        tileTrans.GetComponent<Collider>().enabled = false;
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
        tileTrans.GetComponent<Collider>().enabled = true;
    }
}
