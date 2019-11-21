using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeManager instance;

    public List<List<Node>> board = new List<List<Node>>();
    public List<Node> bench = new List<Node>();
    public GameObject benchNodePrefab;
    public GameObject boardNodePrefab;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < 8; i++)
        {
            Vector2 pos = new Vector2(-4.5f, 3.5f - i);
            GameObject b = Instantiate(benchNodePrefab, pos, Quaternion.identity) as GameObject;
            b.GetComponent<Node>().nType = NodeType.Bench;
            bench.Add(b.GetComponent<Node>());
        }
        for (int i = 0; i < 8; i++)
        {
            List<Node> a = new List<Node>();
            board.Add(a);
            for (int j = 0; j < 8; j++)
            {
                Vector2 pos = new Vector2(-2.5f + j, 3.5f - i);
                GameObject b = Instantiate(boardNodePrefab, pos, Quaternion.identity) as GameObject;
                b.GetComponent<Node>().nType = NodeType.Board;
                b.GetComponent<Node>().indices = new Vector2Int(i, j);
                board[i].Add(b.GetComponent<Node>());
            }
        }
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Node n = board[i][j];
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
}
