using Client.Lockstep.Behaviours.Data;
using Engine.Common.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SelectableObject;

public class Sample2 : Sample
{
    // Start is called before the first frame update
    void Start()
    {
        EventDispatcher<SelectionEvent, SelectableObject>.AddListener(SelectionEvent.Select, OnSelectObject);
        EventDispatcher<SelectionEvent, SelectableObject>.AddListener(SelectionEvent.Deselect, OnDeselectObject);
    }

    void OnSelectObject(SelectableObject selectableObject)
    {
        if (selectableObject != null)
        {
            if(selectableObject.TryGetComponent<AppearanceRenderer>(out var component))
            {
                if(!Selection.SelectedIds.Contains(component.EntityId))
                {
                    Selection.SelectedIds.Add(component.EntityId);
                }
            }
        }
    }

    void OnDeselectObject(SelectableObject deselectableObject)
    {
        if(deselectableObject != null)
        {
            if(deselectableObject.TryGetComponent<AppearanceRenderer>(out var component))
            {
                Selection.SelectedIds.Remove(component.EntityId);
            }
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}
