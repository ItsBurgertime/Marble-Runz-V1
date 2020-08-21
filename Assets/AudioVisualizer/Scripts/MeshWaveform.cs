using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
*
*/
namespace AudioVisualizer
{
    public class KeyValuePair
    {
        public int key;
        public Vector3 value;

        public KeyValuePair(int key, Vector3 value)
        {
            this.key = key;
            this.value = value;
        }
    }

    [RequireComponent(typeof(MeshFilter))]
    public class MeshWaveform : Waveform
    {

        //____________Public Variables

        public Transform rippleOrigin;
        public WaveformType waveformType = WaveformType.Ripple;
        public Vector3 waveformAxis = Vector3.up;
        public bool debug;

        public enum WaveformType
        {
            Ripple,
            Spherical
        }

        //____________Delegates/Actions

        //____________Protected Variables

        //____________Private Variables

        Mesh mesh;
        Vector3 localDirection;
        Vector3[] vertices;
        Vector3[] modifiedVertices;
        List<KeyValuePair> orderedVertices;
        float[] audioSamples;

        /*________________Monobehaviour Methods________________*/

        private void Awake()
        {
            mesh = GetComponent<MeshFilter>().sharedMesh;
            vertices = mesh.vertices;
            // need to maintain a dictionary of ordered verts vs verts in the mesh
            orderedVertices = new List<KeyValuePair>();
            for(int i = 0; i < vertices.Length; i++)
            {

                orderedVertices.Add(new KeyValuePair(i,vertices[i]));
            }


            modifiedVertices = new Vector3[vertices.Length];
        }

        private void OnDisable()
        {
            mesh.vertices = vertices;
        }

        private void Update()
        {
            if (frequencyRange == FrequencyRange.Decibal)
            {
                audioSamples = AudioSampler.instance.GetAudioSamples(audioIndex, vertices.Length, abs, useAudioFile);
            }
            else
            {
                audioSamples = AudioSampler.instance.GetFrequencyData(audioIndex, frequencyRange, vertices.Length, abs, useAudioFile);
            }
            Draw();

            if(rippleOrigin.hasChanged)
            {
                OrderVertices();
                rippleOrigin.hasChanged = false;
            }


            //start and end index for our line draw for loop

            /*
            if (frequencyRange == FrequencyRange.Decibal)
            {
                audioSamples = AudioSampler.instance.GetAudioSamples(audioIndex, lineAtt.lineSegments, abs, useAudioFile);
            }
            else
            {
                audioSamples = AudioSampler.instance.GetFrequencyData(audioIndex, frequencyRange, lineAtt.lineSegments, abs, useAudioFile);
            }
            */
        }

        private void OnDrawGizmos()
        {
            if(debug)
            {
                if (rippleOrigin != null)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(rippleOrigin.position, 0.01f);
                }

                if(orderedVertices != null)
                {
                    float percent = 0;
                    for (int i = 0; i < orderedVertices.Count; i++)
                    {
                        percent = (float)i / (orderedVertices.Count - 1);
                        Gizmos.color = new Color(percent, 1-percent, 0);
                        Gizmos.DrawSphere(transform.TransformPoint(orderedVertices[i].value), 0.01f);
                    }
                }
            }

        }

        /*________________Public Methods________________*/

        /*________________Protected Methods________________*/

        /*________________Private Methods________________*/

        void OrderVertices()
        {
            Debug.Log("OrderingVertices");
            Vector3 localPosOfRippleOrigin = transform.InverseTransformPoint(rippleOrigin.position);
            orderedVertices = orderedVertices.OrderBy(keyValuePair => Vector3.Distance(keyValuePair.value, localPosOfRippleOrigin)).ToList();

        }

        void Draw()
        {
            switch(waveformType)
            {
                case WaveformType.Ripple:
                    DrawRipple();
                    break;
                case WaveformType.Spherical:
                    DrawSpherical();
                    break;
            }
        }

        void DrawRipple()
        {
            //localDirection = transform.TransformDirection(waveformAxis);
            for (int i = 0; i < orderedVertices.Count; i++)
            {
                modifiedVertices[i] = vertices[i] + waveformAxis * audioSamples[orderedVertices[i].key] * waveformAmplitude;
            }
            mesh.vertices = modifiedVertices;
        }

        void DrawSpherical()
        {

        }

        void DrawScaleTest()
        {
            float rms = AudioSampler.instance.GetRMS(audioIndex, useAudioFile);

            for (int i = 0; i < vertices.Length; i++)
            {
                modifiedVertices[i] = vertices[i] * rms;
            }

            mesh.vertices = modifiedVertices;
        }

        Vector3 Multiply(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

    }

}
