using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour {

    //public GameObject gameController;
	// Use this for initialization
	void Start () {
       //gameController.
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            GameController.score++;
            Debug.Log("score is: " + GameController.score);
        }
    }
}
