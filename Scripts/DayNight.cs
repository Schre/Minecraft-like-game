using UnityEngine;
using System.Collections;

public class DayNight : MonoBehaviour {

	// Use this for initialization
	public static DayNight currentTime;
	public bool isDay;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		StartCoroutine(rotate ());
	
	}
	IEnumerator rotate()
	{
		transform.Rotate(new Vector3(.05f, 0, 0));
		yield return new WaitForSeconds (.2f);
	}
}
