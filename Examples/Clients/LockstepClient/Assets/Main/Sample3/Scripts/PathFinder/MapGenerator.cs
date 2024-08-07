using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public RectInt RectArea;
    public float Size;

    public PathFinder.Node[,] Nodes;
    public PathFinder Finder;

    public GameObject Target;
    public GameObject ClickPoint;
    private void Start()
    {
        
    }
    private void OnDrawGizmos()
    {
        CreateMaps(true,false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                ClickPoint.SetActive(false);
                ClickPoint.SetActive(true);
                ClickPoint.transform.position = hit.point;
                if (Nodes != null)
                {
                    PathFinder.Node selected = null;
                    selected = GetNode(hit.point);
                    if (selected != null)
                    {
                        var targetNode = GetNode(Target.transform.position);
                        Debug.LogWarning($"target {targetNode.Bounds.center} {targetNode.X} {targetNode.Y} selected:{selected.Bounds.center} {selected.X} {selected.Y}");
                        var path = Finder.Find(targetNode, selected);
                        Debug.LogWarning(path.Count);
                    }
                }
            }
        }
    }

    PathFinder.Node GetNode(Vector3 pos)
    {
        PathFinder.Node selected = null;
        float dist = float.MaxValue;
        foreach (var node in Nodes)
        {
            var d = Vector3.Distance(node.Bounds.center, pos);
            if (d < dist)
            {
                dist = d;
                selected = node;
            }
        }
        return selected;
    }
    void CreateMaps(bool debug, bool generate)
    {
        for (int i = 0; i <= RectArea.max.x + Math.Abs(RectArea.min.x); i++)
        {
            for (int j = 0; j <= RectArea.max.y + Math.Abs(RectArea.min.y); j++)
            {
                if (debug)
                {
                    Gizmos.DrawCube(new Vector3(i, 0, j) + new Vector3(RectArea.min.x, 0, RectArea.min.y), Size * Vector3.one);
                }
                if (generate)
                {
                    Nodes[i, j] = new PathFinder.Node(i, j, new Bounds(new Vector3(i, 0, j) + new Vector3(RectArea.min.x, 0, RectArea.min.y), Vector3.one * Size));
                    Nodes[i, j].SetWalkable(true);
                }
            }
        }
    }

    [ContextMenu("Create")]
    void Create()
    {
        Nodes = new PathFinder.Node[RectArea.max.x + Math.Abs(RectArea.min.x) + 1, RectArea.max.y + Math.Abs(RectArea.min.y) + 1];
        CreateMaps(false, true);
        Finder = new PathFinder();
        Finder.Nodes = Nodes;
    }
}

