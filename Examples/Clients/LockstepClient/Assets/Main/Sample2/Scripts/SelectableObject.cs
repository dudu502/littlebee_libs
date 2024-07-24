using Engine.Common.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectableObject : MonoBehaviour
{
    public enum SelectionEvent
    {
        Select,
        Deselect,
    }
    public bool isSelected = false;

    public Material highlightMaterial;
    private Material originalMaterial;

    void Start()
    {
        originalMaterial = GetComponent<MeshRenderer>().material;
    }


    public void Select()
    {
        isSelected = true;
        if (highlightMaterial) GetComponent<MeshRenderer>().material = highlightMaterial;
        EventDispatcher<SelectionEvent, SelectableObject>.DispatchEvent(SelectionEvent.Select, this);
    }

    public void Deselect()
    {
        isSelected = false;
        if (originalMaterial) GetComponent<MeshRenderer>().material = originalMaterial;
        EventDispatcher<SelectionEvent, SelectableObject>.DispatchEvent(SelectionEvent.Deselect, this);
    }
}
