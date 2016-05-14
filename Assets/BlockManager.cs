using UnityEngine;
using System.Collections;

public class BlockManager : MonoBehaviour {

	public GameObject block;
	public int blockCount;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < blockCount; i++) {
			float x = ((i%4) * 1.6f) + transform.position.x - 0.6f;
			float row = Mathf.Floor (i/4.0f);
			float y = (row * -1.6f) + transform.position.y + 2.0f;
			float z = transform.position.z;
			Vector3 position = new Vector3 (x, y, z);
			GameObject blockClone = (GameObject)Instantiate (block, position, transform.rotation);
			blockClone.transform.name = "Block " + (i + 1);
			blockClone.transform.parent = transform;

			CubeScript cubeScript = blockClone.GetComponent<CubeScript> ();
			//cubeScript.StartSpinning ();
		}
		//Instantiate(block, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
