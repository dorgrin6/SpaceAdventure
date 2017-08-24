using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour {
    public GameObject[] trackedObjects;
    public GameObject radarPrefab;
    List<GameObject> radarObjects;
    List<GameObject> borderObjects;
    public float switchDistance;
    public Transform helpTransform;

	// Use this for initialization
	void Start () {
        createGameObjects();
	}

    private void createGameObjects()
    {
        radarObjects = new List<GameObject>();
        borderObjects = new List<GameObject>();

        foreach(GameObject go in trackedObjects)
        {
            GameObject newObject = Instantiate(radarPrefab, go.transform.position, Quaternion.identity) as GameObject;
            radarObjects.Add(newObject);
            GameObject newObject1 = Instantiate(radarPrefab, go.transform.position, Quaternion.identity) as GameObject;
            borderObjects.Add(newObject1);
        }
    }

    // Update is called once per frame
    void Update () {

		for( int i=0; i< radarObjects.Count; i++)
        {
            if (Vector3.Distance(radarObjects[i].transform.position, transform.position) > switchDistance)
            {
                // switch to border objects
                helpTransform.LookAt(radarObjects[i].transform);
                borderObjects[i].transform.position = transform.position + switchDistance * helpTransform.forward;
                borderObjects[i].layer = LayerMask.NameToLayer("Radar");
                radarObjects[i].layer = LayerMask.NameToLayer("Invisible");
            }
            else
            {
                // switch to radar objects
                borderObjects[i].layer = LayerMask.NameToLayer("Invisible");
                radarObjects[i].layer = LayerMask.NameToLayer("Radar");

            }
        }
	}
}
