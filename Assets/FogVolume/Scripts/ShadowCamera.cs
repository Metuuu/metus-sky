﻿using UnityEngine;
[ExecuteInEditMode]
public class ShadowCamera : MonoBehaviour
{
    Camera ThisCamera = null;
    GameObject Dad = null;
    FogVolume Fog = null;
    public RenderTexture RT_Opacity, RT_OpacityBlur, RT_PostProcess;
    public RenderTexture GetOpacityRT()
    {
        return RT_Opacity;
    }
    public RenderTexture GetOpacityBlurRT()
    {
        return RT_OpacityBlur;
    }

    [System.Serializable]
    public enum TextureSize
    {
        _64 = 64,
        _128 = 128,
        _256 = 256,
        _512 = 512,
        _1024 = 1024
    };


    public TextureSize SetTextureSize
    {
        set
        {
            if (value != textureSize)
                SetQuality(value);
        }
        get
        {
            return textureSize;
        }
    }
    public TextureSize textureSize = TextureSize._128;
    /// Blur iterations - larger number means more blur.
    [Range(0, 10)]
    public int iterations = 3;

    /// Blur spread for each iteration. Lower values
    /// give better looking blur, but require more iterations to
    /// get large blurs. Value is usually between 0.5 and 1.0.
    [Range(0.0f, 1)]
    public float blurSpread = 0.6f;
    public int Downsampling = 2;
    void SetQuality(TextureSize value)
    {
        textureSize = value;
        // print((int)textureSize);
    }
    Shader blurShader = null;
    Shader PostProcessShader = null;
    Material blurMaterial = null;
    Material postProcessMaterial = null;
    protected Material BlurMaterial
    {
        get
        {
            if (blurMaterial == null)
            {
                blurMaterial = new Material(blurShader);
                blurMaterial.hideFlags = HideFlags.DontSave;
            }
            return blurMaterial;
        }
    }
    protected Material PostProcessMaterial
    {
        get
        {
            if (postProcessMaterial == null)
            {
                postProcessMaterial = new Material(PostProcessShader);
                postProcessMaterial.hideFlags = HideFlags.DontSave;
            }
            return postProcessMaterial;
        }
    }
    protected void GetRT(ref RenderTexture rt, int size, string name)
    {
        // Release existing one
        ReleaseRT(rt);
        rt = RenderTexture.GetTemporary(size, size, 0, RenderTextureFormat.R8, RenderTextureReadWrite.Linear);
        rt.filterMode = FilterMode.Bilinear;
        rt.name = name;
        rt.wrapMode = TextureWrapMode.Repeat;

    }
    public void ReleaseRT(RenderTexture rt)
    {
        if (rt != null)
        {
            RenderTexture.ReleaseTemporary(rt);
            rt = null;
        }
    }

    // Performs one blur iteration.
    public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
    {
        float off = 0.5f + iteration * blurSpread;
        Graphics.BlitMultiTap(source, dest, BlurMaterial,
                               new Vector2(-off, -off),
                               new Vector2(-off, off),
                               new Vector2(off, off),
                               new Vector2(off, -off)
            );
    }

    // Downsamples the texture to a quarter resolution.
    private void DownSample(RenderTexture source, RenderTexture dest)
    {
        float off = 1.0f;
        Graphics.BlitMultiTap(source, dest, BlurMaterial,
                               new Vector2(-off, -off),
                               new Vector2(-off, off),
                               new Vector2(off, off),
                               new Vector2(off, -off)
            );
    }
    RenderTexture RT_OpacityBlur2;
    void Blur(RenderTexture Input, int BlurRTSize)
    {
        GetRT(ref RT_OpacityBlur, BlurRTSize, "Shadow blurred");
        GetRT(ref RT_OpacityBlur2, BlurRTSize, "Shadow blurred");
        // Copy source to the smaller texture.
        DownSample(Input, RT_OpacityBlur);
        //Graphics.Blit(Input, RT_OpacityBlur);

        // Blur the small texture
        for (int i = 0; i < iterations; i++)
        {
            FourTapCone(RT_OpacityBlur, RT_OpacityBlur2, i);
            Graphics.Blit(RT_OpacityBlur2, RT_OpacityBlur);
        }


        Shader.SetGlobalTexture("RT_OpacityBlur", RT_OpacityBlur);
        Fog.RT_OpacityBlur = RT_OpacityBlur;
    }

   

    void RenderShadowMap()
    {//esto está hecho una puta mierda
     //  if (FogVolumeUtilities.ExtensionMethods.TimeSnap(SkipFrames))
     //  {
     //ideally, a cheaper version should be rendered here. 
     //So lets render a version with no lighting stuff   

        Fog.FogVolumeShader.maximumLOD = 100;
      
            SetQuality(textureSize);
       
      
            GetRT(ref RT_Opacity, (int)textureSize, "Opacity");

            ThisCamera.targetTexture = RT_Opacity;

           // print(Fog.ShadowCameraSkippedFrames);


       
            ThisCamera.Render();
            Fog.RT_Opacity = RT_Opacity;

           // Shader.SetGlobalTexture("RT_Opacity", RT_Opacity);


        if (RT_Opacity != null)
        {
            GetRT(ref RT_PostProcess, (int)textureSize, "Shadow PostProcess");
            PostProcessMaterial.SetFloat("ShadowColor", Fog.ShadowColor.a);
            // PostProcessMaterial.SetFloat("_jitter", Fog._jitter);
            Graphics.Blit(RT_Opacity, RT_PostProcess, PostProcessMaterial);
            // ThisCamera.targetTexture = null;
            Graphics.Blit(RT_PostProcess, RT_Opacity);
            //RenderTexture.ReleaseTemporary(RT_Opacity);
            if (iterations > 0)
            {
                Blur(RT_Opacity, (int)textureSize >> Downsampling);
            }
            else
            {
                Shader.SetGlobalTexture("RT_OpacityBlur", RT_Opacity);
                Fog.RT_OpacityBlur = RT_Opacity;

            }
            Fog.RT_Opacity = RT_Opacity;
        }
        //Shader.SetGlobalTexture("RT_Opacity", RT_Opacity);
        BlurMaterial.SetFloat("ShadowColor", Fog.ShadowColor.a);

       // back to normal
        Fog.FogVolumeShader.maximumLOD = 600;
    }

    void ShaderLoad()
    {
        blurShader = Shader.Find("Hidden/Fog Volume/BlurEffectConeTap");
        if (blurShader == null) print("Hidden / Fog Volume / BlurEffectConeTap #SHADER ERROR#");

        PostProcessShader = Shader.Find("Hidden/Fog Volume/Shadow Postprocess");
        if (PostProcessShader == null) print("Hidden/Fog Volume/Shadow Postprocess #SHADER ERROR#");

    }

    void OnEnable()
    {

        ShaderLoad();
        Dad = transform.parent.gameObject;
        Fog = Dad.GetComponent<FogVolume>();
        ThisCamera = gameObject.GetComponent<Camera>();

        CameraTransform();

    }

    public void CameraTransform()
    {
        if (ThisCamera != null)
        {
            ThisCamera.orthographicSize = Dad.GetComponent<FogVolume>().fogVolumeScale.x / 2;
            ThisCamera.transform.position = Dad.transform.position;
            ThisCamera.farClipPlane = Fog.fogVolumeScale.y + Fog.shadowCameraPosition;
            //  print(ThisCamera.farClipPlane);
            Vector3 VerticalTranslate = new Vector3(0, 0, Fog.fogVolumeScale.y / 2 - Fog.shadowCameraPosition);
            //  print(Fog.ShadowCameraPosition);

            ThisCamera.transform.Translate(VerticalTranslate, Space.Self);
            Quaternion target = Quaternion.Euler(90, 0, 0);
            ThisCamera.transform.rotation = Dad.transform.rotation * target;

            ThisCamera.enabled = false;

            if (Fog.SunAttached)
            {
                Fog.Sun.transform.rotation = Dad.transform.rotation * target;

            }


        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Fog.IsVisible && Fog.CastShadows)//3.1.7
        {
            if (FogVolumeUtilities.ExtensionMethods.TimeSnap(Fog.ShadowCameraSkippedFrames))
                RenderShadowMap();
#if UNITY_EDITOR
            CameraTransform();
#endif
        }
    }

    void OnPrecull()
    {

    }

    void OnDisable()
    {
        RenderTexture.active = null;
        ThisCamera.targetTexture = null;
        if (RT_Opacity) DestroyImmediate(RT_Opacity);
        if (RT_OpacityBlur) DestroyImmediate(RT_OpacityBlur);
        if (RT_PostProcess) DestroyImmediate(RT_PostProcess);
        if (blurMaterial) DestroyImmediate(blurMaterial);
        if (postProcessMaterial) DestroyImmediate(postProcessMaterial);

    }
}
