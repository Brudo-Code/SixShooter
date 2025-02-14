/*
 *  Spring UV Controller - by Laszlo Savanya
 *  
 *  This script controls the custom spring mesh's UV coordinates to simulate spring mechanism.
 *  Script only allows mesh to be scaled along the Z axis between a minimum (pressure) and maximum (tension) values, and calculates a material UV scale.
 *  Use attached mesh for best results!
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringUVController : MonoBehaviour
{
    Transform tr;
    MeshRenderer mRenderer;

    [System.Serializable]
    public class ScaleConnection
    {
        public float UVScale;
        public float localTransformScale;
    }
    // Settings for fully pressed spring
    public ScaleConnection scalesAtFullPressure;
    // Settings for fully pulled spring
    public ScaleConnection scalesAtFullTension;
    // Use Debug mode on Mesh Renderer to see shader's main texture name! (If it is "_BaseTexture" or "_MainTexture" for example.)
    public string MaterialTextureName;
    
    void Start()
    {
        // Initializing transform and meshrenderer.
        tr = transform;
        mRenderer = tr.GetComponent<MeshRenderer>();
    }
        
    void Update()
    {
        ClampScales();
        float uvScale = EvaluateUVScales();

        // Texture UV scale is set between the current lerp vale of minimum and maximum transform scale.
        mRenderer.material.SetTextureScale(MaterialTextureName, new Vector2(1f, uvScale));
        // Texture UV offset is set in correlation with scale, so that the spring texture is always centered (spring axis is always at V coordinate 0.5).
        mRenderer.material.SetTextureOffset(MaterialTextureName, new Vector2(0f, -uvScale + uvScale * 0.5f + 0.5f));
    }

    // Mesh's scale is clamped between minimum and maximum transform scale settings.
    void ClampScales()
    {
        if (tr.localScale.z < scalesAtFullPressure.localTransformScale)
            tr.localScale = new Vector3(1f, 1f, scalesAtFullPressure.localTransformScale);
        else if (tr.localScale.z > scalesAtFullTension.localTransformScale)
            tr.localScale = new Vector3(1f, 1f, scalesAtFullTension.localTransformScale);
    }

    // UV scale is calculated in correlation with the transform scale.
    float EvaluateUVScales()
    {
        float scaleValue = Mathf.InverseLerp(scalesAtFullPressure.localTransformScale, scalesAtFullTension.localTransformScale, tr.localScale.z);
        return Mathf.Lerp(scalesAtFullPressure.UVScale, scalesAtFullTension.UVScale, scaleValue);
    }
}
