using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeManager instance;

    public List<List<Node>> board = new List<List<Node>>();
    public List<Node> bench = new List<Node>();
    public GameObject benchNodeMiddlePrefab;
    public GameObject benchNodeTopPrefab;
    public GameObject benchNodeBottomPrefab;
    public GameObject boardNodeMiddlePrefab;
    public GameObject boardNodeTopLeftPrefab;
    public GameObject boardNodeTopMidPrefab;
    public GameObject boardNodeTopRightPrefab;
    public GameObject boardNodeMidLeftPrefab;
    public GameObject boardNodeMidRightPrefab;
    public GameObject boardNodeBotLeftPrefab;
    public GameObject boardNodeBotMidPrefab;
    public GameObject boardNodeBotRightPrefab;

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
            GameObject toSpawn = i == 0 ? this.benchNodeTopPrefab : i == 7 ? this.benchNodeBottomPrefab : this.benchNodeMiddlePrefab;
            GameObject b = Instantiate(toSpawn, pos, Quaternion.identity) as GameObject;
            b.GetComponent<Node>().nType = NodeType.Bench;
            bench.Add(b.GetComponent<Node>());
        }
        
        GameObject[][] boardPrefabs = {
            new GameObject[] { boardNodeTopLeftPrefab, boardNodeTopMidPrefab, boardNodeTopRightPrefab },
            new GameObject[] { boardNodeMidLeftPrefab, boardNodeMiddlePrefab, boardNodeMidRightPrefab },
            new GameObject[] { boardNodeBotLeftPrefab, boardNodeBotMidPrefab, boardNodeBotRightPrefab }
        };

        for (int i = 0; i < 8; i++)
        {
            List<Node> a = new List<Node>();
            board.Add(a);
            int boardI = i == 0 ? 0 : i == 7 ? 2 : 1;
            for (int j = 0; j < 8; j++)
            {
                int boardJ = j == 0 ? 0 : j == 7 ? 2 : 1;
                Vector2 pos = new Vector2(-2.5f + j, 3.5f - i);
                GameObject b = Instantiate(boardPrefabs[boardI][boardJ], pos, Quaternion.identity) as GameObject;
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
