using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Blur_Telescope : MonoBehaviour
{
    [SerializeField]
    private Volume _volume;
    private DepthOfField _depth;
    [SerializeField]
    private float _focusDistance;
    // Update is called once per frame
    void Update()
    {
        _volume.profile.TryGet(out _depth);
        {
            _depth.focusDistance.value = _focusDistance;
        }
    }
}
