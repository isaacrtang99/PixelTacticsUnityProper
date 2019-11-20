using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UnitManager : MonoBehaviour
{
    public List<Character> allies;
    public List<Character> enemies;
    GameObject nodeManager;
    List<List<GameObject>> board;
    List<GameObject> bench;
    public GameObject allySpawnPrefab;
    public GameObject allySpawnPrefabRanged;
    public GameObject enemySpawnPrefab;
    public GameObject enemySpawnPrefabRanged;
    public bool gameStarted;
    float addCooldown = 0.0f;
    float resetCooldown = 0.0f;
    int money = 10;
    int level = 0;
    int onBoard;
    // Start is called before the first frame update

    void Start()
    {
        allies = new List<Character>();
        enemies = new List<Character>();
        gameStarted = true;
        nodeManager = GameObject.Find("NodeManager");
        board = nodeManager.GetComponent<NodeManager>().board;
        bench = nodeManager.GetComponent<NodeManager>().bench;
    }

    // Update is called once per frame
    void Update()
    {

        addCooldown -= Time.deltaTime;
        resetCooldown -= Time.deltaTime;
        if (addCooldown < 0.0f && Input.GetKey("a") && money > 0) 
        {
            addCooldown = 1.0f;
            AddAlly();
        }
        if (addCooldown < 0.0f && Input.GetKey("s") && money > 0)
        {
            addCooldown = 1.0f;
            AddAllyRanged();
        }
        if (resetCooldown < 0.0f && Input.GetKey("space") && !gameStarted)
        {
           onBoard = 0;
            foreach (Character c in allies)
            {
                if (c.currNode.nType == NodeType.Board) onBoard++;
            }
            if (onBoard != 0)
            {
                gameStarted = true;
                startStage();
            }
        }
        if (gameStarted)
        {
            if(enemies.Count == 0)
            {
                money += 3;
                money += Mathf.Max(0,8 - onBoard);
                level++;
                CreateNewStage(level);
                gameStarted = false;
            }
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
                b.GetComponent<Character>().health = 1000;
                b.GetComponent<Character>().max_health = 1000;
                allies.Add(b.GetComponent<Character>());
                bench[i].GetComponent<Node>().SetUnit(b.GetComponent<Character>());
                money--;
                break;
            }
        }
    }
    void AddAllyRanged()
    {
        for (int i = 0; i < bench.Count; i++)
        {
            if (bench[i].GetComponent<Node>().GetUnit() == null)
            {
                GameObject b = Instantiate(allySpawnPrefabRanged, bench[i].transform.position, Quaternion.identity) as GameObject;
                b.GetComponent<Character>().SetNode(bench[i].GetComponent<Node>());
                b.GetComponent<Character>().type = "ally";
                b.GetComponent<Character>().health = 800;
                b.GetComponent<Character>().max_health = 800;
                allies.Add(b.GetComponent<Character>());
                bench[i].GetComponent<Node>().SetUnit(b.GetComponent<Character>());
                money--;
                break;
            }
        }
    }
    void CreateNewStage(int level)
    {
    
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
        if(level == 1)
        {
            CreateNewStage1();
        }
        else if(level == 2)
        {
            CreateNewStage2();
        }
        else if(level == 3)
        {
            CreateNewStage3();
        }
        else if(level == 4)
        {
            CreateNewStage4();
        }
        else if(level == 5)
        {
            CreateNewStage5();
        }
        else
        {
            CreateNewStage5();
        }
    }
    void CreateNewStage1()
    {
        List<GameObject> targetSlots = new List<GameObject>();
        targetSlots.Add(board[0][4]);
        targetSlots.Add(board[2][4]);
        targetSlots.Add(board[4][4]);
        targetSlots.Add(board[6][4]);

        for (int i = 0; i < targetSlots.Count; i++)
        {
            GameObject b = Instantiate(enemySpawnPrefab, targetSlots[i].transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Character>().SetNode(targetSlots[i].GetComponent<Node>());
            b.GetComponent<Character>().type = "enemy";
            b.GetComponent<Character>().health = 1000;
            b.GetComponent<Character>().max_health = 1000;
            enemies.Add(b.GetComponent<Character>());
            targetSlots[i].GetComponent<Node>().SetUnit(b.GetComponent<Character>());
        }
    }
    void CreateNewStage2()
    {
        List<GameObject> targetSlots = new List<GameObject>();
        targetSlots.Add(board[0][7]);
        targetSlots.Add(board[2][7]);
        targetSlots.Add(board[4][7]);
        targetSlots.Add(board[6][7]);

        for (int i = 0; i < targetSlots.Count; i++)
        {
            GameObject b = Instantiate(enemySpawnPrefabRanged, targetSlots[i].transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Character>().SetNode(targetSlots[i].GetComponent<Node>());
            b.GetComponent<Character>().type = "enemy";
            b.GetComponent<Character>().health = 800;
            b.GetComponent<Character>().max_health = 800;
            enemies.Add(b.GetComponent<Character>());
            targetSlots[i].GetComponent<Node>().SetUnit(b.GetComponent<Character>());
        }
    }
    void CreateNewStage3()
    {
        List<GameObject> targetRangedSlots = new List<GameObject>();
        targetRangedSlots.Add(board[1][7]);
        targetRangedSlots.Add(board[3][7]);
        targetRangedSlots.Add(board[5][7]);
        targetRangedSlots.Add(board[7][7]);

        List<GameObject> targetSlots = new List<GameObject>();
        targetSlots.Add(board[0][4]);
        targetSlots.Add(board[2][4]);
        targetSlots.Add(board[4][4]);
        targetSlots.Add(board[6][4]);

        for (int i = 0; i < targetSlots.Count; i++)
        {
            GameObject b = Instantiate(enemySpawnPrefab, targetSlots[i].transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Character>().SetNode(targetSlots[i].GetComponent<Node>());
            b.GetComponent<Character>().type = "enemy";
            b.GetComponent<Character>().health = 1000;
            b.GetComponent<Character>().max_health = 1000;
            enemies.Add(b.GetComponent<Character>());
            targetSlots[i].GetComponent<Node>().SetUnit(b.GetComponent<Character>());
        }

        for (int i = 0; i < targetRangedSlots.Count; i++)
        {
            GameObject b = Instantiate(enemySpawnPrefabRanged, targetRangedSlots[i].transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Character>().SetNode(targetRangedSlots[i].GetComponent<Node>());
            b.GetComponent<Character>().type = "enemy";
            b.GetComponent<Character>().health = 800;
            b.GetComponent<Character>().max_health = 800;
            enemies.Add(b.GetComponent<Character>());
            targetRangedSlots[i].GetComponent<Node>().SetUnit(b.GetComponent<Character>());
        }
    }
    void CreateNewStage4()
    {
        List<GameObject> targetRangedSlots = new List<GameObject>();
        targetRangedSlots.Add(board[3][6]);
        targetRangedSlots.Add(board[4][6]);
        targetRangedSlots.Add(board[5][6]);
        targetRangedSlots.Add(board[6][6]);

        List<GameObject> targetSlots = new List<GameObject>();
        targetSlots.Add(board[3][4]);
        targetSlots.Add(board[4][4]);
        targetSlots.Add(board[5][4]);
        targetSlots.Add(board[6][4]);

        for (int i = 0; i < targetSlots.Count; i++)
        {
            GameObject b = Instantiate(enemySpawnPrefab, targetSlots[i].transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Character>().SetNode(targetSlots[i].GetComponent<Node>());
            b.GetComponent<Character>().type = "enemy";
            b.GetComponent<Character>().health = 1000;
            b.GetComponent<Character>().max_health = 1000;
            enemies.Add(b.GetComponent<Character>());
            targetSlots[i].GetComponent<Node>().SetUnit(b.GetComponent<Character>());
        }

        for (int i = 0; i < targetRangedSlots.Count; i++)
        {
            GameObject b = Instantiate(enemySpawnPrefabRanged, targetRangedSlots[i].transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Character>().SetNode(targetRangedSlots[i].GetComponent<Node>());
            b.GetComponent<Character>().type = "enemy";
            b.GetComponent<Character>().health = 800;
            b.GetComponent<Character>().max_health = 800;
            enemies.Add(b.GetComponent<Character>());
            targetRangedSlots[i].GetComponent<Node>().SetUnit(b.GetComponent<Character>());
        }
    }
    void CreateNewStage5()
    {
        List<GameObject> targetRangedSlots = new List<GameObject>();
        targetRangedSlots.Add(board[1][5]);
        targetRangedSlots.Add(board[3][5]);
        targetRangedSlots.Add(board[5][5]);
        targetRangedSlots.Add(board[7][5]);
        targetRangedSlots.Add(board[0][6]);
        targetRangedSlots.Add(board[2][6]);
        targetRangedSlots.Add(board[4][6]);
        targetRangedSlots.Add(board[6][6]);

        List<GameObject> targetSlots = new List<GameObject>();
        targetSlots.Add(board[0][4]);
        targetSlots.Add(board[2][4]);
        targetSlots.Add(board[4][4]);
        targetSlots.Add(board[6][4]);

        for (int i = 0; i < targetSlots.Count; i++)
        {
            GameObject b = Instantiate(enemySpawnPrefab, targetSlots[i].transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Character>().SetNode(targetSlots[i].GetComponent<Node>());
            b.GetComponent<Character>().type = "enemy";
            b.GetComponent<Character>().health = 1000;
            b.GetComponent<Character>().max_health = 1000;
            enemies.Add(b.GetComponent<Character>());
            targetSlots[i].GetComponent<Node>().SetUnit(b.GetComponent<Character>());
        }

        for (int i = 0; i < targetRangedSlots.Count; i++)
        {
            GameObject b = Instantiate(enemySpawnPrefabRanged, targetRangedSlots[i].transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Character>().SetNode(targetRangedSlots[i].GetComponent<Node>());
            b.GetComponent<Character>().type = "enemy";
            b.GetComponent<Character>().health = 800;
            b.GetComponent<Character>().max_health = 800;
            enemies.Add(b.GetComponent<Character>());
            targetRangedSlots[i].GetComponent<Node>().SetUnit(b.GetComponent<Character>());
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
    public void RemoveAlly(Character a)
    {
        for(int i = 0; i < allies.Count; i++)
        {
            if (allies[i].Equals(a))
            {
                allies.RemoveAt(i);
            }
        }
    }
    public void RemoveEnemy(Character e)
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
