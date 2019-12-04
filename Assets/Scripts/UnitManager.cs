using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UnitManager : MonoBehaviour
{
    public static UnitManager instance;

    public List<Character> allies;
    public List<Character> enemies;
    public List<Character> allyCopies;
    public GameObject allySpawnPrefab;
    public GameObject allySpawnPrefabRanged;
    public GameObject allyAssassinPrefab;
    public GameObject enemySpawnPrefab;
    public GameObject enemySpawnPrefabRanged;
    public GameObject enemyAssassinPrefab;
    public List<List<Node>> boardNodes;
    public bool gameStarted;
    float addCooldown = 0.0f;
    float resetCooldown = 0.0f;
    public static int money = 0;
    public static int level = 10;
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
                money += 7;
                level++;
                CreateNewStage(level);
                gameStarted = false;
            }
        }

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
        if (money >= 1) AddAlly();

    }

    public void AddRangedButton()
    {
        if (money >= 2) AddAllyRanged();
    }

    public void AddAssassinButton()
    {
        if (money >= 3) AddAllyAssassin();
    }

    void AddAlly()
    {
        for (int i = 0; i < NodeManager.instance.bench.Count; i++)
        {
            if (NodeManager.instance.bench[i].GetUnit() == null)
            {
                GameObject b = Instantiate(allySpawnPrefab, NodeManager.instance.bench[i].transform.position, Quaternion.identity) as GameObject;
                b.GetComponent<Character>().SetNode(NodeManager.instance.bench[i]);
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
                allies.Add(b.GetComponent<Character>());
                NodeManager.instance.bench[i].SetUnit(b.GetComponent<Character>());
                money-= 2;
                break;
            }
        }
    }

    void AddAllyAssassin()
    {
        for (int i = 0; i < NodeManager.instance.bench.Count; i++)
        {
            if (NodeManager.instance.bench[i].GetUnit() == null)
            {
                GameObject b = Instantiate(allyAssassinPrefab, NodeManager.instance.bench[i].transform.position, Quaternion.identity) as GameObject;
                b.GetComponent<Character>().SetNode(NodeManager.instance.bench[i]);
                allies.Add(b.GetComponent<Character>());
                NodeManager.instance.bench[i].SetUnit(b.GetComponent<Character>());
                money-= 3;
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
        Debug.Log(level);
        int difficulty = 5 + (int)(level * 1.5);
        int moreDifficulty = difficulty;
        while(difficulty > 0 && enemies.Count < 32)
        {
            Debug.Log(difficulty);
            if (difficulty == 1)
            {
                SpawnSkel();
                difficulty -= 1;
            }
            else if (difficulty == 2)
            {
                SpawnGoomba();
                difficulty -= 2;
            }
            else
            {
                int dif = Random.Range(1, 3);
                if (dif == 1)
                {
                    SpawnSkel();
                }
                else if(dif == 2)
                {
                    SpawnGoomba();
                }
                difficulty -= dif;
            }
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
    public void SpawnSkel()
    {
        for(int i = 4; i < 8;i++)
        {
            int j = i;
            if (j < 7)
            {
                j = Random.Range(j, j + 2);
                if (GetAvailable(j).Count == 0) j = i;
            }
            List<int> available = GetAvailable(j);
            if (available.Count == 0) continue;
            int row = Random.Range(0,available.Count);
            row = available[row];
            GameObject b = Instantiate(enemySpawnPrefab, NodeManager.instance.board[row][j].transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Character>().SetNode(NodeManager.instance.board[row][j]);
            enemies.Add(b.GetComponent<Character>());
            NodeManager.instance.board[row][j].SetUnit(b.GetComponent<Character>());
            break;
        }
    }
    public void SpawnGoomba()
    {
        for (int i = 7; i >= 4; i--)
        {
            int j = i;
            if(j > 4)
            {
                j = Random.Range(j - 1, j + 1);
                if (GetAvailable(j).Count == 0) j = i;
            }
            List<int> available = GetAvailable(j);
            if (available.Count == 0) continue;
            int row = Random.Range(0, available.Count);
            row = available[row];
            GameObject b = Instantiate(enemySpawnPrefabRanged, NodeManager.instance.board[row][j].transform.position, Quaternion.identity) as GameObject;
            b.GetComponent<Character>().SetNode(NodeManager.instance.board[row][j]);
            enemies.Add(b.GetComponent<Character>());
            NodeManager.instance.board[row][j].SetUnit(b.GetComponent<Character>());
            break;
        }
    }
    public List<int> GetAvailable(int col)
    {
        List<int> available = new List<int>();
        for(int i = 0; i < 8; i++)
        {
            if(NodeManager.instance.board[i][col].GetUnit() == null)
            {
                available.Add(i);
            }
        }
        return available;
    }
}
