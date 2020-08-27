using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Copies the scale of this object's transform. And repeats it with a delay to other objects.
*/
namespace AudioVisualizer
{
    public class Repeater : MonoBehaviour 
    {
        //____________Public Variables

        public float delay = 0.2f;
        public List<GameObject> objects;

        public RepeaterType type = RepeaterType.Translate;
        public enum RepeaterType { Rotate,Scale, Translate };
        //____________Delegates/Actions

        //____________Protected Variables

        List<Vector3> oldValues;
        float timer = 0.0f;
        float maxTime;
        float[] delays;
        int[] delayIndices; // indices into the oldScales buffer, based on delay
        Vector3[] startPositions;
        bool fullBuffer = false;
        bool initialized = false;

        //____________Private Variables

        /*________________Monobehaviour Methods________________*/

      
        private void Update()
        {
            if(!initialized)
            {
                return;
            }

            //fill the buffer up with values!
            switch(type)
            {
                case RepeaterType.Translate:
                    FillBuffer(this.transform.position);
                    TranslateObjects();
                    break;
                case RepeaterType.Scale:
                    FillBuffer(this.transform.localScale);
                    ScaleObjects();
                    break;
                case RepeaterType.Rotate:
                    FillBuffer(this.transform.rotation.eulerAngles);
                    break;
                default:
                break;
            }

            


        }

        /*________________Public Methods________________*/

        public void Initialize()
        {
            oldValues = new List<Vector3>();
            initialized = true;
            maxTime = objects.Count * delay;

            //find the index for each object to use into our databuffer.
            delayIndices = new int[objects.Count];
            delays = new float[objects.Count];
            for(int i = 0; i < objects.Count; i++)
            {
                delays[i] = delay * i;
            }

            startPositions = new Vector3[objects.Count];
            for(int i = 0; i < objects.Count; i++)
            {
                startPositions[i] = objects[i].transform.localPosition;
            }
        }

        /*________________Protected Methods________________*/

        /*________________Private Methods________________*/

        /// <summary>
        /// Scalle objects in the grid, base on their index into the data buffer.
        /// </summary>
        void ScaleObjects()
        {
            //scale other objects
            for (int i = 0; i < objects.Count; i++)
            {
                Vector3 newScale = oldValues[delayIndices[i]]; // desired scale
                Vector3 scaleDelta = newScale - objects[i].transform.localScale; // difference in scale
                objects[i].transform.localScale += scaleDelta; //make the change
                objects[i].transform.localPosition = objects[i].transform.localPosition + Vector3.up * scaleDelta.y * 0.5f; //move the object so it looks stationary
            }
        }

        /// <summary>
        /// Translate objects in the grid, base on their index into the data buffer.
        /// </summary>
        void TranslateObjects()
        {
            //scale other objects
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].transform.localPosition = new Vector3(startPositions[i].x, oldValues[delayIndices[i]].y, startPositions[i].z); // desired position
            }
        }

        void FillBuffer(Vector3 value)
        {
            //fill up our buffer of scales from the past
            if (timer < maxTime)
            {
                oldValues.Add(value);
                timer += Time.deltaTime;
            }
            else // once our buffer is as long as it needs to be, start replacing values.
            {
                oldValues.Add(value); // add a new item
                oldValues.RemoveAt(0); // remove the first item

                if (!fullBuffer)
                {
                    fullBuffer = true;
                    //calculate the index for each object to use into the oldscales array.
                    int indexSpacing = oldValues.Count / objects.Count;
                    for (int i = 0; i < objects.Count; i++)
                    {
                        delayIndices[i] = indexSpacing * (objects.Count-1-i);
                    }
                }
            }
        }

    }
}
