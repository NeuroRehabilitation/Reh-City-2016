using System;
using UnityEngine;

namespace Assets.scripts.GUI
{
    public class Inventory_add_item : MonoBehaviour {
	
        // private
        int ArrayGrid = 0;
	
	
        InventoryGUI Theinventory;
	
        String[] itemicon = new String[]{"0_chocapic", "1_milkbox", "2_juice_1", "3_juice_2"};
	
	
        public static int InventoryNewItemAdded = 0;
	
	
        //icons 
        public Texture BlankIcon;
        public Texture TheNewItem;
	
        public Inventory_add_item(){}
	
        // Use this for initialization
        void Start ()
        {
            Theinventory = gameObject.GetComponent("InventoryGUI") as InventoryGUI;
        }
	
        // Update is called once per Frame
        public void newItem (int item)
        {
            if(Inventory_add_item.InventoryNewItemAdded > -1)
            {
                TheNewItem = Resources.Load("Images/Items/"+itemicon[item]) as Texture;
                if(ArrayGrid < Theinventory.Grids.Length)
                {
                    if(Theinventory.Grids[ArrayGrid] == BlankIcon)
                    {
					
                        Theinventory.Grids[ArrayGrid] = TheNewItem;
                        ArrayGrid += 1;
                        Inventory_add_item.InventoryNewItemAdded = -1;
                    }
                    else if(Theinventory.Grids[ArrayGrid] != BlankIcon)
                    {
                        ArrayGrid += 1;
                    }
                }
            }
        }
	
        public void teste(){}
	
        //void OnMouseDown()//OnTriggerEnter(Collider col)
        //{
        //newItem(0);
        //Inventory_add_item.InventoryNewItemAdded = 0;
        //Destroy(col.gameObject);
        //Debug.Log("colission");
        // Chocapic
        /*if (col.gameObject.tag == "item_0")
		{
			newItem(0);
			Inventory_add_item.InventoryNewItemAdded = 0;
			Destroy(col.gameObject);
		}
		// Milk
		if (col.gameObject.tag == "item_1")
		{
			newItem(1);
			Inventory_add_item.InventoryNewItemAdded = 0;
			Destroy(col.gameObject);
		}
		// Juice1
		if (col.gameObject.tag == "item_2")
		{
			newItem(2);
			Inventory_add_item.InventoryNewItemAdded = 0;
			Destroy(col.gameObject);
		}
		// Juice2
		if (col.gameObject.tag == "item_3")
		{
			newItem(3);
			Inventory_add_item.InventoryNewItemAdded = 0;
			Destroy(col.gameObject);
		}*/

        //}
    }
}
