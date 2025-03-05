using System.Collections.Generic;
using System.Linq;
using Assets.scripts.objectives;
using UnityEngine;

namespace Assets.scripts.collectibles
{
    public class ItemShuffler : MonoBehaviour {

        public List<GameObject> items = new List<GameObject>();
        private List<Vector3> positions = new List<Vector3>(); 
        private GameObject _mger;
        private ObjectiveManager _objManager;
        private bool _shuffled;

        private void Start()
        {
            _mger = GameObject.FindGameObjectWithTag("Manager");
            _objManager = _mger.GetComponent<ObjectiveManager>();
        }

        private void Update()
        {
            
            if (_objManager.GetCurrentObjective != null && _objManager.GetCurrentObjective.FirstItemName != "NaN" && !_shuffled)
            {
                StartShuffleOperation();
            }
        }

        private void RandomizeArray(GameObject[] arr)
        {
            for (var i = arr.Length - 1; i > 0; i--)
            {
                var r = Random.Range(0, i);
                var tmp = arr[i];
                arr[i] = arr[r];
                arr[r] = tmp;
            }
        }

        private void StartShuffleOperation()
        {
            var tempitems = GameObject.FindGameObjectsWithTag("items").OrderBy(g => g.transform.GetSiblingIndex()).ToArray();


            for (var i = 0; i < tempitems.Length; i++)
            {
                positions.Add(tempitems[i].transform.position);
            }
            RandomizeArray(tempitems);
            var tempFirstItemQuantity = 0;
            var tempSecondItemQuantity = 0;
            var tempThirdItemQuantity = 0;
            var tempFourthItemQuantity = 0;

            for (var i = 0; i < tempitems.Length; i++)
            {
                var sum = tempFirstItemQuantity + tempSecondItemQuantity + tempThirdItemQuantity + tempFourthItemQuantity + _objManager.GetCurrentObjective.Distractors;

                if (tempitems[i].GetComponentInChildren<CollectibleItem>().ItemName.ToString() == _objManager.GetCurrentObjective.FirstItemName && tempFirstItemQuantity < _objManager.GetCurrentObjective.NumberofFirstItem)
                {
                    tempFirstItemQuantity++;
                    items.Add(tempitems[i]);
                }
                else if (tempitems[i].GetComponentInChildren<CollectibleItem>().ItemName.ToString() == _objManager.GetCurrentObjective.SecondItemName && tempSecondItemQuantity < _objManager.GetCurrentObjective.NumberofSecondItem)
                {
                    tempSecondItemQuantity++;
                    items.Add(tempitems[i]);
                }
                else if (tempitems[i].GetComponentInChildren<CollectibleItem>().ItemName.ToString() == _objManager.GetCurrentObjective.ThirdItemName && tempThirdItemQuantity < _objManager.GetCurrentObjective.NumberofThirdItem)
                {
                    tempThirdItemQuantity++;
                    items.Add(tempitems[i]);
                }
                else if (tempitems[i].GetComponentInChildren<CollectibleItem>().ItemName.ToString() == _objManager.GetCurrentObjective.FourthItemName && tempFourthItemQuantity < _objManager.GetCurrentObjective.NumberofFourthItem)
                {
                    tempFourthItemQuantity++;
                    items.Add(tempitems[i]);
                }
                else if (items.Count < sum &&
                    tempitems[i].GetComponentInChildren<CollectibleItem>().ItemName.ToString() != _objManager.GetCurrentObjective.FirstItemName &&
                    tempitems[i].GetComponentInChildren<CollectibleItem>().ItemName.ToString() != _objManager.GetCurrentObjective.SecondItemName &&
                    tempitems[i].GetComponentInChildren<CollectibleItem>().ItemName.ToString() != _objManager.GetCurrentObjective.ThirdItemName &&
                    tempitems[i].GetComponentInChildren<CollectibleItem>().ItemName.ToString() != _objManager.GetCurrentObjective.FourthItemName)
                {
                    items.Add(tempitems[i]);
                }
                else
                {
                    Destroy(tempitems[i]);
                }
            }
            for (var i = 0; i < items.Count; i++)
            {
                items[i].transform.position = new Vector3(positions[i].x, positions[i].y, positions[i].z);;
            }
            KnuthShuffle();
            _shuffled = true;
        }

        

        /// <summary>
        /// Each Item has a random number generated.
        /// we are sorting the array using insertion sort.
        /// sorting is done for positions.
        /// </summary>
        private int ShuffleCounter = 0;
        void Shuffle()
        {
            for (int i = 0; i < items.Count; i++)
            {
                for (int j = i; j > 0 && less(items[j].GetComponentInChildren<CollectibleItem>().randomnumber, items[j - 1].GetComponentInChildren<CollectibleItem>().randomnumber); j--)
                {
                    exchange(items, j, j - 1);
                    ShuffleCounter++;
                    // Debug.Log("Shuffled" +j.ToString()+ " and " + (j-1).ToString()+" for "+ShuffleCounter.ToString()+" time ");
                }
                //Debug.Log(i);
            }
            Debug.Log("Shuffled items and it took " + ShuffleCounter + " Cycles");
        }
        /// <summary>
        /// efficient shuffle algorithm
        /// </summary>
        void KnuthShuffle()
        {
            for (int i = 1; i < items.Count; i++)
            {
                int randomnumber = Random.Range(0, i);
                exchange(items, i, randomnumber);
                ShuffleCounter++;
            }
            //Debug.Log("Shuffled items and it took " + ShuffleCounter + " Cycles" );
        }
        bool less(int a, int b)
        {
            if (a <= b) return true;
            return false;
        }

        /// <summary>
        /// exchange both positions and 
        /// also the array items.
        /// </summary>
        void exchange(List <GameObject> itemarray, int a, int b)
        {
            if (a == b) return;
            Vector3 temp = itemarray[b].transform.position;
            GameObject tempg = itemarray[b];
            itemarray[b].transform.position = itemarray[a].transform.position;
            itemarray[b] = itemarray[a];
            itemarray[a].transform.position = temp;
            itemarray[a] = tempg;
        }
    }
}
