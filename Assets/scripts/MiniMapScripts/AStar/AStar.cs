using System.Collections;
using UnityEngine;

/*
 * AStar pathfinding algorithm
*/
namespace Assets.scripts.MiniMapScripts.AStar
{
    public class AStar
    {
        public static PriorityQueue ClosedList, OpenList;
        private static ArrayList _list;
        private static float HeuristicEstimateCost(Node curNode,Node goalNode)
        {
            
            var vecCost = curNode.position - goalNode.position;
            //Debug.Log("HeuristicEstimateCost for current node " + curNode.name + " at " + curNode.position + " and goal node" + goalNode.name + " at " + goalNode.position + vecCost.magnitude);
            return vecCost.magnitude;
        }
        public static  ArrayList FindPath(Node start, Node goal)
        {
            OpenList = new PriorityQueue();
            OpenList.Clear();
            OpenList.Push(start);
            start.nodeTotalCost = 0;
            start.estimatedCost = HeuristicEstimateCost(start,goal);

            ClosedList = new PriorityQueue();
            ClosedList.Clear();
            Node node = null;
		
            while(OpenList.Length!=0)
            {
                node = OpenList.First();
                //Debug.Log("calculating for node: " + node.name);
                if(node.position == goal.position)
                {
                    //Debug.Log("Path found");
                    return CalculatePath(node);
                }
                var neighbours = new ArrayList();
                neighbours.Clear();
                NodeManager.Instance.GetNeighbours(node,neighbours);

                var index = 0;

                foreach (var t in neighbours)
                {
                    
                    var neighbourNode =(Node)t;

                    //Debug.Log("FindPath start: " + start.name + " ; goal: " + goal.name + " ; node being calculated: " + node.name + " ; neighbour" + index + ": " + neighbourNode.name);
                    //Debug.Log(neighbours.Count + "neighbours found for node " + node.name + " . At " + index + " index neighbour (" + neighbourNode.name + "),There are " + ClosedList.Length + " in ClosedList and " + OpenList.Length + " in OpenList");
                    if (!ClosedList.Contains(neighbourNode))
                    {
                        //Debug.Log("closed list doesn't contain neighbourNode: " + neighbourNode.name);
                        var cost = HeuristicEstimateCost(node,neighbourNode);
                        var totalcost = node.nodeTotalCost + cost;
                        var neighbournodeEstCost = HeuristicEstimateCost(neighbourNode,goal);
					
                        neighbourNode.nodeTotalCost = totalcost;
                        neighbourNode.parent = null;
                        neighbourNode.parent = node;
                        neighbourNode.estimatedCost = totalcost+neighbournodeEstCost;
                        //Debug.Log("neighbourNode.estimatedCost: " + neighbourNode.estimatedCost);
					
                        if(!OpenList.Contains(neighbourNode))
                        {
                            //Debug.Log("Pushing neighbourNode to open list: " + neighbourNode.name);
                            OpenList.Push(neighbourNode);
                        }
                    }

                    index++;
                }
                ClosedList.Push(node);
                
                OpenList.Remove(node);
                
             

                //Debug.Log("pushing node " + node.name + " to closedList and removing it from OpenList");



            }

            if (node != null && node.position != goal.position)
            {
                //Debug.Log("goal Not Found");
                return null;
            }
            return CalculatePath(node);
        }
   
        private static ArrayList CalculatePath(Node node)
        {
            if (_list == null) _list = new ArrayList();
            else _list.Clear();
       
            while (node != null)
            {
                _list.Add(node);
                node = node.parent;
            }
            _list.Reverse();
            return _list;
        }
        public static  void ClearPath()
        {
            if (OpenList != null)
            {
                foreach (Node n in OpenList.nodes)
                {
                    n.parent = null;
                }
            }

            if (ClosedList != null)
            {
                foreach (Node n in ClosedList.nodes)
                {
                    n.parent = null;
                }
            }

            if (_list != null)
            {
                foreach (Node n in _list)
                {
                    n.parent = null;
                }
                _list.Clear();
            }  
        }
    }
}

