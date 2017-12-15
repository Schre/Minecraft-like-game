using UnityEngine;
using System.Collections;

public class World : MonoBehaviour {
	
	public static World currentWorld;
	public Inventory currentInventory;
	public int chunkWidth = 50, chunkHeight = 100, seed = 0;
	public float viewRange = 60;
	public int SURFACE_LEVEL = 10;

	public Chunk grassBiome;
	public Chunk sandBiome;
	// Use this for initialization
	void Awake () {
		currentInventory = GetComponent<Inventory>();
		currentWorld = this;
		if (seed == 0)
			seed = Random.Range(0, int.MaxValue);
	}
	
	// Update is called once per frame
	void Update () {
		ScanForChunks ();
	}
	void ScanForChunks()
	{
		for (float x = transform.position.x - viewRange; x < transform.position.x + viewRange; x += chunkWidth) 
		{
			for (float y = transform.position.y - viewRange; y < transform.position.y; y += viewRange) 
			{
				for (float z = transform.position.z - viewRange; z < transform.position.z + viewRange; z += chunkWidth) {
					Vector3 pos = new Vector3 (x, 0, z);
					Chunk chunk = Chunk.FindChunk (pos);
					if (chunk != null)
						continue; // inside of a chunk
					// create chunk
					pos.x = Mathf.Floor (pos.x / (float)chunkWidth) * chunkWidth;
					pos.z = Mathf.Floor (pos.z / (float)chunkWidth) * chunkWidth;
					Chunk chunkFab = grassBiome;
					var randomNum = Mathf.Round (Random.value * 1000);
					if (randomNum % 7 == 0) {
						chunkFab = sandBiome;
						chunkFab.isSandBiome = false;

					} else
						chunkFab.isGrassBiome = true;
					chunkFab.isGrassBiome = true;
					chunkFab.isHills = true;


					chunk = (Chunk)Instantiate (chunkFab, pos, Quaternion.identity);
				}
			}
		}
	}



}
