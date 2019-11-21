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
    public List<Node> mAdjacent;
    public NodeType nType;
    public Vector2Int indices;
    public Character currChar = null;
    void Start()
    {
        currChar = null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetUnit(Character c)
    {
        currChar = c;
    }
    public Character GetUnit()
    {
        return currChar;
    }

}
