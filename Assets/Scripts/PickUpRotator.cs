using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PickUpRotator : MonoBehaviour {
    public AudioClip soundPickUp;
    public AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
	}


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.clip = soundPickUp;
            audioSource.Play();
            this.gameObject.SetActive(false);
        }
    }
}
