using System.Collections;
using System.Collections.Generic;
using Assets.scripts.GUI;
using Assets.scripts.Manager;
using UnityEngine;

/* Attach this class to Nodemanager class in City scene
 * It sets up the Start and goal node.
 * This class is used by AStar class to getneighbours of a node.
 * 
*/

namespace Assets.scripts.MiniMapScripts.AStar
{
    public class NodeManager : MonoBehaviour {

        public int numOfRows;
        public int numOfCols;
        public Node[,] nodes;
        public GameObject[] child;
        public GameObject[] ObstacleList;
        private static List<Node> StartNodeList;
        private static List<Node> GoalNodeList;
        public static Node StartNode;
        public static List<string> PlayerPath = new List<string>();

        private static NodeManager s_Instance = null;
        public static NodeManager Instance{
            get{
                if(s_Instance == null)
                {
                    s_Instance = FindObjectOfType(typeof(NodeManager)) as NodeManager;
                    if(s_Instance==null){
                        Debug.Log("Could not locate GridManager");
                    }
                }
                return s_Instance;
            }
        }

        void Awake ()
        {
            nodes = new Node[numOfRows,numOfCols];
            child = GameObject.FindGameObjectsWithTag("Node");
            ObstacleList = GameObject.FindGameObjectsWithTag("Obstacle");
            StartNodeList = new List<Node>();
            GoalNodeList = new List<Node>();
            SetUpNodes();
        }

        private void Update()
        {
            var nearestNode = GetNearestNode();
            
            if (nearestNode != null)
            {
                if (PlayerPath.Count == 0)
                {
                    PlayerPath.Add(nearestNode.name);
                }
                else if (PlayerPath[PlayerPath.Count - 1] != nearestNode.name)
                {
                    PlayerPath.Add(nearestNode.name);
                }
            }
            
        }

        void SetUpNodes()
        {
            foreach (GameObject g in child)
            {
                g.name = "Node " + GetRow(g.GetComponent<Node>()) + " " + GetCol(g.GetComponent<Node>());
                nodes[GetRow(g.GetComponent<Node>()), GetCol(g.GetComponent<Node>())] = g.GetComponent<Node>();
                StartNodeList.Add(g.GetComponent<Node>());
                GoalNodeList.Add(g.GetComponent<Node>());
            }    		
        }
   
        public Node GetStartNode()
        {
            foreach(Node node in StartNodeList)
            {
                node.GetDistanceFromPlayer();
            }
            InsertionSort(StartNodeList);

            //Quaternion rotation = Quaternion.LookRotation(StartNodeList[0].GetRelativePosition());
            //Debug.Log(rotation);
            //GameManager.PlayerData.Playerrotation = Quaternion.Inverse(rotation) * GameManager.PlayerData.Playerrotation;
            
            //Debug.Log("Node set");
            return StartNodeList[0];
        }
        public Node GetGoalNode()
        {
            foreach (Node node in GoalNodeList)
            {
                node.GetDistanceFromGoal();
            }
            InsertionSortForGoal(GoalNodeList);
            
            return GoalNodeList[0];
        }

        public Node CalculateStartNode(Vector3 location)
        {
            foreach (Node node in StartNodeList)
            {
                node.CalculateDistanceStartingPos(location);
                
            }
            InsertionSort(StartNodeList);
            
            return StartNodeList[0];
        }

        public Node CalculateGoalNode(Vector3 location)
        {
            foreach (Node node in GoalNodeList)
            {

                //Debug.Log(node.name + " ; " + node.position);
                node.CalculateDistanceFromGoal(location);
            }
            InsertionSortForGoal(GoalNodeList);

            return GoalNodeList[0];
        }

        public Node GetNearestNode()
        {
            for (int i = 0; i < StartNodeList.Count; i++)
            {
                Node node = StartNodeList[i];
                node.GetDistanceFromPlayer();
                if (node.DistanceFromPlayer < 20) return node;
            }
            return null;
        }

        public void GetNeighbours(Node node, ArrayList neighbours)
        {
            int thisrow = GetRow(node);
            int thiscol = GetCol(node);
            int thisindex = node.index;
            //Bottom
            int assignRow = thisrow-1;
            int assignCol = thiscol;
            AssignNeighbour(thisindex ,thisrow,thiscol,assignRow,assignCol,neighbours);
            //Top
            assignRow = thisrow+1;
            assignCol = thiscol;
            AssignNeighbour(thisindex ,thisrow,thiscol,assignRow,assignCol,neighbours);	
            //Left
            assignRow = thisrow;
            assignCol = thiscol-1;
            AssignNeighbour(thisindex ,thisrow,thiscol,assignRow,assignCol,neighbours);
            //Right
            assignRow = thisrow;
            assignCol = thiscol+1;
            AssignNeighbour(thisindex ,thisrow,thiscol,assignRow,assignCol,neighbours);
		
        }
	
        public void AssignNeighbour(int nodeindex,int noderow,int nodecol, int row,int col,ArrayList neighbours)
        {
            
            if(row!=-1 && col!=-1 && row<numOfRows && col < numOfCols)
            {
                if(nodes[row,col]!=null)
                {
                    Node nodetoAdd = nodes[row,col];
                    if(!ObstacleInPath(nodeindex,noderow,nodecol,row,col))
                    {
                        //Debug.Log("nodeindex: " + nodeindex + " ; noderow: " + noderow + " ; nodecol: " + nodecol + " ; row: " + row + " ; col: " + col + " ; neighboursList: " + neighbours.Count);
                        neighbours.Add(nodetoAdd);
                    }		
                }
            }	
        }
	
        bool ObstacleInPath(int nodeindex, int noderow, int nodecol , int nextrow, int nextcol)
        {
            foreach(GameObject g in ObstacleList)
            {
                // if the node next to the obstacle 
                if(g.GetComponent<Obstacle>().ObstacleFor == nodeindex)
                {
                    if((nextrow == g.GetComponent<Obstacle>().CannotConnecttoRow)&&(nextcol==g.GetComponent<Obstacle>().CannotConnecttoCol))return true;
                }
            }
            return false;
        }
        public int GetRow(Node node)
        {
            return node.index / numOfCols;
        }

        public int GetRow(int index)
        {
            return index / numOfCols;
        }

        public int GetCol(Node node)
        {
            return node.index % numOfRows;
        }

        public int GetCol(int index)
        {
            return index % numOfRows;
        }

        void InsertionSort(List<Node> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i; j > 0 && less(list[j].DistanceFromPlayer, list[j - 1].DistanceFromPlayer); j--)
                {
                    exchange(list, j, j - 1);
                }
            }
        }
        void InsertionSortForGoal(List<Node> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i; j > 0 && less(list[j].DistanceFromGoal, list[j - 1].DistanceFromGoal); j--)
                {
                    exchange(list, j, j - 1);
                }
            }
        }
        bool less(float a, float b)
        {
            if (a < b) return true;
            return false;
        }
        void exchange(List<Node> list, int a, int b)
        {
            Node temp = list[b];
            list[b] = list[a];
            list[a] = temp;
            temp = null;
        }
	
    }
}
