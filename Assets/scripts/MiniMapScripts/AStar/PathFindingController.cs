using System;
using System.Collections;
using Assets.scripts.Controller;
using Assets.scripts.GUI;
using Assets.scripts.Manager;
using UnityEngine;

namespace Assets.scripts.MiniMapScripts.AStar
{
    public class PathFindingController : MonoBehaviour {

        public static Node StartNode;
        public static Node GoalNode;
        public static string GoalNodeName;
        public static ArrayList PathArray;
        public static ArrayList InitialObjPath;
        private static ArrayList _pathCalc;
        //public static int Crosses;
        private bool _pathRedesigned;
   
        private NodeManager nodemanager;
        //private LineRenderer line;
        private CompassArrow arrow;

        private DrawObjectiveList drawobjectivelist;
        public static bool showGreenPath = true;
        public Color PathColor;
        private static PathFindingController s_Instance = null;
        public static PathFindingController Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindObjectOfType(typeof(PathFindingController)) as PathFindingController;
                    if (s_Instance == null)
                    {
                        Debug.Log("Could not locate Pathfinding");
                    }
                }
                return s_Instance;
            }
        }
	
        private void Start () {
            nodemanager = NodeManager.Instance;
            PathArray = new ArrayList();
            _pathRedesigned = false;
            InitialObjPath = new ArrayList();
            _pathCalc = new ArrayList();
            //line = GetComponent<LineRenderer>();
            arrow = CompassArrow.Instance;
            drawobjectivelist = DrawObjectiveList.Instance;
            ResetNodes();
        }

        public static int CrossingNodes(Node start, Node goal)
        {
            AStar.ClearPath();
            _pathCalc.Clear();
            _pathCalc = AStar.FindPath(start, goal);
            var path = "CalcGoal: " + goal.name + _pathCalc[0];
            var row0 = _pathCalc[0].ToString()[5];
            var col0 = _pathCalc[0].ToString()[7];

            var row = row0;
            var col = col0;
            var crosses = 0;
            
            for (var i = 1; i < _pathCalc.Count; i++)
            {
                path = path + " - " + _pathCalc[i];
                var row2 = _pathCalc[i].ToString()[5];
                var col2 = _pathCalc[i].ToString()[7];

                if (row2 != row && col2 == col && col2 != col0)
                {
                    crosses++;
                }
                else if (row2 == row && col2 != col && row2 != row0)
                {
                    crosses++;
                }

                row0 = row;
                col0 = col;
                row = row2;
                col = col2;

                //Debug.Log("row: " + row2 + " col: " + col2 + "Crosses: " + Crosses);
            }
            //Debug.Log(path + " crosses: " + crosses);
            return crosses;
        }

        public void FindPath()
        {
            AStar.ClearPath();
            StartNode = nodemanager.GetStartNode();
            GoalNode = nodemanager.GetGoalNode();
            GoalNodeName = GoalNode.name;
            PathArray = AStar.FindPath(StartNode, GoalNode);

            if (!_pathRedesigned)
            {
                InitialObjPath.Clear();
                for (var i = 0; i < PathArray.Count; i++)
                {
                    InitialObjPath.Add(PathArray[i]);
                }
                NavigationTimer.NavTimer = NavigationTimer.NavigationTime*InitialObjPath.Count;
            }
                
            
            //Debug.Log(InitialObjPath.Count);
            var row0 = PathArray[0].ToString()[5];
            var col0 = PathArray[0].ToString()[7];

            var row = row0;
            var col = col0;
            var Cross = 0;

            for (var i = 1; i < PathArray.Count; i++)
            {
                var row2 = PathArray[i].ToString()[5];
                var col2 = PathArray[i].ToString()[7];

                if (row2 != row && col2 == col && col2 != col0)
                {
                    Cross ++;
                }
                else if (row2 == row && col2 != col && row2 != row0)
                {
                    Cross ++;
                }

                row0 = row;
                col0 = col;
                row = row2;
                col = col2;

                //Debug.Log("row: " + row2 + " col: " + col2 + "Crosses: " + Cross);
            }
            //Debug.Log("Current objective: " + path + " crosses: " + Cross);
            arrow.SetPathNodes(PathArray);
            //line.SetVertexCount(PathArray.Count);
            ResetNodes();


            DrawPathLine();
        }
	
        private void Update ()
        {
            RedesignPath();
            //DrawPathLine();

            if (GoalNode != null && nodemanager.GetNearestNode() != null)
            {
                if (nodemanager.GetNearestNode() != GoalNode &&
                    nodemanager.GetNearestNode() != (Node) PathArray[PathArray.Count - 2])
                {
                    NavigationTimer.CanJump = true;
                }
                else
                    NavigationTimer.CanJump = false;
            }
            else
                NavigationTimer.CanJump = true;

        }

        private void ResetNodes()
        {
            var nodes = GameObject.FindGameObjectsWithTag("Node");
            var layer = 18;
            if (PerformanceProcessor.CurrentProfileModel.Difficulty >= 9f)
            {
                layer = 12;
            }

            foreach (GameObject g in nodes)
            {
                if (g.GetComponent<Node>().Direction1 != null)
                {
                    g.GetComponent<Node>().Direction1.SetActive(false);
                    g.GetComponent<Node>().Direction1.layer = layer;
                }
                if (g.GetComponent<Node>().Direction2 != null)
                {
                    g.GetComponent<Node>().Direction2.SetActive(false);
                    g.GetComponent<Node>().Direction2.layer = layer;
                }
                if (g.GetComponent<Node>().Direction3 != null)
                {
                    g.GetComponent<Node>().Direction3.SetActive(false);
                    g.GetComponent<Node>().Direction3.layer = layer;
                }
                if (g.GetComponent<Node>().Direction4 != null)
                {
                    g.GetComponent<Node>().Direction4.SetActive(false);
                    g.GetComponent<Node>().Direction4.layer = layer;
                }
            }
        }

        private void DrawPathLine()
        {
            if (PathArray == null)
            {
                return;
            }

            if (drawobjectivelist.MinimizeObjective && !showGreenPath)
            {
                PathColor.a = 1;
            }

            //PathColor.a = (drawobjectivelist.MinimizeObjective && !showGreenPath) ? 0 : 1;
            //line.gameObject.renderer.material.color = PathColor;
            if (PathArray.Count > 0)
            {
                int index = 1;
                //int index = 0;
                
                foreach (Node node in PathArray)
                {
                    if (index < PathArray.Count)
                    {
                        Node nextNode = (Node)PathArray[index];
                        //line.SetPosition(index-1, node.position);
                        //line.SetPosition(index, nextNode.position);
                        //Debug.DrawLine(node.position, nextNode.position, Color.green);
                        //Debug.Log(node + " : " + nextNode.ToString());
                        var row = int.Parse(node.ToString()[5].ToString());
                        var col = int.Parse(node.ToString()[7].ToString());

                        var nextRow = int.Parse(nextNode.ToString()[5].ToString());
                        var nextCol = int.Parse(nextNode.ToString()[7].ToString());
                        
                        if (row < nextRow)
                            node.Direction2.SetActive(true);
                        else if(row > nextRow)
                            node.Direction4.SetActive(true);
                        else if (col > nextCol)
                            node.Direction3.SetActive(true);
                        else if(col < nextCol)
                            node.Direction1.SetActive(true);

                        

                        index++;
                    }
                }
            }
        }

        private void RedesignPath()
        {
            if (StartNode == null || GoalNode == null || nodemanager.GetNearestNode() == null) return;
            if (!PathArray.Contains(nodemanager.GetNearestNode()))
            {
                _pathRedesigned = true;
                FindPath();
            }
        }
    }
}
