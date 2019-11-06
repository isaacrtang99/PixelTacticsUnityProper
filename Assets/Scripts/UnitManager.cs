using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public List<Character> allies;
    public List<Character> enemies;
    GameObject nodeManager;
    List<List<GameObject>> board;
    List<GameObject> bench;
    public GameObject allySpawnPrefab;
    public GameObject enemySpawnPrefab;
    public bool gameStarted;
    float addCooldown = 0.0f;
    float resetCooldown = 0.0f;
    // Start is called before the first frame update

    void Start()
    {
        allies = new List<Character>();
        enemies = new List<Character>();
        gameStarted = false;
        nodeManager = GameObject.Find("NodeManager");
        board = nodeManager.GetComponent<NodeManager>().board;
        bench = nodeManager.GetComponent<NodeManager>().bench;
    }

    // Update is called once per frame
    void Update()
    {
        addCooldown -= Time.deltaTime;
        resetCooldown -= Time.deltaTime;
        if (addCooldown < 0.0f && Input.GetKey("a")) 
        {
            addCooldown = 1.0f;
            AddAlly();
        }
        if (resetCooldown < 0.0f && Input.GetKey("x"))
        {
            gameStarted = false;
            resetCooldown = 1.0f;
            CreateNewStage();
        }
        if (resetCooldown < 0.0f && Input.GetKey("space") && !gameStarted)
        {
            gameStarted = true;
            startStage();
        }
        /*for (int i = 0; i < bench.Count; i++)
        {
            if (bench[i].GetComponent<Node>().GetUnit() != null)
            {
                //bench[i].GetComponent<Node>().GetUnit().gameObject.SetActive(false);
            }
        }*/
    }

    void AddAlly()
    {
        for (int i = 0; i < bench.Count; i++)
        {
            if (bench[i].GetComponent<Node>().GetUnit() == null)
            {
                GameObject b = Instantiate(allySpawnPrefab, bench[i].transform.position, Quaternion.identity) as GameObject;
                b.GetComponent<Character>().SetNode(bench[i].GetComponent<Node>());
                b.GetComponent<Character>().type = "ally";
                b.GetComponent<Character>().health = 10;
                allies.Add(b.GetComponent<Character>());
                bench[i].GetComponent<Node>().SetUnit(b.GetComponent<Character>());
                break;
            }
        }
    }
    void CreateNewStage()
    {
        List<GameObject> emptySlots = new List<GameObject>();
    
        //wipe the stage
        for (int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if (board[i][j].GetComponent<Node>().GetUnit() != null)
                {
                    if(board[i][j].GetComponent<Node>().GetUnit().type == "ally")
                    {
                        RemoveAlly(board[i][j].GetComponent<Node>().GetUnit());
                    }
                    else
                    {
                        RemoveEnemy(board[i][j].GetComponent<Node>().GetUnit());
                    }
                    Destroy(board[i][j].GetComponent<Node>().GetUnit().gameObject);
                    board[i][j].GetComponent<Node>().SetUnit(null);
                }
            }
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 4; j < 8; j++)
            {
                emptySlots.Add(board[i][j]);
            }
        }

        for (int i = 0; i < 1; i++)
        {
            int r = Random.Range(0, emptySlots.Count);
            GameObject b = Instantiate(enemySpawnPrefab, emptySlots[r].transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Character>().SetNode(emptySlots[r].GetComponent<Node>());
            b.GetComponent<Character>().type = "enemy";
            b.GetComponent<Character>().health = 10;
            enemies.Add(b.GetComponent<Character>());
            emptySlots[r].GetComponent<Node>().SetUnit(b.GetComponent<Character>());
            emptySlots.RemoveAt(r);
        }
    }
    void startStage()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board[i][j].GetComponent<Node>().GetUnit() != null)
                {
                    board[i][j].GetComponent<Node>().GetUnit().cState = CharacterState.Active;
                }
            }
        }
    }
    void RemoveAlly(Character a)
    {
        for(int i = 0; i < allies.Count; i++)
        {
            if (allies[i].Equals(a))
            {
                allies.RemoveAt(i);
            }
        }
    }
    void RemoveEnemy(Character e)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].Equals(e))
            {
                enemies.RemoveAt(i);
            }
        }
    }

}
