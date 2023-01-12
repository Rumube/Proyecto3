using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blur : MonoBehaviour
{
    public Material _Material;

    [Range(0.1f, 10)]
    public float _BlurRadius = 0.1f;

    [Range(0, 10)]
    public int downSample = 1;

    [Range(0, 10)]
    public int iteration = 1;

    public void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (_Material)
        {
            RenderTexture rt1 = RenderTexture.GetTemporary(src.width >> downSample, src.height >> downSample, 0, src.format);
            RenderTexture rt2 = RenderTexture.GetTemporary(src.width >> downSample, src.height >> downSample, 0, src.format);

            _Material.SetFloat("_BlurRadius", _BlurRadius);

            Graphics.Blit(src, rt1, _Material);

            for (int i = 0; i < iteration; i++)
            {
                Graphics.Blit(rt1, rt2, _Material);
                Graphics.Blit(rt2, rt1, _Material);
            }

            Graphics.Blit(rt1, dst, _Material);

            RenderTexture.ReleaseTemporary(rt1);
            RenderTexture.ReleaseTemporary(rt2);
        }
    }
}
