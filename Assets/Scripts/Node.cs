using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    Bench,
    Board
};
public class Node : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> mAdjacent;
    public NodeType nType;
    public Vector2Int indices;
    public Character currObject;
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
