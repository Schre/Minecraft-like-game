using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class map_texture : MonoBehaviour 
{

	// Use this for initialization
	public static void setGrassUvs (Vector2[] uvs) 
	{
		//var mf = GetComponent <MeshFilter>();
		//var mesh = GetComponent<Mesh>();
		//if (mf != mf.mesh)
		//	mesh = mf.mesh;
		//if (mesh == null || mesh.uv.Length != 24) 
		//{
		//	Debug.Log ("Attatch to cube");
		//}


		uvs [0].Set (0.0f, 0.0f);
		uvs [1].Set(0.333f, 0.0f);
		uvs [2].Set(0.0f, 0.333f);
		uvs [3].Set(0.333f, 0.333f);

		uvs [8].Set(0.334f, 0.0f);
		uvs [9].Set(0.666f, 0.0f);
		uvs [4].Set (0.334f, 0.333f);
		uvs [5].Set (0.666f, 0.333f);

		uvs [10].Set (0.667f, 0.0f);
		uvs [11].Set(1.0f, 0.0f);
		uvs [6].Set(0.667f, 0.333f);
		uvs [7].Set(1.0f, 0.333f);

		uvs [15].Set (0.0f, 0.334f);
		uvs [12].Set(0.333f, 0.334f);
		uvs [14] .Set (0.0f, 0.666f);
		uvs [13].Set (0.333f, 0.666f);

		uvs [19].Set (0.334f, 0.334f);
		uvs [16].Set(0.666f, 0.334f);
		uvs [18] .Set (0.334f, 0.666f);
		uvs [17].Set (0.666f, 0.666f);

		uvs [23].Set (0.667f, 0.334f);
		uvs [20].Set(1.0f, 0.334f);
		uvs [22] .Set (0.667f, 0.666f);
		uvs [21].Set (1.0f, 0.666f);
	}
}
