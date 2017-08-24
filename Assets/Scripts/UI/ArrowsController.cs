using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ArrowsController: MonoBehaviour {
    public GameObject player; 
	public GameObject arrow;
    public Text arrowText;
    public RectTransform miniMapPanel;
    public Canvas canvas;

    public float PANEL_HEIGHT = 195f;
    public float PANEL_WIDTH = 182f;
    public float WORLD_END_HEIGHT = 10000f;

    private GameObject originalArrow;

	// Use this for initialization
	void Start () {
        GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
        //originalArrow = arrow.

        foreach (GameObject pickup in pickUps) {
			createNewArrow (pickup);
            Debug.Log("created pickup");
		}
	}

	// caluclations from https://docs.unity3d.com/Manual/DirectionDistanceFromOneObjectToAnother.html
	void createNewArrow(GameObject pickup){
		Vector3 heading = pickup.transform.position - player.transform.position;
		float distance = heading.magnitude;
		Vector3 direction = heading / distance;

        float relativeHeight = (distance / WORLD_END_HEIGHT) * PANEL_HEIGHT;
        float relativeWidth = (distance / WORLD_END_HEIGHT) * PANEL_WIDTH;

        arrow.transform.rotation = Quaternion.Euler(direction.x, direction.y, direction.z);
        arrow.transform.position = new Vector3(relativeWidth, relativeHeight, 0f);
        arrowText.text = string.Format("{0:N2}", distance);

        Debug.Log(string.Format("Created arrow at distance:{0}", distance));
        Instantiate (arrow);


        //arrow.transform.position = arrowOriginalPos;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
