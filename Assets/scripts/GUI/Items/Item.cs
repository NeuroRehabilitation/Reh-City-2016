using UnityEngine;
using System.Collections;

/* Base class for all items 
 * 
*/

public class Item{
	
	
	protected GUIStyle itemstyle;
	
	public GUIStyle ItemStyle
	{
		get
		{
			return itemstyle;
		}
	}
	
	public void SetDefaultItemStyle()
	{
		itemstyle.alignment = TextAnchor.LowerRight;
		itemstyle.fontSize = 20;
		itemstyle.fontStyle = FontStyle.Bold;
		itemstyle.stretchWidth = false;
		itemstyle.stretchHeight = false;
			
	}
	
	
}
