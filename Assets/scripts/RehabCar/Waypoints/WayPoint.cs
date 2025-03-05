using UnityEngine;

namespace Assets.scripts.RehabCar.Waypoints
{
    public class WayPoint : MonoBehaviour {

        public bool isStart = false;
        public WayPoint Next;
    
        [HideInInspector]
        public Vector3 pos;
        public enum path
        {
            First,
            Second,
            Third,
            Fourth,
            Fifth
        };
        public path Path;

        private void Awake()
        {
            pos = transform.position;
        }
        void OnDrawGizmos()
        {
            switch(Path)
            {
                case path.First:
                    Gizmos.color = Color.green;
                    break;
                case path.Second:
                    Gizmos.color = Color.cyan;
                    break;
                case path.Third:
                    Gizmos.color = Color.magenta;
                    break;
                case path.Fourth:
                    Gizmos.color = Color.white;
                    break;
                case path.Fifth:
                    Gizmos.color = Color.blue;
                    break;
                default:
                    Gizmos.color = Color.green;
                    break;
            }
            Gizmos.DrawCube(transform.position, new Vector3(2, 2, 2));
        }
    }
}
