using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioVisualizer
{
    /// <summary>
    /// This class is used together with either an ObjectPositionWaveform, or ObjectScaleWaveform.
    /// It places content in a grid, animates the first row, and uses the "Repeater" class to pass the first row changes down the line.
    /// </summary>
    public class GridWaveform : MonoBehaviour 
    {

        //____________Public Variables

        [Tooltip("Object that get's placed in each cell of the grid")]
        public GameObject cellPrefab;
        [Tooltip("Number for grid cells in the X direction")]
        public int gridCountX;
        [Tooltip("Number for grid cells in the Z direction")]
        public int gridCountZ;
        [Tooltip("Size of the grid in world space")]
        public float gridWidthX = 10.0f;
        [Tooltip("Size of the grid in world space")]
        public float gridWidthZ = 10.0f;
        [Tooltip("Determines if connecting lines go in teh x direction or the z direction")]
        public bool horizontalLines = true;
        public Vector3 startScale = Vector3.one; // starting size for each object.
        public Gradient gradient;
        public LineAttributes line;

        //____________Delegates/Actions

        //____________Protected Variables

        //____________Private Variables
        ObjectPositionWaveform positionWaveform;
        ObjectScaleWaveform scaleWaveform;
        GameObject[,] cells;
        Repeater.RepeaterType repeaterType;

        int lastGridCountX;
        int lastGridCountZ;
        float lastGridWidthX;
        float lastGridWidthZ;
        /*________________Monobehaviour Methods________________*/

        private void Awake()
        {
            scaleWaveform = this.GetComponent<ObjectScaleWaveform>();
            positionWaveform = this.GetComponent<ObjectPositionWaveform>();

            if(scaleWaveform)
            {
                repeaterType = Repeater.RepeaterType.Scale;
            }
            else
            {
                repeaterType = Repeater.RepeaterType.Translate;

            }

            lastGridCountX = gridCountX;
            lastGridCountZ = gridCountZ;
            lastGridWidthX = gridWidthX;
            lastGridWidthZ = gridWidthZ;
            SetupGrid();
        }

        private void Update()
        {
            if(lastGridCountX != gridCountX || lastGridCountZ != gridCountZ ||
                lastGridWidthX != gridWidthX || lastGridWidthZ != gridWidthZ)
            {
                DestroyGrid();
                SetupGrid();
            }

            lastGridCountX = gridCountX;
            lastGridCountZ = gridCountZ;
            lastGridWidthX = gridWidthX;
            lastGridWidthZ = gridWidthZ;
        }

        /*________________Public Methods________________*/

        /*________________Protected Methods________________*/

        /*________________Private Methods________________*/

        void DestroyGrid()
        {
            //clear out waveforms
            if (positionWaveform)
            {
                positionWaveform.objects.Clear();
            }
            if (scaleWaveform)
            {
                scaleWaveform.objects.Clear();
            }

            //destroy grid cells
            int xCount = cells.GetLength(0);
            int zCount = cells.GetLength(1);
            for(int x = 0; x <xCount; x++)
            {
                for(int z= 0; z < zCount; z++)
                {
                    Destroy(cells[x, z]);
                }
            }

        }

        void SetupGrid()
        {
            //get the position for the bottom left corner in the grid.
            Vector3 bottomLeftCorner = this.transform.position - this.transform.forward * gridWidthZ * 0.5f - this.transform.right * gridWidthX * 0.5f;

            // distance between each cell in each dimension
            float deltaX = gridWidthX / (gridCountX - 1);
            float deltaZ = gridWidthZ / (gridCountZ - 1);

            cells = new GameObject[gridCountX, gridCountZ];
            for (int x = 0; x < gridCountX; x++)
            {
                float percent = (float)x / (gridCountX - 1);
                //Debug.Log(percent);
                Color color = gradient.Evaluate(percent);
                for (int z = 0; z < gridCountZ; z++)
                {
                    Vector3 pos = bottomLeftCorner + this.transform.right * x * deltaX + this.transform.forward * z * deltaZ;
                    cells[x, z] = Instantiate(cellPrefab, pos, this.transform.rotation) as GameObject;
                    cells[x, z].transform.SetParent(this.transform);
                    cells[x, z].transform.localScale = startScale;
                    cells[x, z].name = "(" + x + "," + z + ")";
                    cells[x, z].GetComponent<MeshRenderer>().material.color = color;
                    ParticleSystem ps = cells[x, z].GetComponentInChildren<ParticleSystem>();
                    if(ps != null)
                    {
                        ParticleSystem.MainModule main = ps.main;
                        main.startColor = color;
                    }
                    
                    if (z == 0)
                    {
                        //add cells into the waveforms.
                        if(scaleWaveform != null)
                        {
                            scaleWaveform.objects.Add(cells[x, z]);
                        }
                        if(positionWaveform != null)
                        {
                            positionWaveform.objects.Add(cells[x, z]);
                        }
                    }
                }
            }


            //setup repeaters

            for (int x = 0; x < gridCountX; x++)
            {
                Repeater repeater = cells[x, 0].AddComponent<Repeater>();
                repeater.objects = new List<GameObject>();
                repeater.type = repeaterType;
                repeater.delay = 0.1f;

                for (int z = 1; z < gridCountZ; z++)
                {
                    repeater.objects.Add(cells[x, z]);
                }
                repeater.Initialize();
            }

            //setup line connector
            if(horizontalLines)
            {
                for(int z = 0; z < gridCountZ; z++)
                {

                    LineConnector connector = cells[0, z].AddComponent<LineConnector>();
                    connector.targets = new List<Transform>();
                    connector.line = line.Clone();
                    Color c = gradient.Evaluate((float)z / (gridCountZ - 1));
                    c.a = 1;
                    connector.line.startColor = c;
                    c.a = 0;
                    connector.line.endColor = c;

                    for (int x = 0; x < gridCountX; x++)
                    {
                        connector.targets.Add(cells[x,z].transform);
                    }
                }
            }
            else
            {

                for (int x = 0; x < gridCountX; x++)
                {
                    LineConnector connector = cells[x, 0].AddComponent<LineConnector>();
                    connector.targets = new List<Transform>();
                    connector.line = line.Clone();
                    float percent = (float)x / (gridCountX - 1);
                    //Debug.Log(percent);
                    Color c = gradient.Evaluate(percent);
                    c.a = 1;
                    connector.line.startColor = c;
                    c.a = 0;
                    connector.line.endColor = c;
                
                    for (int z = 0; z < gridCountZ; z++)
                    {
                        connector.targets.Add(cells[x, z].transform);
                    }
                }
            }



            if (positionWaveform)
            {
                positionWaveform.Initialize();
            }
            if (scaleWaveform)
            {
                //scaleWaveform.Initialize();
            }
        }

        public static Gradient GetColorGradient(Color c1, Color c2)
        {
            Gradient g = new Gradient();
            GradientColorKey[] gck = new GradientColorKey[2];

            // Populate the color keys at the relative time 0 and 1 (0 and 100%)
            gck[0].color = c1;
            gck[0].time = 0.0f;
            gck[1].color = c2;
            gck[1].time = 1.0f;

            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            GradientAlphaKey[] gak = new GradientAlphaKey[2];
            gak[0].alpha = c1.a;
            gak[0].time = c1.a;
            gak[1].alpha = c2.a;
            gak[1].time = c2.a;

            g.SetKeys(gck, gak);

            return g;
        }

        /// <summary>
        /// Place this waveform on top of a target object.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="height, how far above the target we place the waveform"></param>
        /// <param name="cellsPerMeterX, optional parameter. Adjusts cellCount based on size of the target object"></param>
        /// <param name="cellsPerMeterZ,  optional parameter. Adjusts cellCount based on size of the target object"></param>
        public void PlaceOnGameObject(GameObject target, float height, float cellsPerMeterX = 0, float cellsPerMeterZ = 0)
        {
            // place the grid above an object

            this.transform.position = target.transform.position + Vector3.up * height;
            this.transform.rotation = target.transform.rotation;

            Bounds bounds = target.GetComponent<Collider>().bounds;

            //adjust size of the grid waveform
            gridWidthX = bounds.extents.x * 2;
            gridWidthZ = bounds.extents.z * 2;

            //potentially adjust numCells in each dimension
            if(cellsPerMeterX != 0)
            {
                gridCountX = (int)(cellsPerMeterX/gridWidthX);
            }
            if(cellsPerMeterZ != 0)
            {
                gridCountZ = (int)(cellsPerMeterZ/gridWidthZ);

            }

            DestroyGrid();
            SetupGrid();
        }


    }
}
