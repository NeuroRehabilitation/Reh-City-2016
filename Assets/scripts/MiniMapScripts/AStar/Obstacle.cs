using UnityEngine;

namespace Assets.scripts.MiniMapScripts.AStar
{
    public class Obstacle : MonoBehaviour {
	
        public int ObstacleFor;
        public int ObstacleIndex;

        public int CannotConnecttoRow;
        public int CannotConnecttoCol;
	
        private void Start () {
	
            CannotConnecttoRow = NodeManager.Instance.GetRow(ObstacleIndex);
            CannotConnecttoCol = NodeManager.Instance.GetCol(ObstacleIndex);
        }
	
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position,4.0f);
        }
    }
}
