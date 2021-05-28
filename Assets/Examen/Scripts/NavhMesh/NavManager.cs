using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examen.Managers;
using UnityEditor;
using UnityEngine;

public class NavManager : Singelton<NavManager>
{
    public Vector2 StartPoint
    {
        get => _startPoint;
        set
        {
            if (_startPoint == value) return;
            _startPoint = value;
            GenerateNav();
        }
    }

    private Vector2 _startPoint = new Vector2(0, 0);
    private Vector2 NodeCount = new Vector2(100, 100);

    private List<NavNode> CalculatingNodes = new List<NavNode>();

    private NavNode[] currentPath;

    private NavNode[] nodes;
    private static float size = .8f;

    public override void Awake()
    {
        if (nodes == null)
            GenerateNav();
    }

    public override void OnDrawGizmos()
    {
        if (nodes == null) return;
        HashSet<Vector2> drawnSet = new HashSet<Vector2>();

        Gizmos.color = Color.cyan;

        if (currentPath != null)
            foreach (var nodePath in currentPath)
            {
                drawnSet.Add(nodePath.pos);
                Gizmos.DrawCube(nodePath.pos, (Vector3.one * size * 0.9f));
            }

        Gizmos.color = Color.magenta;

        if (CalculatingNodes != null)
            foreach (var VARIABLE in CalculatingNodes)
            {
                drawnSet.Add(VARIABLE.pos);
                Gizmos.DrawCube(VARIABLE.pos, (Vector3.one * size * 0.9f));
            }
        foreach (var node in nodes)
        {
            if (node == null) continue;
            if (drawnSet.Contains(node.pos)) continue;

            Gizmos.color = node.IsObstocale ? Color.red : Color.black;
            Gizmos.DrawCube(node.pos, (Vector3.one * size * 0.9f));
        }
    }

    public async Task<NavNode[]> FindPath(Vector2 pos, Vector2[] targetPos)
    {
        CalculatingNodes = new List<NavNode>();
        currentPath = null;
        Dictionary<NavNode, NavNode> path = new Dictionary<NavNode, NavNode>();
        HashSet<NavNode> SearchedNodes = new HashSet<NavNode>();
        Stack<NavNode> ToSearch = new Stack<NavNode>();
        HashSet<NavNode> Targetnodes = new HashSet<NavNode>();
        NavNode start = findClosestNode(pos);

        ToSearch.Push(start);

        {
            for (int i = 0; i < targetPos.Length; i++)
            {
                Targetnodes.Add(findClosestNode(targetPos[i]));
            }
        }



        while (ToSearch.Count != 0)
        {
            NavNode currentNode = ToSearch.Pop();
            CalculatingNodes.Add(currentNode);

            if (Targetnodes.Contains(currentNode))
            {
                return GetPath(ref path, currentNode);
            }

            foreach (var neighbour in currentNode.GetNeighbours())
            {
                if (neighbour == null)
                    continue;
                if (SearchedNodes.Contains(neighbour))
                {
                    continue;
                }
                if (neighbour.IsObstocale) continue;

                if (neighbour != currentNode)
                    path.Add(neighbour, currentNode);

                ToSearch.Push(neighbour);
                SearchedNodes.Add(neighbour);
            }

            await Task.Delay(1);
        }

        Debug.Log("No Path found");
        currentPath = null;
        return null;
    }

    private NavNode findClosestNode(Vector2 pos)
    {
        float distance = -1;
        NavNode closeNode = null;
        foreach (var VARIABLE in nodes)
        {
            if (closeNode != null && !((VARIABLE.pos - pos).magnitude < distance)) continue;

            distance = (VARIABLE.pos - pos).magnitude;
            closeNode = VARIABLE;

        }
        return closeNode;
    }

    private NavNode[] GetPath(ref Dictionary<NavNode, NavNode> nodes, NavNode start)
    {
        HashSet<NavNode> path = new HashSet<NavNode>();
        path.Add(start);

        NavNode currenNode = start;
        while (true)
        {
            if (!nodes.ContainsKey(currenNode))
            {
                Debug.LogError("Failed to find key");
                break;
            }
            currenNode = nodes[currenNode];
            if (path.Contains(currenNode))
            {
                Debug.LogError("OOF");
                break;
            }
            path.Add(currenNode);
        }
        currentPath = path.ToArray();
        return currentPath;
    }

    private void GenerateNav()
    {
        Debug.LogWarning("Generating nav");


        if (nodes != null)
            foreach (var VARIABLE in nodes)
            {
                VARIABLE.Dispose();
            }

        nodes = new NavNode[(int)(NodeCount.x * NodeCount.y)];
        for (int y = 0; y < NodeCount.y; y++)
        {
            for (int x = 0; x < NodeCount.x; x++)
            {

                NavNode newNode = new NavNode(_startPoint + Vector2.left * x * size + Vector2.up * y * size, size);
                {

                    if (x != 0)
                    {
                        NavNode OtherNode = nodes[(x - 1) + y * (int)NodeCount.y];
                        OtherNode.AddNeighbour(newNode);
                        newNode.AddNeighbour(OtherNode);
                    }

                    if (y != 0)
                    {
                        NavNode OtherNode = nodes[x + (y - 1) * (int)NodeCount.y];
                        OtherNode.AddNeighbour(newNode);
                        newNode.AddNeighbour(OtherNode);
                    }

                    if (x != 0 && y != 0)
                    {
                        //      NavNode OtherNode = nodes[(x - 1) + (y - 1) * (int)NodeCount.y];
                        //      OtherNode.AddNeighbour(newNode);
                        //     newNode.AddNeighbour(OtherNode);
                    }



                }

                nodes[x + y * (int)NodeCount.y] = newNode;
            }
        }
    }
}
