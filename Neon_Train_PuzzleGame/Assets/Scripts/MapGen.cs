using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    [SerializeField] private int _width = 10;
    [SerializeField] private int _length = 10;
    [SerializeField] private float _floorHeight = 0.5f;

    [SerializeField] private List<GameObject> _tilePrefabs = new List<GameObject>();
    [SerializeField] private GameObject _floorPrefab;

    public List<Track> _tracks;

    void Awake()
    {
        // _tracks = GetComponentsInChildren<Track>().ToList();
        // for (int i = 0; i < _tracks.Count; i++)
        // {
        //     // tracks[i].Init(i);
        // }
    }

    public void GenerateMap()
    {
        ClearMap();

        Transform _floorTrans = Instantiate(
                            _floorPrefab,
                            new Vector3(transform.position.x + ((float)_width / 2) - 0.5f,
                                        transform.position.y + -0.25f,
                                        transform.position.z + ((float)_length / 2) - 0.5f),
                            Quaternion.identity, transform).transform;
        _floorTrans.localScale = new Vector3(_width, _floorHeight, _length);
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _length; z++)
            {
                int randomIndex = Random.Range(0, _tilePrefabs.Count);
                Track track = Instantiate(_tilePrefabs[randomIndex], new Vector3(transform.position.x + x, transform.position.y + 0.25f, transform.position.z + z), Quaternion.identity, transform).GetComponent<Track>();
                _tracks.Add(track);

                // track.Init(x * _length + z);
                // if (Random.Range(0.0f, 1.0f) > 0.8f)
                //     track.ToggleButtonTrack();
            }
        }
    }

    public void ClearMap()
    {
        for (int i = _tracks.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(_tracks[i].gameObject);
        }

        _tracks.Clear();

        if (transform.GetChild(0) is Transform t)
            DestroyImmediate(t);
    }
}
