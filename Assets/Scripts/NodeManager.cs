using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public List<List<GameObject>> board = new List<List<GameObject>>();
    public List<GameObject> bench = new List<GameObject>();
    public GameObject benchNode;
    public GameObject boardNode;
    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < 8; i++)
        {
            Vector2 pos = new Vector2(-4.5f, 3.5f - i);
            GameObject b = Instantiate(benchNode,pos,Quaternion.identity) as GameObject;
            b.GetComponent<Node>().nType = NodeType.Bench;
            bench.Add(b);
        }
        for (int i = 0; i < 8; i++)
        {
            List<GameObject> a = new List<GameObject>();
            board.Add(a);
            for (int j = 0; j < 8; j++)
            {
                Vector2 pos = new Vector2(-2.5f + j, 3.5f - i);
                GameObject b = Instantiate(boardNode, pos, Quaternion.identity) as GameObject;
                b.GetComponent<Node>().nType = NodeType.Board;
                b.GetComponent<Node>().indices = new Vector2Int(i, j);
                board[i].Add(b);
            }
        }
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Node n = board[i][j].GetComponent<Node>();
                if (n.indices.x > 0)
                {
                    n.mAdjacent.Add(board[n.indices.x - 1][n.indices.y]);
                }
                if (n.indices.y > 0)
                {
                    n.mAdjacent.Add(board[n.indices.x][n.indices.y - 1]);
                }
                if (n.indices.x < 7)
                {
                    n.mAdjacent.Add(board[n.indices.x + 1][n.indices.y]);
                }
                if (n.indices.y < 7)
                {
                    n.mAdjacent.Add(board[n.indices.x][n.indices.y + 1]);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
