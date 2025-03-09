using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRotator : MonoBehaviour
{
    [SerializeField] private LayerMask _interactableMask;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 50.0f, _interactableMask))
            {
                Track track = hit.transform.parent.GetComponent<Track>();
                if (track.IsAligned)
                    track.RotateTile();
                Debug.DrawRay(ray.origin, ray.direction * 50.0f, Color.green, 1.0f);
            }
            else
                Debug.DrawRay(ray.origin, ray.direction * 50.0f, Color.red, 1.0f);
        }
    }
}
