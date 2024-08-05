using Assets.scripts.GUI;
using UnityEngine;

namespace Assets.scripts.RehabCar.Waypoints
{
    public class CarOnWaypoints : MonoBehaviour {

        private GameObject waypointmanager;
        private global::Assets.scripts.RehabCar.RehabCar car;
        private WayPoint currentwaypoint;
        //private WayPoint previouswaypoint;
        /// <summary>
        /// -ve and +ve angles
        /// </summary>
        private float origtheta=0;
        /// <summary>
        /// angle from 0-360
        /// </summary>
        private float correctedtheta = 0;

        private float leftboundary = 0;
        private float rightboundary = 0;
        private float distance;
        /// <summary>
        /// used to check if the car has reached halfway
        /// </summary>
        private float totaldistance;
        /// <summary>
        /// directional vector to get the angle
        /// </summary>
        private Vector3 desiredrotation;
        private float turndirection;
        private float turndirectionatboundary;

        public bool reached = false;
        private float turnangle;
        private Transform RayTransform;
        private Vector3 RightRayPoint;
        private Vector3 RightDistancefromRayPoint;
        private Vector3 LeftRayPoint;
        private Vector3 LeftDistancefromRayPoint;
        private Vector3 CenterPoint;
        private Vector3 DistancefromRayPoint;
        private RaycastHit hit;
        private AudioClip Horn;

        private Ray CenterRay;
        private Ray Leftray;
        private Ray RightRay; 

        private bool CollidingPlayer = false;
        private bool CollidingWall = false;

        public enum path
        {
            First,
            Second,
            Third,
            Fourth,
            Fifth
        };
        public path Path;
        // Use this for initialization
        void Start () {
            waypointmanager = GameObject.Find("CarWayPointManager");
            car = GetComponent<global::Assets.scripts.RehabCar.RehabCar>();
            RayTransform = transform.FindChild("RayObj");
            GetCurrentWaypoint();
            CalculateTurnAngle();
            LeftRayPoint = new Vector3(RayTransform.position.x + 1, RayTransform.position.y, RayTransform.position.z + 1);
            LeftDistancefromRayPoint = LeftRayPoint - RayTransform.position;
            RightRayPoint = new Vector3(RayTransform.position.x - 1, RayTransform.position.y, RayTransform.position.z + 1);
            RightDistancefromRayPoint = RightRayPoint - RayTransform.position;
            CenterPoint = new Vector3(RayTransform.position.x, RayTransform.position.y, RayTransform.position.z + 1);
            DistancefromRayPoint = CenterPoint - RayTransform.position;
            Horn = Resources.Load("Sounds/Effects/CarHorn") as AudioClip;
        }

        void GetCurrentWaypoint()
        {
            switch (Path)
            {
                case path.First:
                    currentwaypoint = waypointmanager.transform.FindChild("CarPath_01").GetComponent<WayPointManager>().staringwaypoint;
                    break;
                case path.Second:
                    currentwaypoint = waypointmanager.transform.FindChild("CarPath_02").GetComponent<WayPointManager>().staringwaypoint;
                    break;
                case path.Third:
                    currentwaypoint = waypointmanager.transform.FindChild("CarPath_03").GetComponent<WayPointManager>().staringwaypoint;
                    break;
                case path.Fourth:
                    currentwaypoint = waypointmanager.transform.FindChild("CarPath_04").GetComponent<WayPointManager>().staringwaypoint;
                    break;
                case path.Fifth:
                    currentwaypoint = waypointmanager.transform.FindChild("CarPath_05").GetComponent<WayPointManager>().staringwaypoint;
                    break;
                default:
                    break;
            }
        }

        // here we are calculating the turn angle that the car has to perform
        // this is called only once everytime car reaches a waypoint or if it is at start.
        void CalculateTurnAngle()
        {
            desiredrotation = transform.position - currentwaypoint.pos;
            origtheta = (Mathf.Atan2(desiredrotation.z, desiredrotation.x) * Mathf.Rad2Deg) + 90;
            // if origtheta gives negative angle then multiply with -1 to make it +ve else subtract from 360
            correctedtheta = origtheta > 0 ? 360 - origtheta : -1 *origtheta;
            turndirection = GetTurnDirectionAtWaypoint();
            //  Debug.Log(" turn direction " + turndirection);
            leftboundary = correctedtheta > 1 ? correctedtheta - 1 : 360 - correctedtheta;
            rightboundary = correctedtheta < 360 ? correctedtheta + 1 : Mathf.Abs(correctedtheta - (correctedtheta + 1));
            // Debug.Log("angle b/w " + currentwaypoint + " is " + correctedtheta + " original theta is " + origtheta * -1 + " turn direction " + turndirection);
            // Debug.Log("boundaries are " + leftboundary + " and " + rightboundary);
        }
        float GetTurnDirectionAtWaypoint()
        {
            #region 0 - 180 degrees
            if ((transform.localEulerAngles.y > 0 && transform.localEulerAngles.y < 180) && (correctedtheta > 0 && correctedtheta < 180))
            {
                // Debug.Log("In 0- 180 degrees quadrant");
                if (correctedtheta > transform.localEulerAngles.y)
                {
                    //turn right;
                    return 1;
                }
                else if (correctedtheta < transform.localEulerAngles.y)
                {
                    //turn left;
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            #endregion

            #region 90-270 degrees
            if ((transform.localEulerAngles.y > 90 && transform.localEulerAngles.y < 270) && (correctedtheta > 90 && correctedtheta < 270))
            {
                //   Debug.Log("In 90-270 degrees quadrant");
                if (correctedtheta > transform.localEulerAngles.y)
                {
                    //turn right;
                    return 1;
                }
                else if (correctedtheta < transform.localEulerAngles.y)
                {
                    //turn left;
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            #endregion

            #region 180-360 degrees
            if ((transform.localEulerAngles.y > 180 && transform.localEulerAngles.y < 360) && (correctedtheta > 180 && correctedtheta < 360))
            {
                //   Debug.Log("In 180-360 degrees quadrant");
                if (correctedtheta > transform.localEulerAngles.y)
                {
                    //turn right;
                    return 1;
                }
                else if (correctedtheta < transform.localEulerAngles.y)
                {
                    //turn left;
                    return -1;
                }
                else
                {
                    return 0;
                }
            }

            #endregion

            #region 270-90 degrees
            if ((transform.localEulerAngles.y > 270 && transform.localEulerAngles.y < 360) && (correctedtheta > 270 && correctedtheta < 360) ||
                (transform.localEulerAngles.y > 0 && transform.localEulerAngles.y < 90) && (correctedtheta > 0 && correctedtheta < 90))
            {
                //    Debug.Log("In complicated quadrant 270-90");
                if (correctedtheta > transform.localEulerAngles.y)
                {
                    //turn right;
                    return 1;
                }
                else if (correctedtheta < transform.localEulerAngles.y)
                {
                    //turn left;
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            else if ((transform.localEulerAngles.y > 270 && transform.localEulerAngles.y < 360) && (correctedtheta > 0 && correctedtheta < 90))
            {
                //     Debug.Log("In complicated quadrant 270-90");
                //turn right
                return 1;
            }
            else if ((transform.localEulerAngles.y > 0 && transform.localEulerAngles.y < 90) && (correctedtheta > 270 && correctedtheta < 360))
            {
                //    Debug.Log("In complicated quadrant 270-90");
                //turn left
                return -1;
            }
            #endregion
            return 0;
        }
        void CheckDistanceFromWaypoints()
        {
            //Debug.Log(distance);
            // distance is calculated in update so we are checking if it is zero then ignore.
            if (distance == 0 || CollidingPlayer || CollidingWall) return;
            // start moving if the distance b/w car and next waypoint is large
            if (distance > 10)
            {
                GiveAcceleration();
            }
            // if the car is near a waypoint
            else
            {
                StopAcceleration();
            }
        }
        void GiveAcceleration()
        {
            // dont turn if the car is within boundaries
            if (transform.eulerAngles.y > leftboundary && transform.eulerAngles.y < rightboundary)
            {
                car.ApplyThrottle(0.8f);
                car.ApplySteering(0);
            }
            // turn the car 
            else
            {
                // keep on calculating the angle and adjusting boundaries
                CheckTurnAngleAlongTheWay();
                if (!ReachedDesiredAngle())
                {
                    car.ApplySteering((turndirection) * turnangle);
                }
                car.ApplyThrottle(0.9f);
            }
        }
        bool ReachedDesiredAngle()
        {
            float carrotation = transform.localEulerAngles.y;
            float amounttorotate = correctedtheta - carrotation;
            // turn angle is more when car reaches waypoints
            turnangle = (1 * Mathf.Abs(amounttorotate)) / 20;
            turnangle = Mathf.Clamp(turnangle, 0, 1);
            if (!reached)
            {
                if (Mathf.Round(carrotation) == Mathf.Round(correctedtheta)) return true;
                // turn angle is less when car is moving towards a waypoint
                turnangle = (0.7f * Mathf.Abs(amounttorotate)) / 20;
                turnangle = Mathf.Clamp(turnangle, 0, 0.7f);
                // round the boundaries to nearest int to reduce gittering i.e avoiding continuous left/right wheel movement 
                leftboundary = Mathf.Round(leftboundary);
                rightboundary = Mathf.Round(rightboundary);
                carrotation = Mathf.Round(carrotation);
                if ((leftboundary < 90 && rightboundary < 90) && (carrotation > 270))
                {
                    //turn right
                    turndirection = 1;
                    return false;
                }
                else if ((leftboundary > 270 && rightboundary > 270) && (carrotation < 90))
                {
                    //turn left
                    turndirection = -1;
                    return false;
                }
                /*  else if ((leftboundary > 270 && rightboundary < 90) && (carrotation > 90))
            {
                //turn left
                turndirection = -1;
                return false;
            }
            else if ((leftboundary < 270 && rightboundary > 90) && (carrotation > 270 || carrotation < 90))
            {
                //turn left
                turndirection = -1;
                return false;
            }*/
                else if (carrotation < leftboundary)
                {
                    turndirection = 1;
                    return false;
                }
                else if (carrotation > rightboundary)
                {
                    turndirection = -1;
                    return false;
                }
                else
                {
                    turndirection = 0;
                    return false;
                }
            }
            //Debug.Log(turnangle +" and turn direction "+turndirection);
      
            return false;
        }
        void StopAcceleration()
        {
            // give less acceleration to prevent from total stopping
            car.ApplyThrottle(0.2f);
            // if speed is too high apply breaks
            if (car.currentspeed > car.topspeed / 3) car.ApplyBrakes(1f);
            // if car has reached near a waypoint
            if (distance < 7 && distance >3)
            {
                // here car realises it has reached waypoint
                // and stops checking boundaries (ReachedDesiredAngle())
                reached = true;
            }else if(distance<3){
            
                // Get the next waypoint and calculate the turn angle
                //previouswaypoint = currentwaypoint;
                currentwaypoint = currentwaypoint.Next;
                CalculateTurnAngle();
            }
        }
        void CheckTurnAngleAlongTheWay()
        {
            reached = false;
            desiredrotation = transform.position - currentwaypoint.pos;
            origtheta = (Mathf.Atan2(desiredrotation.z, desiredrotation.x) * Mathf.Rad2Deg) + 90;
            correctedtheta = origtheta > 0 ? 360 - origtheta : -1 * origtheta;
            leftboundary = correctedtheta>0?correctedtheta - 1:360 - correctedtheta;
            rightboundary = correctedtheta < 360 ? correctedtheta + 1 :Mathf.Abs(correctedtheta - (correctedtheta+1));
            //Debug.Log("left boundary at half way " + leftboundary + " right boundary " + rightboundary);
        }
        void RayCast()
        {
            float RnewX = RightDistancefromRayPoint.x * Mathf.Cos(transform.localEulerAngles.y * Mathf.Deg2Rad) - RightDistancefromRayPoint.z * Mathf.Sin(transform.localEulerAngles.y * Mathf.Deg2Rad);
            float RnewZ = RightDistancefromRayPoint.x * Mathf.Sin(transform.localEulerAngles.y * Mathf.Deg2Rad) + RightDistancefromRayPoint.z * Mathf.Cos(transform.localEulerAngles.y * Mathf.Deg2Rad);

            float LnewX = LeftDistancefromRayPoint.x * Mathf.Cos(transform.localEulerAngles.y * Mathf.Deg2Rad) - LeftDistancefromRayPoint.z * Mathf.Sin(transform.localEulerAngles.y * Mathf.Deg2Rad);
            float LnewZ = LeftDistancefromRayPoint.x * Mathf.Sin(transform.localEulerAngles.y * Mathf.Deg2Rad) + LeftDistancefromRayPoint.z * Mathf.Cos(transform.localEulerAngles.y * Mathf.Deg2Rad);

            float CnewX = DistancefromRayPoint.x * Mathf.Cos(transform.localEulerAngles.y * Mathf.Deg2Rad) - DistancefromRayPoint.z * Mathf.Sin(transform.localEulerAngles.y * Mathf.Deg2Rad);
            float CnewZ = DistancefromRayPoint.x * Mathf.Sin(transform.localEulerAngles.y * Mathf.Deg2Rad) + DistancefromRayPoint.z * Mathf.Cos(transform.localEulerAngles.y * Mathf.Deg2Rad);
            RightRayPoint.x = RayTransform.position.x - RnewX;
            RightRayPoint.z = RayTransform.position.z + RnewZ;

            LeftRayPoint.x = RayTransform.position.x - LnewX;
            LeftRayPoint.z = RayTransform.position.z + LnewZ;

            CenterPoint.x = RayTransform.position.x - CnewX;
            CenterPoint.z = RayTransform.position.z + CnewZ;

            CenterRay = new Ray(CenterPoint, transform.forward);
            Leftray = new Ray(LeftRayPoint, transform.forward);
            RightRay = new Ray(RightRayPoint, transform.forward);

            Debug.DrawRay(Leftray.origin, Leftray.direction, Color.green);
            Debug.DrawRay(RightRay.origin, RightRay.direction, Color.green);
            Debug.DrawRay(CenterRay.origin, CenterRay.direction, Color.green);
        }
        bool CollisionDetectionWithPlayer()
        {
            if (Physics.Raycast(Leftray, out hit, 50) || Physics.Raycast(RightRay, out hit, 50)||Physics.Raycast(CenterRay,out hit,50))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    if (!GetComponent<AudioSource>().isPlaying)
                    {
                        GetComponent<AudioSource>().clip = Horn;
                        if(!CalibrationGUI.CalibProcess)
                            GetComponent<AudioSource>().Play();
                    }
                    return true;
                }
            }
            return false;
        }
        bool CollisionDetectionWithWall()
        {
            if (Physics.Raycast(Leftray, out hit, 0.5f) || Physics.Raycast(RightRay, out hit, 0.5f) || Physics.Raycast(CenterRay, out hit, 0.5f))
            {
                if (hit.collider.gameObject.tag == "City")
                {
                    return true;
                }
            }
            return false;
        }
        void Update()
        {
            distance = (transform.position - currentwaypoint.pos).magnitude;
            RayCast();
            CollidingPlayer = CollisionDetectionWithPlayer();
            CollidingWall = CollisionDetectionWithWall();
            if (CollidingPlayer)
            {
                car.ApplyThrottle(0);
                car.ApplyHandBrakes(0.8f);
                //Debug.Log("car is colliding with player");
            }
            else
            {
                car.ReleaseBrakes();
            }
        }
        void FixedUpdate () {
            CheckDistanceFromWaypoints();
        }
    }
}
