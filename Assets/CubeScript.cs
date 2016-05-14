using UnityEngine;
using System.Collections;

public class CubeScript : MonoBehaviour {

	public Material[] availableMaterials;

	public GameObject back;
	public GameObject front;
	public GameObject bottom;
	public GameObject left;
	public GameObject right;
	public GameObject top;

	private float temps;

	private bool spinning;

	const int FRONT = 0;
	const int TOP = 1;
	const int BACK = 2;
	const int BOTTOM = 3;
	const int LEFT = 4;
	const int RIGHT = 5;

	const int UP_SWIPE = 6;
	const int DOWN_SWIPE = 7;
	const int LEFT_SWIPE = 8;
	const int RIGHT_SWIPE = 9;

	private int[] orientations = new int[6];
	private string[] faces = new string[6];

	//inside class
	Vector2 firstPressPos;
	Vector2 secondPressPos;
	Vector2 currentSwipe;

	// Use this for initialization
	void Start () {
		// Initialize
		for (int i = 0; i < 6; i++) {
			orientations [i] = i;
		}

		faces [FRONT] = "front";
		faces [TOP] = "top";
		faces [BACK] = "back";
		faces [BOTTOM] = "bottom";
		faces [LEFT] = "left";
		faces [RIGHT] = "right";


		MeshRenderer backMesh = back.GetComponent<MeshRenderer> ();
		backMesh.material = availableMaterials [Random.Range (0, availableMaterials.Length)];

		MeshRenderer frontMesh = front.GetComponent<MeshRenderer> ();
		frontMesh.material = availableMaterials [Random.Range (0, availableMaterials.Length)];

		MeshRenderer bottomMesh = bottom.GetComponent<MeshRenderer> ();
		bottomMesh.material = availableMaterials [Random.Range (0, availableMaterials.Length)];

		MeshRenderer leftMesh = left.GetComponent<MeshRenderer> ();
		leftMesh.material = availableMaterials [Random.Range (0, availableMaterials.Length)];

		MeshRenderer rightMesh = right.GetComponent<MeshRenderer> ();
		rightMesh.material = availableMaterials [Random.Range (0, availableMaterials.Length)];

		MeshRenderer topMesh = top.GetComponent<MeshRenderer> ();
		topMesh.material = availableMaterials [Random.Range (0, availableMaterials.Length)];
	}

	private int FindOriginalPosition(int position) {
		for (int i = 0; i < orientations.Length; i++) {
			if (orientations [i] == position)
				return i;
		}

		return -1;
	}

	private void PrintOrientation() {
		for (int i = 0; i < orientations.Length; i++) {
			Debug.Log (faces [orientations [i]] + " : ORIGINAL : " + faces [i]); 
		}
	}

	private void UpdateCurrentOrientation(int direction) {
		if(direction == UP_SWIPE) {
			int originalFront = orientations [FRONT];
			orientations [FRONT] = orientations [BOTTOM];
			orientations [BOTTOM] = orientations [BACK];
			orientations [BACK] = orientations [TOP];
			orientations [TOP] = originalFront;
		} else if(direction == DOWN_SWIPE) {
			int originalFront = orientations [FRONT];
			orientations [FRONT] = orientations [TOP];
			orientations [TOP] = orientations [BACK];
			orientations [BACK] = orientations [BOTTOM];
			orientations [BOTTOM] = originalFront;
		} else if(direction == LEFT_SWIPE) {
			int originalFront = orientations [FRONT];
			orientations [FRONT] = orientations [RIGHT];
			orientations [RIGHT] = orientations [BACK];
			orientations [BACK] = orientations [LEFT];
			orientations [LEFT] = originalFront;
		} else if(direction == RIGHT_SWIPE) {
			int originalFront = orientations [FRONT];
			orientations [FRONT] = orientations [LEFT];
			orientations [LEFT] = orientations [BACK];
			orientations [BACK] = orientations [RIGHT];
			orientations [RIGHT] = originalFront;
		}
	}

	public void Swipe() {
		if(Input.GetMouseButtonDown(0))
		{
			//save began touch 2d point
			firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
		}
		if(Input.GetMouseButtonUp(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			// Casts the ray and get the first game object hit
			Physics.Raycast(ray, out hit);
			if (hit.transform.name.Equals(transform.name)) {
				//save ended touch 2d point
				secondPressPos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);

				//create vector from the two points
				currentSwipe = new Vector2 (secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

				//normalize the 2d vector
				currentSwipe.Normalize ();

				Debug.Log ("SWIPE: " + hit.transform.name);

				Vector3 rotation = transform.rotation.eulerAngles;


				//float x = Mathf.Round (rotation.x);
				//float y = Mathf.Round (rotation.y);
				//float z = Mathf.Round (rotation.z);
				//Debug.Log (x + "," + y + "," + z);

				Hashtable rotationArgs = new Hashtable ();
				rotationArgs.Add ("time", 0.125f);
				//Quaternion euler = new Quaternion();

				//swipe upwards
				if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
					//Debug.Log ("up swipe: " + (rotation.x - 90.0f));
					UpdateCurrentOrientation(UP_SWIPE);
					int originalFrontPosition = FindOriginalPosition (FRONT);
					Debug.Log ("ORIGINAL FRONT IS " + faces [originalFrontPosition]);
					//transform.rotation 

					if (originalFrontPosition == LEFT) {
						rotationArgs.Add ("z", 90.0f);
					} else if (originalFrontPosition == RIGHT) {
						rotationArgs.Add ("z", -90.0f);
					} else if (orientations [LEFT] == LEFT) {
						// Right side up, and good to go
						rotationArgs.Add ("x", 90.0f);
					} else if (orientations [LEFT] == RIGHT) {
						rotationArgs.Add ("x", -90.0f);
					} else if (orientations [RIGHT] == TOP) {
						rotationArgs.Add ("y", 90.0f);
					} else if (orientations [LEFT] == TOP) {
						rotationArgs.Add ("y", -90.0f);
					}
				}
				//swipe down
				if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
					//Debug.Log ("down swipe: " + (rotation.x - 90.0f));
					UpdateCurrentOrientation(DOWN_SWIPE);
					int originalFrontPosition = FindOriginalPosition (FRONT);
					Debug.Log ("ORIGINAL FRONT IS " + faces [originalFrontPosition]);
					if (originalFrontPosition == LEFT) {
						rotationArgs.Add ("z", -90.0f);
					} else if (originalFrontPosition == RIGHT) {
						rotationArgs.Add ("z", 90.0f);
					} else if (orientations [LEFT] == LEFT) {
						// Right side up, and good to go
						rotationArgs.Add ("x", -90.0f);
					} else if (orientations [LEFT] == RIGHT) {
						rotationArgs.Add ("x", 90.0f);
					} else if (orientations [RIGHT] == TOP) {
						rotationArgs.Add ("y", -90.0f);
					} else if (orientations [LEFT] == TOP) {
						rotationArgs.Add ("y", 90.0f);
					}
				}
				//swipe left
				if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
					//Debug.Log ("left swipe");
					UpdateCurrentOrientation(LEFT_SWIPE);
					int originalLeftPosition = FindOriginalPosition (LEFT);
					Debug.Log ("ORIGINAL LEFT IS " + faces [originalLeftPosition]);

					if (originalLeftPosition == TOP) {
						rotationArgs.Add ("x", -90.0f);
					} else if (originalLeftPosition == BOTTOM) {
						rotationArgs.Add ("x", 90.0f);
					} else if (orientations [TOP] == TOP) {
						// Right side up, and good to go
						rotationArgs.Add ("y", 90.0f);
					} else if (orientations [TOP] == BOTTOM) {
						rotationArgs.Add ("y", -90.0f);
					} else if (orientations [TOP] == FRONT) {
						rotationArgs.Add ("z", -90.0f);
					} else if (orientations [BOTTOM] == FRONT) {
						rotationArgs.Add ("z", 90.0f);
					}
				}
				//swipe right
				if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
					//Debug.Log ("right swipe");
					UpdateCurrentOrientation(RIGHT_SWIPE);
					int originalLeftPosition = FindOriginalPosition (LEFT);
					Debug.Log ("ORIGINAL LEFT IS " + faces [originalLeftPosition]);

					if (originalLeftPosition == TOP) {
						rotationArgs.Add ("x", 90.0f);
					} else if(originalLeftPosition == BOTTOM) {
						rotationArgs.Add ("x", -90.0f);
					} else if (orientations [TOP] == TOP) {
						// Right side up, and good to go
						rotationArgs.Add ("y", -90.0f);
					} else if (orientations [TOP] == BOTTOM) {
						rotationArgs.Add ("y", 90.0f);
					} else if (orientations [TOP] == FRONT) {
						rotationArgs.Add ("z", 90.0f);
					} else if (orientations [BOTTOM] == FRONT) {
						rotationArgs.Add ("z", -90.0f);
					}
				}

				iTween.RotateAdd (hit.transform.gameObject, rotationArgs);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		Swipe ();
		/*if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			// Casts the ray and get the first game object hit
			Physics.Raycast(ray, out hit);
			if (hit.transform.name.StartsWith ("Block ")) {
				Swipe ();
				//Debug.Log ("This hit at " + hit.transform.name);
				//Debug.Log (hit.point);
				//Debug.Log (transform.position);
				//iTween.RotateBy (hit.transform.gameObject, new Vector3 (0.0f, 0.25f, 0.0f), .5f);
			}

		}*/

		if (this.spinning) {
			transform.Rotate (new Vector3 (80.0f * Time.deltaTime, 80.0f * Time.deltaTime, 80.0f * Time.deltaTime));
		}
	}

	public void StartSpinning() {
		this.spinning = true;
	}

	public void StopSpinning() {
		this.spinning = false;
	}
		
}
