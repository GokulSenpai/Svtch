using UnityEngine;

public class Highlighter : MonoBehaviour
{
    private Outline _outlineRef;
    private void Awake()
    {
        _outlineRef = gameObject.GetComponent<Outline>();
    }

    private void OnMouseOver()
    { 
        print ("Mouse is over GameObject.");
        _outlineRef.OutlineColor = Color.Lerp(Color.clear, Color.white, Mathf.PingPong(Time.time, 1));
    }

    private void OnMouseExit()
    {
        print ("Mouse is no longer on GameObject.");
        _outlineRef.OutlineColor = Color.Lerp(_outlineRef.OutlineColor, Color.clear, Mathf.PingPong(Time.time, 1));
        _outlineRef.OutlineColor = Color.clear;
    }
}