#define Neighbour8
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder 
{
    public class Node : IHeapItem<Node>
    {
#if Neighbour8
        static readonly int[] dRow = new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };
        static readonly int[] dCol = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };
#else
        static readonly int[] dRow = new int[] { -1, 0, 0, 1, };
        static readonly int[] dCol = new int[] { 0, -1, 1, 0, };
#endif
        public int HeapIndex { get; set; }
        public int X, Y;
        public bool Walkable;
        public int H, G, F;
        public Node LastNode;
        public Bounds Bounds;
        public Node(int x, int y, Bounds bounds)
        {
            X = x; Y = y;
            Bounds = bounds;
        }
        public Node SetWalkable(bool walkable)
        {
            Walkable = walkable;
            return this;
        }
        public int CompareTo(Node other)
        {
            int compare = F.CompareTo(other.F);
            if (compare == 0)
                compare = H.CompareTo(other.H);
            return -compare;
        }

        public static void ForeachNeighborNodes(Node[,] nodes, int row, int col, Action<Node> foreachAction)
        {
            int rows = nodes.GetLength(0);
            int cols = nodes.GetLength(1);
#if Neighbour8
            for (int i = 0; i < 8; i++)
#else
            for (int i = 0; i < 4; i++)
#endif
            {
                int newRow = row + dRow[i];
                int newCol = col + dCol[i];

                if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols)
                {
                    foreachAction(nodes[newRow, newCol]);
                }
            }
        }
    }

    public Node[,] Nodes;
    public Stack<Node> Find(Node start, Node end)
    {
        Stack<Node> pathStack = new Stack<Node>();
        Heap<Node> openList = new Heap<Node>(Nodes.GetLength(0) * Nodes.GetLength(1));
        HashSet<Node> closeSet = new HashSet<Node>();
        openList.Add(start);
        while (openList.Count > 0)
        {
            Node current = openList.RemoveFirst();
            closeSet.Add(current);
            if (current == end)
            {
                Node temp = end;
                while (temp.LastNode != null)
                {
                    pathStack.Push(temp);
                    temp = temp.LastNode;
                }
                pathStack.Push(start);
                break;
            }
            Node.ForeachNeighborNodes(Nodes, current.X, current.Y, n =>
            {
                if (n.Walkable && !closeSet.Contains(n))
                {
                    int g = current.G + GetDistance(current, n);
                    if (g < n.G || !openList.Contains(n))
                    {
                        n.G = g;
                        n.H = GetDistance(n, end);
                        n.LastNode = current;
                        if (!openList.Contains(n))
                            openList.Add(n);
                    }
                }
            });
        }
        return pathStack;
    }
    private int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.X - b.X);
        int dstY = Mathf.Abs(a.Y - b.Y);
        return 14 * dstY + 10 * Mathf.Abs(dstY - dstX);
    }
}
