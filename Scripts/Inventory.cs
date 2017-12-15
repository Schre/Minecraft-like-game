using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	// ITEM CODES
	int WOOD = 1, PLANK = 3, STONE = 4, IRON_ORE = 5, DIAMOND_ORE = 6, GOLD_ORE = 7, CRAFTING = 8, FURNACE = 9, GRASS = 10, DIRT = 11, SAND = 12;
	public Texture WOODt, PLANKt, STONEt, IRON_OREt, DIAMOND_OREt, GOLD_OREt, CRAFTINGt, FURNACEt, GRASSt, DIRTt, SANDt;
	public Texture Selected;
	//
	// Use this for initialization
	private Vector2 InventoryDim = new Vector2(Screen.width / 1.5f, Screen.height/1.5f);
	private Vector2 InventoryPos = new Vector2(Screen.width/6f, Screen.height/8f);
	public static Inventory currentInventory;
	private Vector2 SquareDim = new Vector2 (Screen.width/24, Screen.width/24);
	private int inventoryWidth = 6;
	private int inventoryHeight = 5;
	public int[,] items = new int[6, 5];
	public Vector2 SelectedPosition;
	private float SelectedPosLimitLeftX = 0;
	private float SelectedPosLimitRightX = 0;
	private float SelectedPosLimitUpY = 0, LeftLim = 0, RightLim = 0, UpLim = 0, DownLim = 0;
	private float SelectedPosLimitDownY = 0;
	private Vector2 quickToggle;
	private Vector2 SelectedItem = new Vector2(0f,0f);
	public bool isOpen;
	public int stopper = 0;
	void Start () {
		isOpen = false;
		GUI.enabled = false;
		 LeftLim = SelectedPosLimitLeftX = SelectedPosition.x;
		 RightLim = SelectedPosLimitRightX = SelectedPosition.x + SquareDim.x * (inventoryWidth-1);
		 UpLim = SelectedPosLimitUpY = SelectedPosition.y;
		 DownLim = SelectedPosLimitDownY = SelectedPosition.y + SquareDim.y * (inventoryHeight - 1);
		SelectedItem.x = SelectedItem.y = 0;
		quickToggle = new Vector2 (Screen.width / 2f - inventoryWidth * SquareDim.x / 2f, Screen.height / 1.1f);
	}

	// Update is called once per frame
	void Update()
	{
		
			if (Input.GetKeyDown (KeyCode.E)) {
				isOpen = !isOpen;
				Cursor.visible = !Cursor.visible;
			}
		if (isOpen) {
			if (stopper == 1) {
				SelectedPosition = new Vector2 (InventoryPos.x + 5f * SquareDim.x, InventoryPos.y + 4f * SquareDim.y);
				SelectedPosLimitLeftX = LeftLim;
				SelectedPosLimitRightX = RightLim;
				SelectedPosLimitUpY = UpLim;
				SelectedPosLimitDownY = DownLim;
				SelectedItem.x = SelectedItem.y = 0;
			}
			stopper = 0;

			if (Input.GetKeyDown (KeyCode.D))
			if (SelectedPosition.x != SelectedPosLimitRightX) {
				SelectedPosition.x += SquareDim.x;
				SelectedItem.x += 1;
			}
			if (Input.GetKeyDown (KeyCode.A))
			if (SelectedPosition.x != SelectedPosLimitLeftX) {
				SelectedPosition.x -= SquareDim.x;
				SelectedItem.x -= 1;
			}
			if (Input.GetKeyDown (KeyCode.W))
			if (SelectedPosition.y != SelectedPosLimitUpY) {
				SelectedPosition.y -= SquareDim.y;
				SelectedItem.y -= 1;
			}
			if (Input.GetKeyDown (KeyCode.S))
			if (SelectedPosition.y != SelectedPosLimitDownY) {
				SelectedPosition.y += SquareDim.y;
				SelectedItem.y += 1;
			}
			
		} else {

			if (stopper == 0) {
				SelectedItem.x = 0;
				SelectedPosition = quickToggle;
			}

			stopper = 1;
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				SelectedPosition.x = quickToggle.x;
				SelectedItem.x = 0;
			}
			if (Input.GetKeyDown (KeyCode.Alpha2)) {
				SelectedPosition.x = quickToggle.x + SquareDim.x;
				SelectedItem.x = 1;
			}
			if (Input.GetKeyDown (KeyCode.Alpha3)) {
				SelectedPosition.x = quickToggle.x + SquareDim.x * 2;
				SelectedItem.x = 2;
			}
			if (Input.GetKeyDown (KeyCode.Alpha4)) {
				SelectedPosition.x = quickToggle.x + SquareDim.x * 3;
				SelectedItem.x = 3;
			}
			if (Input.GetKeyDown (KeyCode.Alpha5)) {
				SelectedPosition.x = quickToggle.x + SquareDim.x * 4;
				SelectedItem.x = 4;
			}
			
			if (Input.GetKeyDown (KeyCode.Alpha6)) {
				SelectedPosition.x = quickToggle.x + SquareDim.x * 5;
				SelectedItem.x = 5;
			}
		}
			

	}

	void OnGUI()
	{

		if (isOpen) {
			GUI.DrawTexture (new Rect (SelectedPosition, SquareDim), Selected);
			GUI.Box (new Rect (InventoryPos.x, InventoryPos.y, InventoryDim.x, InventoryDim.y), "Inventory");
			for (int x = 0; x < inventoryWidth; x++) {
				for (int y = 0; y < inventoryHeight; y++) {
					Rect rectangle = new Rect(InventoryPos.x + x * SquareDim.x + 5f * SquareDim.x, InventoryPos.y + y * SquareDim.y + 4 * SquareDim.y, SquareDim.x, SquareDim.y);
					GUI.Box (rectangle, "");
					// DRAW ALL ITEMS
					int itemCode = items[x,y];
					Texture currentTex = null;
					switch (itemCode) 
					{
					default:
						break;
					case 1:
						currentTex = WOODt;
						break;
					case 2:
						currentTex = null;
						break;
					case 3:
						currentTex = PLANKt;
						break;
					case 4:
						currentTex = STONEt;
						break;
					case 5:
						currentTex = IRON_OREt;
						break;
					case 6:
						currentTex = DIAMOND_OREt;
						break;
					case 7:
						currentTex = GOLD_OREt;
						break;
					case 8:
						currentTex = CRAFTINGt;
						break;
					case 9:
						currentTex = FURNACEt;
						break;
					case 10:
						currentTex = GRASSt;
						break;
					case 11:
						currentTex = DIRTt;
						break;
					case 12:
						currentTex = SANDt;
						break;
					}
					if (currentTex != null) {
						GUI.DrawTexture (new Rect (rectangle.position.x + SquareDim.x / 16f, rectangle.position.y + SquareDim.y / 16f, rectangle.size.x - SquareDim.x / 8f, rectangle.size.y - SquareDim.y / 8f), currentTex);
					} 

				}
			}

		} else
			GUI.enabled = false;
		if (!isOpen) {
			
			GUI.DrawTexture (new Rect (SelectedPosition, SquareDim), Selected);
		
			GUI.Box (new Rect (Screen.width/2 - inventoryWidth*SquareDim.x/2, Screen.height/1.1f, inventoryWidth*SquareDim.x, SquareDim.y), "");
			for (int i = 0; i < inventoryWidth; i++) {
				GUI.Box (new Rect (Screen.width/2 - inventoryWidth*SquareDim.x/2f + i*SquareDim.x, Screen.height/1.1f, SquareDim.x, SquareDim.y), "");
				if (getItemTexture(items[i,0]) != null)
				{
					
					Texture tex =  getItemTexture(items[i,0]);
					GUI.DrawTexture (new Rect (quickToggle.x + SquareDim.x*i + SquareDim.x/16f, quickToggle.y + SquareDim.y/16f, SquareDim.x-SquareDim.x/8f, SquareDim.y - SquareDim.y/8f), tex);
						//}
				}
			}
		}
	}

	public void addItem(int itemCode)
	{
		if (itemCode != 2) {
			for (int y = 0; y < inventoryHeight; y++) {
				for (int x = 0; x < inventoryWidth; x++) {
					if (items [x, y] == itemCode) {
						x = y = inventoryHeight * 3;
					} else if (items [x, y] != 0)
						continue;
					else {
						if (items [x, y] != itemCode) {
							items [x, y] = itemCode;
							x = y = inventoryHeight * 3;
						}
					}
					break;
				}
			}
		}

	}
	public int getSelectedItem()
	{
		
		return items [(int)SelectedItem.x, (int)SelectedItem.y];
	}
	public Texture getItemTexture(int itemCode)
	{
		Texture currentTex = null;
		switch (itemCode) 
		{
		default:
			break;
		case 1:
			currentTex = WOODt;
			break;
		case 2:
			currentTex = null;
			break;
		case 3:
			currentTex = PLANKt;
			break;
		case 4:
			currentTex = STONEt;
			break;
		case 5:
			currentTex = IRON_OREt;
			break;
		case 6:
			currentTex = DIAMOND_OREt;
			break;
		case 7:
			currentTex = GOLD_OREt;
			break;
		case 8:
			currentTex = CRAFTINGt;
			break;
		case 9:
			currentTex = FURNACEt;
			break;
		case 10:
			currentTex = GRASSt;
			break;
		case 11:
			currentTex = DIRTt;
			break;
		case 12:
			currentTex = SANDt;
			break;
		}
		return currentTex;
	}
		
}
