//using AudioVisualizer;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.VFX;

//[RequireComponent(typeof(VisualEffect))]
//public class AudioVFX : MonoBehaviour
//{

//    [SerializeField] AudioViz  m_audioViz;

//    VisualEffect m_vfx;
//    [SerializeField] string m_radiusPropName = "Radius";
//    [SerializeField] float m_radiusMultiplier = 10;
//    int m_radiusPropID;
//    [SerializeField] string m_intensityPropName = "Intensity";
//    [SerializeField] float m_intensityMultiplier = 10;
//    int m_intensityPropID;

//    [SerializeField] Vector2 m_remap = new Vector2(0, 1);

//    private void Awake()
//    {
//        m_vfx = GetComponent<VisualEffect>();
//        m_radiusPropID = Shader.PropertyToID(m_radiusPropName);
//        m_intensityPropID = Shader.PropertyToID(m_intensityPropName);

//    }
//    private void Update()
//    {
//       // float buffer = m_audioViz.AmplitudeBuffer;

//        float radius = buffer * m_radiusMultiplier;
//        radius = Remap(radius, m_remap.x, m_remap.y);
//        m_vfx.SetFloat(m_radiusPropID, radius);

//        float intensity = (int)(buffer * m_intensityMultiplier);
//        intensity = Remap(intensity, m_remap.x, m_remap.y);
//        m_vfx.SetFloat(m_intensityPropID, intensity);
//    }
//    float Remap (float val, float min, float max)
//    {
//        return (val * (max - min)) + val * min;
//    }

//}
