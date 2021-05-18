using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavNode : IDisposable, IEquatable<NavNode>
{
    public Vector2 pos { get; private set; }
    public bool IsObstocale { get; private set; }
    private HashSet<NavNode> Neighbours = new HashSet<NavNode>();
    private NavNode[] nBors;

    public bool Equals(NavNode other)
    {
        return other == this;
    }

    public NavNode(Vector2 pos, float size)
    {
        this.pos = pos;
        IsObstocale = Physics2D.BoxCast(pos, Vector2.one * size, 0, Vector2.zero, 50, LayerMask.GetMask("Default"));
    }

    public void AddNeighbour(NavNode node)
    {
        if (Neighbours.Contains(node)) return;
        Neighbours.Add(node);
        nBors = Neighbours.ToArray();
    }

    public NavNode[] GetNeighbours()
    {
        return nBors;
    }

    public void Dispose()
    {
        Neighbours.Clear();
        Neighbours = null;
    }
}
