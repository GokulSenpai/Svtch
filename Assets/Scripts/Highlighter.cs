using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    private Outline outlineRef;
    private void Start()
    {
        outlineRef = gameObject.GetComponent<Outline>();
    }

    void OnMouseOver()
    { 
        Debug.Log("Mouse is over GameObject.");
        outlineRef.OutlineColor = Color.Lerp(Color.clear, Color.white, Mathf.PingPong(Time.time, 1));
    }

    void OnMouseExit()
    {
        Debug.Log("Mouse is no longer on GameObject.");
        outlineRef.OutlineColor = Color.Lerp(outlineRef.OutlineColor, Color.clear, Mathf.PingPong(Time.time, 1));
        outlineRef.OutlineColor = Color.clear;
    }
}