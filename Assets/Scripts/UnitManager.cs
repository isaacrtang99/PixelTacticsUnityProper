using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

    public List<Character> allies;
    public List<Character> enemies;
    public GameObject allySpawnPrefab;
    public GameObject allySpawnPrefabRanged;
    public GameObject enemySpawnPrefab;
    public GameObject enemySpawnPrefabRanged;
    public bool gameStarted;
    float addCooldown = 0.0f;
    float resetCooldown = 0.0f;
    public static int money = 10;
    int level = 0;
    int onBoard;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        allies = new List<Character>();
        enemies = new List<Character>();
        gameStarted = true;
    }

    // Update is called once per frame
    void Update()
    {

        addCooldown -= Time.deltaTime;
        resetCooldown -= Time.deltaTime;

        if (gameStarted)
        {
            if (enemies.Count == 0)
            {
                money += 3;
                money += Mathf.Max(0, 8 - onBoard);
                level++;
                CreateNewStage(level);
                gameStarted = false;
            }
        }

        /*for (int i = 0; i < NodeManager.instance.bench.Count; i++)
        {
            if (NodeManager.instance.bench[i].GetComponent<Node>().GetUnit() != null)
            {
                //NodeManager.instance.bench[i].GetComponent<Node>().GetUnit().gameObject.SetActive(false);
            }
        }*/
    }

    public void StartGameButtonClick()
    {
        if (gameStarted) return;

        onBoard = 0;
        foreach (Character c in allies)
        {
            if (c.currNode.nType == NodeType.Board) onBoard++;
        }
        if (onBoard == 0) return;

        gameStarted = true;
        startStage();
    }

    public void AddMeleeButton()
    {
        if (money <= 0) return;

        AddAlly();

    }

    public void AddRangedButton()
    {
        if (money <= 0) return;

        AddAllyRanged();

    }

    void AddAlly()
    {
        for (int i = 0; i < NodeManager.instance.bench.Count; i++)
        {
            if (NodeManager.instance.bench[i].GetUnit() == null)
            {
                GameObject b = Instantiate(allySpawnPrefab, NodeManager.instance.bench[i].transform.position, Quaternion.identity) as GameObject;
                b.GetComponent<Character>().SetNode(NodeManager.instance.bench[i]);
                b.GetComponent<Character>().type = "ally";
                b.GetComponent<Character>().health = 1000;
                b.GetComponent<Character>().max_health = 1000;
                allies.Add(b.GetComponent<Character>());
                NodeManager.instance.bench[i].SetUnit(b.GetComponent<Character>());
                money--;
                break;
            }
        }
    }
    void AddAllyRanged()
    {
        for (int i = 0; i < NodeManager.instance.bench.Count; i++)
        {
            if (NodeManager.instance.bench[i].GetUnit() == null)
            {
                GameObject b = Instantiate(allySpawnPrefabRanged, NodeManager.instance.bench[i].transform.position, Quaternion.identity) as GameObject;
                b.GetComponent<Character>().SetNode(NodeManager.instance.bench[i]);
                b.GetComponent<Character>().type = "ally";
                b.GetComponent<Character>().health = 800;
                b.GetComponent<Character>().max_health = 800;
                allies.Add(b.GetComponent<Character>());
                NodeManager.instance.bench[i].SetUnit(b.GetComponent<Character>());
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
            for (int j = 0; j < 8; j++)
            {
                if (NodeManager.instance.board[i][j].GetUnit() != null)
                {
                    if (NodeManager.instance.board[i][j].GetUnit().type == "ally")
                    {
                        RemoveAlly(NodeManager.instance.board[i][j].GetUnit());
                    }
                    else
                    {
                        RemoveEnemy(NodeManager.instance.board[i][j].GetUnit());
                    }
                    Destroy(NodeManager.instance.board[i][j].GetUnit().gameObject);
                    NodeManager.instance.board[i][j].SetUnit(null);
                }
            }
        }
        if (level == 1)
        {
            CreateNewStage1();
        }
        else if (level == 2)
        {
            CreateNewStage2();
        }
        else if (level == 3)
        {
            CreateNewStage3();
        }
        else if (level == 4)
        {
            CreateNewStage4();
        }
        else if (level == 5)
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
        List<Node> targetSlots = new List<Node>();
        targetSlots.Add(NodeManager.instance.board[0][4]);
        targetSlots.Add(NodeManager.instance.board[2][4]);
        targetSlots.Add(NodeManager.instance.board[4][4]);
        targetSlots.Add(NodeManager.instance.board[6][4]);

        for (int i = 0; i < targetSlots.Count; i++)
        {
            GameObject b = Instantiate(enemySpawnPrefab, targetSlots[i].transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Character>().SetNode(targetSlots[i]);
            b.GetComponent<Character>().type = "enemy";
            b.GetComponent<Character>().health = 1000;
            b.GetComponent<Character>().max_health = 1000;
            enemies.Add(b.GetComponent<Character>());
            targetSlots[i].SetUnit(b.GetComponent<Character>());
        }
    }
    void CreateNewStage2()
    {
        List<Node> targetSlots = new List<Node>();
        targetSlots.Add(NodeManager.instance.board[0][7]);
        targetSlots.Add(NodeManager.instance.board[2][7]);
        targetSlots.Add(NodeManager.instance.board[4][7]);
        targetSlots.Add(NodeManager.instance.board[6][7]);

        for (int i = 0; i < targetSlots.Count; i++)
        {
            GameObject b = Instantiate(enemySpawnPrefabRanged, targetSlots[i].transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Character>().SetNode(targetSlots[i]);
            b.GetComponent<Character>().type = "enemy";
            b.GetComponent<Character>().health = 800;
            b.GetComponent<Character>().max_health = 800;
            enemies.Add(b.GetComponent<Character>());
            targetSlots[i].SetUnit(b.GetComponent<Character>());
        }
    }
    void CreateNewStage3()
    {
        List<Node> targetRangedSlots = new List<Node>();
        targetRangedSlots.Add(NodeManager.instance.board[1][7]);
        targetRangedSlots.Add(NodeManager.instance.board[3][7]);
        targetRangedSlots.Add(NodeManager.instance.board[5][7]);
        targetRangedSlots.Add(NodeManager.instance.board[7][7]);

        List<Node> targetSlots = new List<Node>();
        targetSlots.Add(NodeManager.instance.board[0][4]);
        targetSlots.Add(NodeManager.instance.board[2][4]);
        targetSlots.Add(NodeManager.instance.board[4][4]);
        targetSlots.Add(NodeManager.instance.board[6][4]);

        for (int i = 0; i < targetSlots.Count; i++)
        {
            GameObject b = Instantiate(enemySpawnPrefab, targetSlots[i].transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Character>().SetNode(targetSlots[i]);
            b.GetComponent<Character>().type = "enemy";
            b.GetComponent<Character>().health = 1000;
            b.GetComponent<Character>().max_health = 1000;
            enemies.Add(b.GetComponent<Character>());
            targetSlots[i].SetUnit(b.GetComponent<Character>());
        }

        for (int i = 0; i < targetRangedSlots.Count; i++)
        {
            GameObject b = Instantiate(enemySpawnPrefabRanged, targetRangedSlots[i].transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Character>().SetNode(targetRangedSlots[i]);
            b.GetComponent<Character>().type = "enemy";
            b.GetComponent<Character>().health = 800;
            b.GetComponent<Character>().max_health = 800;
            enemies.Add(b.GetComponent<Character>());
            targetRangedSlots[i].SetUnit(b.GetComponent<Character>());
        }
    }
    void CreateNewStage4()
    {
        List<Node> targetRangedSlots = new List<Node>();
        targetRangedSlots.Add(NodeManager.instance.board[3][6]);
        targetRangedSlots.Add(NodeManager.instance.board[4][6]);
        targetRangedSlots.Add(NodeManager.instance.board[5][6]);
        targetRangedSlots.Add(NodeManager.instance.board[6][6]);

        List<Node> targetSlots = new List<Node>();
        targetSlots.Add(NodeManager.instance.board[3][4]);
        targetSlots.Add(NodeManager.instance.board[4][4]);
        targetSlots.Add(NodeManager.instance.board[5][4]);
        targetSlots.Add(NodeManager.instance.board[6][4]);

        for (int i = 0; i < targetSlots.Count; i++)
        {
            GameObject b = Instantiate(enemySpawnPrefab, targetSlots[i].transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Character>().SetNode(targetSlots[i]);
            b.GetComponent<Character>().type = "enemy";
            b.GetComponent<Character>().health = 1000;
            b.GetComponent<Character>().max_health = 1000;
            enemies.Add(b.GetComponent<Character>());
            targetSlots[i].SetUnit(b.GetComponent<Character>());
        }

        for (int i = 0; i < targetRangedSlots.Count; i++)
        {
            GameObject b = Instantiate(enemySpawnPrefabRanged, targetRangedSlots[i].transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Character>().SetNode(targetRangedSlots[i]);
            b.GetComponent<Character>().type = "enemy";
            b.GetComponent<Character>().health = 800;
            b.GetComponent<Character>().max_health = 800;
            enemies.Add(b.GetComponent<Character>());
            targetRangedSlots[i].SetUnit(b.GetComponent<Character>());
        }
    }
    void CreateNewStage5()
    {
        List<Node> targetRangedSlots = new List<Node>();
        targetRangedSlots.Add(NodeManager.instance.board[1][5]);
        targetRangedSlots.Add(NodeManager.instance.board[3][5]);
        targetRangedSlots.Add(NodeManager.instance.board[5][5]);
        targetRangedSlots.Add(NodeManager.instance.board[7][5]);
        targetRangedSlots.Add(NodeManager.instance.board[0][6]);
        targetRangedSlots.Add(NodeManager.instance.board[2][6]);
        targetRangedSlots.Add(NodeManager.instance.board[4][6]);
        targetRangedSlots.Add(NodeManager.instance.board[6][6]);

        List<Node> targetSlots = new List<Node>();
        targetSlots.Add(NodeManager.instance.board[0][4]);
        targetSlots.Add(NodeManager.instance.board[2][4]);
        targetSlots.Add(NodeManager.instance.board[4][4]);
        targetSlots.Add(NodeManager.instance.board[6][4]);

        for (int i = 0; i < targetSlots.Count; i++)
        {
            GameObject b = Instantiate(enemySpawnPrefab, targetSlots[i].transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Character>().SetNode(targetSlots[i]);
            b.GetComponent<Character>().type = "enemy";
            b.GetComponent<Character>().health = 1000;
            b.GetComponent<Character>().max_health = 1000;
            enemies.Add(b.GetComponent<Character>());
            targetSlots[i].SetUnit(b.GetComponent<Character>());
        }

        for (int i = 0; i < targetRangedSlots.Count; i++)
        {
            GameObject b = Instantiate(enemySpawnPrefabRanged, targetRangedSlots[i].transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Character>().SetNode(targetRangedSlots[i]);
            b.GetComponent<Character>().type = "enemy";
            b.GetComponent<Character>().health = 800;
            b.GetComponent<Character>().max_health = 800;
            enemies.Add(b.GetComponent<Character>());
            targetRangedSlots[i].SetUnit(b.GetComponent<Character>());
        }
    }
    void startStage()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (NodeManager.instance.board[i][j].GetUnit() != null)
                {
                    NodeManager.instance.board[i][j].GetUnit().cState = CharacterState.Active;
                }
            }
        }
    }
    public void RemoveAlly(Character a)
    {
        for (int i = 0; i < allies.Count; i++)
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

    public static int GetMoney()
    {
        return money;
    }

}
