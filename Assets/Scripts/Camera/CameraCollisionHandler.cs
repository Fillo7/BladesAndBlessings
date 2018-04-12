using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionHandler : MonoBehaviour
{
    private float currentTransparency = 0.5f;
    private float targetTransparency = 0.5f;
    private float transparencyDuration = 0.1f;

    private Renderer[] objectRenderers = null;
    private List<Color> oldColors = new List<Color>();

    void Awake()
    {
        objectRenderers = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < objectRenderers.Length; i++)
        {
            oldColors.Add(objectRenderers[i].material.color);
        }
    }

    void Update()
    {
        if (currentTransparency < 1.0f)
        {
            for (int i = 0; i < objectRenderers.Length; i++)
            {
                Material material = objectRenderers[i].material;
                material.SetFloat("_Mode", 3);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;

                Color materialColor = material.color;
                materialColor.a = currentTransparency;
                material.color = materialColor;
                objectRenderers[i].material = material;
            }
        }
        else
        {
            for (int i = 0; i < objectRenderers.Length; i++)
            {
                Material material = objectRenderers[i].material;
                material.SetFloat("_Mode", 0);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;

                material.color = oldColors[i];
                objectRenderers[i].material = material;
            }
            Destroy(this);
        }
        currentTransparency += ((1.0f - targetTransparency) * Time.deltaTime) / transparencyDuration;
    }

    public void SetTransparency()
    {
        currentTransparency = targetTransparency;
    }
}
