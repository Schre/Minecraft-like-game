using UnityEngine;
using System.Collections;

public class PlayerIO : MonoBehaviour {

	public static PlayerIO currentPlayerIO;
	public float clickRange = 10f;
	void Start () {
		currentPlayerIO = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (!World.currentWorld.currentInventory.isOpen) {
			getLeftClick ();
			getRightClick ();
		}

	}
	public void getLeftClick()
	{
		if (!Input.GetMouseButtonDown (0))
			return;
	
		Ray ray = GetComponent<Camera> ().ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0.5f));
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 8)) 
		{
			Chunk chunk = hit.transform.GetComponent<Chunk> ();
			Vector3 p = hit.point;
			if (chunk) 
			{
				p -= hit.normal/2;
				chunk.SetBrick (0, p);
			}
		} else 
		{
			Debug.Log ("Clicked nothing.");
		}
	}
	public void getRightClick()
	{
		if (!Input.GetMouseButtonDown (1))
			return;
		
		byte brick = (byte)World.currentWorld.currentInventory.getSelectedItem ();
		Ray ray = GetComponent<Camera> ().ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0.5f));
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 8)) 
		{
			Chunk chunk = hit.transform.GetComponent<Chunk> ();
			Vector3 p = hit.point;
			if (chunk) 
			{
				p -= ray.direction.normalized/50;
				//if (chunk.GetByte(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y + 1f), Mathf.RoundToInt(p.z)) == 0)

				chunk.SetBrick (brick, p);

			}
		} else 
		{
			Debug.Log ("Clicked nothing.");
		}
	}
}
