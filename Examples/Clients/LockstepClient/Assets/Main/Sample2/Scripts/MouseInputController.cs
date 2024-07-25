using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputController : MonoBehaviour
{
    public LayerMask selectableLayer;
    public SelectableObject currentSelection;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, selectableLayer))
            {
                SelectableObject selectable = hit.collider.GetComponent<SelectableObject>();
                if (selectable != null)
                {
                    if (!selectable.isSelected)
                        selectable.Select();
                    else
                        selectable.Deselect();
              
                    currentSelection = selectable;
                }
                else if (currentSelection != null)
                {
                    currentSelection.Deselect();
                    currentSelection = null;
                }
            }
            else if (currentSelection != null)
            {
                currentSelection.Deselect();
                currentSelection = null;
            }
        }
    }
}
