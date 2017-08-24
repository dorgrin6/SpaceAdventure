﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCollectible : MonoBehaviour {

    public Transform tracking;
    public Transform center;
    Transform pivot;

    void Start()
    {
        pivot = new GameObject().transform;
        pivot.position = center.position;

        float dist = Vector3.Distance(center.position, transform.position);
        pivot.LookAt(tracking);
        pivot.parent = center;
        transform.position = center.position + pivot.forward * dist;
        transform.LookAt(tracking);
        transform.parent = pivot;
    }

    void Update()
    {
        //pivot.LookAt(tracking);
        //Vector3 v3 = Math3d.ProjectPointOnPlane(center.up, center.position, tracking.position);
        //pivot.LookAt(v3);
    }
}
