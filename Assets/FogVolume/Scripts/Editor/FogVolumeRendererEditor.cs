﻿
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FogVolumeRenderer))]

public class FogVolumeRendererEditor : Editor
{
    FogVolumeRenderer _target;
    private Texture2D InspectorImage;
    private GUIStyle HeaderStyle, BodyStyle;
    private static bool ShowInspectorTooltips
    {
        get { return EditorPrefs.GetBool("ShowInspectorTooltips"); }
        set { EditorPrefs.SetBool("ShowInspectorTooltips", value); }
    }
    GUIContent VariableField(string VariableName, string Tooltip)
    {
        return new GUIContent(VariableName, ShowInspectorTooltips ? Tooltip : "");
    }


    void OnEnable()
    {
        _target = (FogVolumeRenderer)target;
        InspectorImage = Resources.Load("InspectorImage", typeof(Texture2D)) as Texture2D;
        HeaderStyle = new GUIStyle();
        HeaderStyle.normal.background = InspectorImage;
        BodyStyle = new GUIStyle();
        // BodyStyle.normal.background = (Texture2D)Resources.Load("RendererInspectorBody");
        if (EditorGUIUtility.isProSkin)
            BodyStyle.normal.background = (Texture2D)Resources.Load("RendererInspectorBodyBlack");
        else
            BodyStyle.normal.background = (Texture2D)Resources.Load("RendererInspectorBodyBright");
    }

    public override void OnInspectorGUI()
    {

        serializedObject.Update();
        GUILayout.Space(10);
        // GUILayout.Box(InspectorImage, GUILayout.ExpandWidth(true));
        GUILayout.BeginVertical(HeaderStyle);

        // EditorGUILayout.HelpBox(BodyStyle.name, MessageType.None);
        GUILayout.Space(EditorGUIUtility.currentViewWidth / 4 - 10);
        //end header        
        GUILayout.EndVertical();
        //begin body
        GUILayout.BeginVertical(BodyStyle);

        EditorGUI.BeginChangeCheck();
        Undo.RecordObject(_target, "Fog volume Renderer parameter");

        GUILayout.Space(20);
        // GUILayout.BeginVertical("box");
        _target._Downsample = EditorGUILayout.IntSlider("Downscale", _target._Downsample, 0, 16);
        if (_target._Downsample>0)
   //     _target.RenderableInSceneView = EditorGUILayout.Toggle(VariableField("Render In Scene View", "Makes it visible for reflection probes and Scene viewport"), _target.RenderableInSceneView);

        //GUILayout.EndVertical();//end box
        if (_target._Downsample > 0)
            _target._BlendMode = (FogVolumeRenderer.BlendMode)EditorGUILayout.EnumPopup("Blend Mode", _target._BlendMode);
        EditorGUILayout.HelpBox("Resolution: " + _target.FogVolumeResolution, MessageType.None);
        if (_target._Downsample > 0)
        {

            GUILayout.BeginVertical("box");
            _target.GenerateDepth = EditorGUILayout.Toggle(VariableField("Depth", "Compute Depth when rendered in low-res and textured volumes collide with scene elements"), _target.GenerateDepth);
            if (_target.GenerateDepth)
                _target.BilateralUpsampling = EditorGUILayout.Toggle(VariableField("Edge-aware upscale", "Minimize the aliasing at the edges of colliding elements") , _target.BilateralUpsampling);


            if (_target.BilateralUpsampling && _target.GenerateDepth)
            {
                _target.upsampleDepthThreshold = EditorGUILayout.Slider("Depth Threshold", _target.upsampleDepthThreshold, 0, .01f);
                _target.ShowBilateralEdge = EditorGUILayout.Toggle("Show edge mask", _target.ShowBilateralEdge);
                _target.USMode = (FogVolumeCamera.UpsampleMode)EditorGUILayout.EnumPopup("Method", _target.USMode);
            }
            GUILayout.EndVertical();//end box

            #region DeNoise
            if (_target._Downsample > 0)
            {
                GUILayout.BeginVertical("box");
                _target.TAA = EditorGUILayout.Toggle("DeNoise", _target.TAA);
                if (_target.TAA && _target.HDR)
                    EditorGUILayout.HelpBox("Color output is in LDR", MessageType.None);
                GUILayout.EndVertical();//end box

            }
            #endregion

          //  #region Stereo
            //if ( _target._Downsample > 0)
            //{
            //    GUILayout.BeginVertical("box");
            //    _target.useRectangularStereoRT = EditorGUILayout.Toggle("Rectangular stereo", _target.useRectangularStereoRT);
            //    GUILayout.EndVertical();//end box
            //}
          //  #endregion


        }

        EditorGUI.EndChangeCheck();
        EditorGUILayout.HelpBox("Fog Volume 3.2.1 August 2017", MessageType.None);
        GUILayout.EndVertical();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(_target);
        }



        serializedObject.ApplyModifiedProperties();
    }


}
