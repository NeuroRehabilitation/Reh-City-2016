using UnityEngine;
using System.Collections;
using Assets.scripts.GUI;

public class InventoryGUI : MonoBehaviour {
	
	//private Vector2 ScrollBarGrid = Vector2.zero;
	private bool InventoryOn = true;
	private int GridValue = -1;
	
	// GUI pos and size
	public Vector2 GridPosition = new Vector2(15,45);
	public Vector2 GridSize = new Vector2(330,300);
	
	public Vector2 ScrollBarPosition = new Vector2(0,95);
	public Vector2 ScrollBarSize = new Vector2(353,257);
	
	public Vector2 ClosePosition = new Vector2(270,0);
	public Vector2 CloseSize = new Vector2(35,35);
	
	public Vector2 WindowPosition = new Vector2(0,0);
	public Vector2 WindowSize = new Vector2(360,360);
	
	// Style
	public GUIStyle buttonStyle;
	
	// Textures
	public Texture InventoryWindow;
	public Texture CloseIcon;
	public Texture[] Grids;
	
	public Transform milk;
	public InventoryGUI(){}
	
	// Use this for initialization
	void Start () {
		//Grids[0] = Resources.Load("Images/close") as Texture;
		
		/*for (var y = 0; y < 5; y++) {
        	for (var x = 0; x < 5; x++) {
            	var cube = GameObject.CreatePrimitive(PrimitiveType.);
            	cube.AddComponent(Rigidbody);
            	cube.transform.position = Vector3 (x, y, 0);
       		}
    	}*/
	}
	
	// Update is called once per Frame
	void Update () 
	{
		
		Inventory_add_item AddingNewItem = GetComponent(typeof(Inventory_add_item)) as Inventory_add_item;
		
		if(Input.GetKeyUp("i"))
		{
			if(InventoryOn == false)
			{
				InventoryOn = true;
			}
			else if (InventoryOn == true) 
			{
				InventoryOn = false;
			}
		}
		
		if( Input.GetMouseButtonDown(0) )
    	{
       		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
       		RaycastHit hit;
 
      		if( Physics.Raycast( ray, out hit, 100 ) )
       		{
						
				if (hit.transform.gameObject.tag == "item_0")
				{
					AddingNewItem.newItem(0);
					Inventory_add_item.InventoryNewItemAdded = 0;
					Destroy(hit.transform.gameObject);
				}
					// Milk
				if (hit.transform.gameObject.tag == "item_1")
				{
					AddingNewItem.newItem(1);
					Inventory_add_item.InventoryNewItemAdded = 0;
					Destroy(hit.transform.gameObject);
				}
					// Juice1
				if (hit.transform.gameObject.tag == "item_2")
				{
					AddingNewItem.newItem(2);
					Inventory_add_item.InventoryNewItemAdded = 0;
					Destroy(hit.transform.gameObject);
				}
					// Juice2
				if (hit.transform.gameObject.tag == "item_3")
				{
					AddingNewItem.newItem(3);
					Inventory_add_item.InventoryNewItemAdded = 0;
					Destroy(hit.transform.gameObject);
				}
	        }
    	}
		
		//AddingNewItem.newItem();
	}
	
	void OnGUI()
	{
		if(InventoryOn == true)
		{
			GUI.BeginGroup(new Rect(WindowPosition.x, WindowPosition.y, WindowSize.x, WindowSize.y), InventoryWindow);
				// Close buttonif()
				if(GUI.Button(new Rect(ClosePosition.x, ClosePosition.y, CloseSize.x, CloseSize.y), CloseIcon, buttonStyle))
				{
					InventoryOn = false;
				}
			
				// Scroll Bar
				//ScrollBarGrid = GUI.BeginScrollView(new Rect(ScrollBarPosition.x, ScrollBarPosition.y, ScrollBarSize.x, ScrollBarSize.y), ScrollBarGrid, new Rect(0, 0, 0, 420));
				
					GridValue = GUI.SelectionGrid(new Rect(GridPosition.x, GridPosition.y, GridSize.x, GridSize.y), GridValue, Grids, 5, buttonStyle);
			
				//GUI.EndScrollView();
			
			GUI.EndGroup();
		}
	}
}
