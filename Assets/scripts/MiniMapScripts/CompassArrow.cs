using System.Collections;
using Assets.scripts.MiniMapScripts.AStar;
using Assets.scripts.objectives;
using UnityEngine;

/* Attach this class to player's arrow object
 * This class gets the PathArray and sets the next Target position to look at
*/

namespace Assets.scripts.MiniMapScripts
{
    public class CompassArrow : MonoBehaviour
    {
        private NodeManager nodemanager;
        public GameObject RendedArrow;
        public Vector3 TargetPosition;
        private ArrayList _pathnodes;
        public static bool TempArrow = true;
        /// <summary>
        /// removing the y value so that arrow does not point towards 3D point.
        /// </summary>
        private Vector3 _lookatposition;
        private static CompassArrow _sInstance;
        public static CompassArrow Instance
        {
            get
            {
                if (_sInstance == null)
                {
                    _sInstance = FindObjectOfType(typeof(CompassArrow)) as CompassArrow;
                    if (_sInstance == null)
                    {
                        Debug.Log("Could not locate CompassArrow");
                    }
                }
                return _sInstance;
            }
        }

        //Set from PathFindingController script once it finds a Path
        public void SetPathNodes(ArrayList list)
        {
            _pathnodes = list;

        }

        private void GetTargetPosition()
        {
            // for each node in the pathlist
            for (var i = 0; i < _pathnodes.Count; i++)
            {
                // get each node
                var node = (Node)_pathnodes[i];
                // check the distance between player and each node
                var Distance = Mathf.Abs(Vector3.Distance(transform.position, node.position));
                // if player is close to one node
                if (Distance < 15)
                {



                    //  and if next node is not near the final destination then, choose to point towards next node
                    if (i < _pathnodes.Count - 1)
                    {
                        var nextnode = (Node)_pathnodes[i + 1];

                        if (i > 0)
                        {
                            var previousnode = (Node)_pathnodes[i - 1];

                            var row0 = previousnode.ToString()[5];
                            var col0 = previousnode.ToString()[7];

                            var row = node.ToString()[5];
                            var col = node.ToString()[7];

                            var row2 = nextnode.ToString()[5];
                            var col2 = nextnode.ToString()[7];

                            if (row2 != row && col2 == col && col2 != col0)
                            {
                                RendedArrow.SetActive(true);
                            }
                            else if (row2 == row && col2 != col && row2 != row0)
                            {
                                RendedArrow.SetActive(true);
                            }
                        }
                        else
                        {
                            RendedArrow.SetActive(true);
                        }

                        TargetPosition = nextnode.position;


                    }
                    // else if next node is final destination then get the postion from Objective manager
                    else
                    {
                        TargetPosition = ObjectiveManager.Instance.GetCurrentObjective.Location;
                    }


                }
                else if (nodemanager.GetNearestNode() != null && node.name == nodemanager.GetNearestNode().name)
                {
                    if (TempArrow)
                        RendedArrow.SetActive(false);
                }
            }
        }

        private void Start()
        {
            _pathnodes = new ArrayList();
            TargetPosition = Vector3.zero;
            nodemanager = NodeManager.Instance;
        }

        /// <summary>
        /// set from Game Manager which gets data about to show arrow or not from start scene settings
        /// </summary>
        public void ToggleArrowDisplay(bool show)
        {
            transform.FindChild("Group1").FindChild("Mesh1").renderer.enabled = show;
        }

        private void Update()
        {
            GetTargetPosition();
            _lookatposition.x = TargetPosition.x;
            _lookatposition.y = 2;
            _lookatposition.z = TargetPosition.z;
            transform.LookAt(_lookatposition);
        }
    }
}