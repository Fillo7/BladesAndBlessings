using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionHandler : MonoBehaviour
{
    private float transparencyLevel = 0.4f;

    private float timer = 0.0f;
    private float transparencyDuration = 0.15f;

    private Renderer[] objectRenderers = null;
    private List<GameObject> transparentObjects = new List<GameObject>();

    void Awake()
    {
        objectRenderers = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < objectRenderers.Length; i++)
        {
            Material material = new Material(objectRenderers[i].material);
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

            GameObject transparentObject = Instantiate(objectRenderers[i].gameObject, objectRenderers[i].gameObject.transform.parent);
            Renderer transparentRenderer = transparentObject.GetComponent<Renderer>();
            transparentRenderer.material = material;
            transparentRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            transparentObjects.Add(transparentObject);

            objectRenderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > transparencyDuration)
        {
            for (int i = 0; i < objectRenderers.Length; i++)
            {
                objectRenderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                Destroy(transparentObjects[i].gameObject);
            }
            Destroy(this);
        }
    }

    public void ResetTimer()
    {
        timer = 0.0f;
    }
}
