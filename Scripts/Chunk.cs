using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimplexNoise;


[RequireComponent (typeof(MeshRenderer))]
[RequireComponent (typeof(MeshCollider))]
[RequireComponent (typeof(MeshFilter))]

public class Chunk : MonoBehaviour {
	byte WOOD = 1, LEAVES = 2, PLANK = 3, STONE = 4, IRON_ORE = 5, DIAMOND_ORE = 6, GOLD_ORE = 7, CRAFTING = 8, FURNACE = 9, GRASS = 10, DIRT = 11, SAND = 12;
	public static List<Chunk> chunks = new List<Chunk>();
	public bool isSandBiome = false, isGrassBiome = false, isHills = false, isMountains = false, isPlain = false;
	public static int width
	{
		get{ return World.currentWorld.chunkWidth; }
	}
	public static int height
	{
		get{ return World.currentWorld.chunkHeight; }
	}

	public byte[,,] map;
	public Mesh visualMesh;
	protected MeshRenderer meshRenderer;
	protected MeshCollider meshCollider;
	protected MeshFilter meshFilter;

	// Use this for initialization
	void Start () {
		chunks.Add (this);

		meshRenderer = GetComponent<MeshRenderer>();
		meshCollider = GetComponent<MeshCollider>();
		meshFilter = GetComponent<MeshFilter>();
		
		
	
		map = new byte[World.currentWorld.chunkWidth, World.currentWorld.chunkHeight, World.currentWorld.chunkWidth];

		Random.seed = World.currentWorld.seed;
		Vector3 grain0Offset = new Vector3 (Random.value * 99999, Random.value * 99999, Random.value * 99999);
		Vector3 grain1Offset = new Vector3 (Random.value * 99999, Random.value * 99999, Random.value * 99999);
		Vector3 grain2Offset = new Vector3 (Random.value * 99999, Random.value * 99999, Random.value * 99999);

		for (int x = 0; x < World.currentWorld.chunkWidth; x++)
		{					
			for (int y = 0; y < World.currentWorld.chunkHeight; y++)
			{
				for (int z = 0; z < World.currentWorld.chunkWidth; z++)
				{
					var pos = new Vector3 (x, y, z);
					pos += transform.position;
					float noiseValue = 0f;
					if (isHills) {
						noiseValue = CalculateNoiseValue (pos, grain0Offset, 1f);
						noiseValue = Mathf.Max (noiseValue, CalculateNoiseValue (pos, grain1Offset, .04f));
						noiseValue = Mathf.Max (CalculateNoiseValue (pos, grain2Offset, .03f));
					} else if (isMountains) {
						noiseValue = CalculateNoiseValue (pos, grain0Offset, .03f);
						noiseValue += CalculateNoiseValue (pos, grain1Offset, .02f);
						noiseValue += CalculateNoiseValue (pos, grain2Offset, .01f);
					} else if (isPlain) {
						noiseValue = CalculateNoiseValue (pos, grain0Offset, .003f);
					}


					noiseValue += (15f - (float)y) / 10;

					if (y == World.currentWorld.SURFACE_LEVEL) {
						map [x, y, z] = GRASS;
					} else if (y < World.currentWorld.SURFACE_LEVEL) {
						float randval = Random.value * 99999999 % 1000;
						if (randval < 3) {
							map [x, y, z] = DIAMOND_ORE;
						} else if (randval < 8) {
							map [x, y, z] = GOLD_ORE;
						} else if (randval < 20) {
							map [x, y, z] = IRON_ORE;
						} else
							map [x, y, z] = STONE;
					} else if (noiseValue > 0.03f) 
					{
						map [x, y, z] = DIRT;
					}
				
				}
			}
		}

		// PLACE GRASS BITCH
		for (int x = 0; x < World.currentWorld.chunkWidth; x++) {
			for (int y = World.currentWorld.SURFACE_LEVEL; y < World.currentWorld.chunkHeight; y++) {
				for (int z = 0; z < World.currentWorld.chunkWidth; z++) 
				{
					if (map [x, y, z] == DIRT) {
						if (map [x, y + 1, z] == 0) {
							
							map [x, y, z] = GRASS;
							if (y < 50 && x > 0 && x < World.currentWorld.chunkWidth - 1 && z > 0 && z < World.currentWorld.chunkWidth - 1) {
								var randVal = Random.value * 24590823 % 1000;
								// DRAW TREE
								if (randVal < 20) 
								{
									randVal = Random.value * 3525423 % 5 + 5;
									for (int i = 0; i < randVal; i++) {
										map [x, y + i, z] = WOOD;
										if (i < randVal - 3 && i > 2) {
											map [x, y + i, z + 1] = LEAVES;
											map [x, y + i, z - 1] = LEAVES;
											map [x + 1, y + i, z] = LEAVES;
											map [x - 1, y + i, z] = LEAVES;
											map [x + 1, y + i, z + 1] = LEAVES;
											map [x + 1, y + i, z - 1] = LEAVES;
											map [x - 1, y + i, z + 1] = LEAVES;
											map [x - 1, y + i, z - 1] = LEAVES;
										} else if (i < randVal - 2 && i > 2) {
											map [x, y + i, z + 1] = LEAVES;
											map [x, y + i, z - 1] = LEAVES;
											map [x + 1, y + i, z] = LEAVES;
											map [x - 1, y + i, z] = LEAVES;
										} else if (i > 2){
											map [x, y + i, z + 1] = LEAVES;
											map [x, y + i, z - 1] = LEAVES;
											map [x + 1, y + i, z] = LEAVES;
											map [x - 1, y + i, z] = LEAVES;
											map [x, y + i + 1, z] = LEAVES;
										}
									}

								}
								// DONE DRAWING TREE
							}

						}
					}
				}
			}

		}
		StartCoroutine (CreateVisualMesh ());
		//CreateVisualMesh();
		
	}

	public virtual float CalculateNoiseValue(Vector3 pos, Vector3 offset, float scale)
	{
		float noiseX = Mathf.Abs((pos.x + offset.x) * scale);
		float noiseY = Mathf.Abs((pos.y + offset.y) * scale);
		float noiseZ = Mathf.Abs((pos.z + offset.z) * scale);

		return Noise.Generate (noiseX, noiseY, noiseZ);
	}
	// Update is called once per frame
	void Update () {
	
	}
	
	public virtual IEnumerator CreateVisualMesh() {
		visualMesh = new Mesh();
		
		List<Vector3> verts = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> tris = new List<int>();
		
		
		for (int x = 0; x < World.currentWorld.chunkWidth; x++)
		{
			for (int y = 0; y < World.currentWorld.chunkHeight; y++)
			{
				for (int z = 0; z < World.currentWorld.chunkWidth; z++)
				{
					if (map[x,y,z] == 0) continue;
					
					byte brick = map[x,y,z];
					// Left wall
					if (IsTransparent(x - 1, y, z))
						BuildFace (brick, 1, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris);
					// Right wall
					if (IsTransparent(x + 1, y , z))
						BuildFace (brick, 2, new Vector3(x + 1, y, z), Vector3.up, Vector3.forward, true, verts, uvs, tris);
					
					// Bottom wall
					if (IsTransparent(x, y - 1 , z))
						BuildFace (brick, 3, new Vector3(x, y, z), Vector3.forward, Vector3.right, false, verts, uvs, tris);
					// Top wall
					if (IsTransparent(x, y + 1, z))
						BuildFace (brick, 4, new Vector3(x, y + 1, z), Vector3.forward, Vector3.right, true, verts, uvs, tris);
					
					// Back
					if (IsTransparent(x, y, z - 1))
						BuildFace (brick, 5, new Vector3(x, y, z), Vector3.up, Vector3.right, true, verts, uvs, tris);
					// Front
					if (IsTransparent(x, y, z + 1))
						BuildFace (brick, 6, new Vector3(x, y, z + 1), Vector3.up, Vector3.right, false, verts, uvs, tris);
				}
			}
		}
					
		visualMesh.vertices = verts.ToArray();
		visualMesh.uv = uvs.ToArray();
		visualMesh.triangles = tris.ToArray();
		visualMesh.RecalculateBounds();
		visualMesh.RecalculateNormals();
		
		meshFilter.mesh = visualMesh;
		meshCollider.sharedMesh = visualMesh;


		yield return 0;
	}
	public virtual void CalculateMapFromScratch()
	{
	}
	public virtual void BuildFace(byte brick, int face, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
	{
		int index = verts.Count;

		verts.Add (corner);
		verts.Add (corner + up);
		verts.Add (corner + up + right);
		verts.Add (corner + right);
		if (brick == GRASS) {
			if (face == 1 || face == 2 || face == 5 || face == 6) {
				uvs.Add (new Vector2 (0.0f, 0.0f));
				uvs.Add (new Vector2 (0.0f, 0.25f));
				uvs.Add (new Vector2 (0.25f, 0.25f));
				uvs.Add (new Vector2 (0.25f, 0.0f));
			} else if (face == 4) {
				uvs.Add (new Vector2 (0.25f, 0.0f));
				uvs.Add (new Vector2 (0.25f, 0.25f));
				uvs.Add (new Vector2 (0.5f, 0.25f));
				uvs.Add (new Vector2 (0.5f, 0.0f));
			} else if (face == 3) {
				uvs.Add (new Vector2 (0.5f, 0.0f));
				uvs.Add (new Vector2 (0.5f, 0.25f));
				uvs.Add (new Vector2 (0.75f, 0.25f));
				uvs.Add (new Vector2 (0.75f, 0.334f));
			}
		} else if (brick == STONE) {
			uvs.Add (new Vector2 (0f, .50f));
			uvs.Add (new Vector2 (0f, 0.75f));
			uvs.Add (new Vector2 (.25f, .75f));
			uvs.Add (new Vector2 (.25f, 0.50f));

		} else if (brick == IRON_ORE) {
			uvs.Add (new Vector2 (.25f, .50f));
			uvs.Add (new Vector2 (.25f, 0.75f));
			uvs.Add (new Vector2 (.5f, .75f));
			uvs.Add (new Vector2 (.5f, 0.50f));

		} else if (brick == DIAMOND_ORE) {
			uvs.Add (new Vector2 (.5f, .50f));
			uvs.Add (new Vector2 (.5f, 0.75f));
			uvs.Add (new Vector2 (.75f, .75f));
			uvs.Add (new Vector2 (.75f, 0.50f));

		} else if (brick == GOLD_ORE) {
			uvs.Add (new Vector2 (.75f, .50f));
			uvs.Add (new Vector2 (.75f, 0.75f));
			uvs.Add (new Vector2 (1f, .75f));
			uvs.Add (new Vector2 (1f, 0.50f));

		} else if (brick == DIRT) {
			uvs.Add (new Vector2 (.5f, 0f));
			uvs.Add (new Vector2 (.5f, 0.25f));
			uvs.Add (new Vector2 (.75f, .25f));
			uvs.Add (new Vector2 (.75f, 0f));
		} else if (brick == WOOD) {
			if (face == 1 || face == 2 || face == 5 || face == 6) {
				uvs.Add (new Vector2 (0.25f, 0.75f));
				uvs.Add (new Vector2 (0.25f, 1f));
				uvs.Add (new Vector2 (0.5f, 1f));
				uvs.Add (new Vector2 (0.5f, 0.75f));
			} else 
			{
				uvs.Add (new Vector2 (0.0f, 0.75f));
				uvs.Add (new Vector2 (0.0f, 1f));
				uvs.Add (new Vector2 (0.25f, 1f));
				uvs.Add (new Vector2 (0.25f, 0.75f));
			} 
			
		}
		else if (brick == LEAVES) {
			uvs.Add (new Vector2 (.5f, .75f));
			uvs.Add (new Vector2 (.5f, 1f));
			uvs.Add (new Vector2 (.75f, 1f));
			uvs.Add (new Vector2 (.75f, .75f));
		}

		
		if (reversed)
		{
			tris.Add(index + 0);
			tris.Add(index + 1);
			tris.Add(index + 2);
			tris.Add(index + 2);
			tris.Add(index + 3);
			tris.Add(index + 0);
		}
		else
		{
			tris.Add(index + 1);
			tris.Add(index + 0);
			tris.Add(index + 2);
			tris.Add(index + 3);
			tris.Add(index + 2);
			tris.Add(index + 0);
		}
		
	}
	public virtual bool IsTransparent (int x, int y, int z)
	{
		byte brick = GetByte(x,y,z);
		switch (brick)
		{
		default:
		case 0: 
			return true;
			
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
		case 11:
		case 12:return false;
		}
	}
	public virtual byte GetByte (int x, int y , int z)
	{
		if ( (x < 0) || (y < 0) || (z < 0) || (y >= World.currentWorld.chunkHeight) || (x >= World.currentWorld.chunkWidth) || (z >= World.currentWorld.chunkWidth))
			return 0;
		return map[x,y,z];
	}

	public static Chunk FindChunk(Vector3 pos)
	{
		Chunk tempChunk = null;
		for (int a = 0; a < chunks.Count; a++) 
		{
			Vector3 cpos = chunks[a].transform.position;
			// check to remove chunks
			if (pos.x < cpos.x || pos.y < cpos.y || pos.y > (cpos.y + World.currentWorld.chunkHeight) || pos.z < cpos.z || pos.x > (cpos.x + World.currentWorld.chunkWidth) || pos.z > (cpos.z + World.currentWorld.chunkWidth)) 
			{
				continue; // not inside this chunk...
			}
			else
			tempChunk = chunks [a]; // inside a chunk
		}
		return tempChunk;
	}
	public bool SetBrick (byte brick, Vector3 worldPos)
	{
		worldPos -= transform.position;
		return SetBrick (brick, Mathf.FloorToInt (worldPos.x), Mathf.FloorToInt (worldPos.y), Mathf.FloorToInt (worldPos.z));  
	}
	public bool SetBrick (byte brick, int x, int y, int z)
	{
		if (x < 0 || y < 0 || z < 0 || x >= width || y >= height || z >= width) 
		{
			return false;
		}
		byte deletedBrick = map [x, y, z];
		if (map [x, y, z] == brick)
			return false;
		map [x, y, z] = brick;
		StartCoroutine (CreateVisualMesh ());

		if (x == 0) 
		{
			Chunk chunk = FindChunk (new Vector3 (x - 2, y, z) + transform.position);
			if (chunk != null)
			StartCoroutine (chunk.CreateVisualMesh ());
		}
		if (x == width - 1) 
		{
			Chunk chunk = FindChunk (new Vector3 (x + 2, y, z) + transform.position);
			if (chunk != null)
			StartCoroutine (chunk.CreateVisualMesh ());
		}
		if (z == 0) 
		{
			Chunk chunk = FindChunk (new Vector3 (x, y, z - 2) + transform.position);
			if (chunk != null)
			StartCoroutine (chunk.CreateVisualMesh ());
		}
		if (z == width - 1) 
		{
			Chunk chunk = FindChunk (new Vector3 (x, y, z + 2) + transform.position);
			if (chunk != null)
			StartCoroutine (chunk.CreateVisualMesh ());
		}
		World.currentWorld.GetComponent<Inventory> ().addItem (deletedBrick);
		return true;
	}

	
}




