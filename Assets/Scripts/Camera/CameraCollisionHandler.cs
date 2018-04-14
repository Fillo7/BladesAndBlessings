using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionHandler : MonoBehaviour
{
    private float transparencyLevel = 0.5f;

    private float timer = 0.0f;
    private float transparencyDuration = 0.15f;

    private Renderer[] objectRenderers = null;
    private List<Color> oldColors = new List<Color>();

    void Awake()
    {
        objectRenderers = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < objectRenderers.Length; i++)
        {
            oldColors.Add(objectRenderers[i].material.color);
        }

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
            materialColor.a = transparencyLevel;
            material.color = materialColor;
            objectRenderers[i].material = material;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > transparencyDuration)
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
    }

    public void ResetTimer()
    {
        timer = 0.0f;
    }
}
