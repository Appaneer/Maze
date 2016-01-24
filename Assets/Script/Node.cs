using UnityEngine;
using System.Collections;

[System.Serializable]
public class Node : IHeapItem<Node>
{
    public int number;//each node has a different number
    public bool isVisited;// has the cell been visited
    public float x;
    public float y;
    public GameObject north;//1
    public GameObject west;//2
    public GameObject east;//3
    public GameObject south;//4

    int heapIndex;
    public Node parent;
    public int gCost;
    public int hCost;
    public int fCost {
        get {
            return gCost + hCost;
        }
    }

    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare) {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0) {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
