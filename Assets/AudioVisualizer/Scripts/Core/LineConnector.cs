using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*
*/
namespace AudioVisualizer
{


public class LineConnector : MonoBehaviour 
{

    //____________Public Variables

     public List<Transform> targets;
     public LineAttributes line;

        //____________Delegates/Actions

        //____________Protected Variables

        //____________Private Variables

        LineRenderer lineRenderer;

        /*________________Monobehaviour Methods________________*/

        private void Start()
        {
            lineRenderer = line.GetLine();
            lineRenderer.positionCount = targets.Count;
            lineRenderer.transform.SetParent(this.transform);
        }

        private void Update()
        {
            for(int i = 0; i <targets.Count; i++)
            {
                lineRenderer.SetPosition(i, targets[i].position);
            }
        }
        /*________________Public Methods________________*/



        /*________________Protected Methods________________*/

        /*________________Private Methods________________*/



    }

}
