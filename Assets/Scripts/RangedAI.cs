using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAI : MonoBehaviour
{
    bool isDead = false;
    bool isMoving = false;
    public float actionTimer = 0.0f;
    Node oldEndNode = null;
    Pathfind pathfinder;
    public List<Character> targets;
    public GameObject mProjectile;
    List<List<GameObject>> board;
    // Use this for initialization
    void Start()
    {
        pathfinder = this.gameObject.GetComponent<Pathfind>();
        targets = null;
    }

    // Update is called once per frame
    void Update()
    {
        string targetType = "";
        actionTimer -= Time.deltaTime;
        GameObject owner = this.gameObject;
        GameObject nodeManager = GameObject.Find("NodeManager");
        GameObject unitManager = GameObject.Find("UnitManager");
        List<List<GameObject>> board = nodeManager.GetComponent<NodeManager>().board;
        if (actionTimer > 0.0f) return;
        if (this.gameObject.GetComponent<Character>().type == "ally")
        {
            targets = unitManager.GetComponent<UnitManager>().enemies;
            targetType = "enemy";
        }
        else if (this.gameObject.GetComponent<Character>().type == "enemy")
        {
            targets = unitManager.GetComponent<UnitManager>().allies;
            targetType = "ally";
        }
        int validTargetCount = 0;
        foreach(Character c in targets)
        {
            if (c.currNode.nType == NodeType.Board) validTargetCount++;
        }
        if (validTargetCount == 0) return;
        if (gameObject.GetComponent<Character>().currNode != null && gameObject.GetComponent<Character>().currNode.nType != NodeType.Bench && gameObject.GetComponent<Character>().cState == CharacterState.Active && targets.Count > 0)
        {
            Node targetAttackNode = null;
            this.pathfinder.mState = MeleeState.Move;
            foreach (Character c in targets)
            {
                Node n = c.currNode;
                if (n.GetUnit() != null)
                {
                    Node myNode = this.gameObject.GetComponent<Character>().currNode;
                    int dist = Mathf.Abs(n.indices.x - myNode.indices.x) + Mathf.Abs(n.indices.y - myNode.indices.y);
                    if (n.GetUnit().type == targetType && dist <= 4)
                    {
                        Debug.Log("Found enemy to attack");
                        pathfinder.mState = MeleeState.Attack;
                        targetAttackNode = n;
                        break;
                    }
                }
            }

            if (this.pathfinder.mState == MeleeState.Move)
            {
                isMoving = true;
                bool hasFoundTargetSpace = false;
                Character targetCharacter = null;
                Node targetNode = null;
                int shortestDist = int.MaxValue;
                int shortestSideDist = int.MaxValue;
                Character mC = gameObject.GetComponent<Character>();
                while (!hasFoundTargetSpace)
                {
                    bool available = false;
                    foreach (Character c in targets)
                    {
                        if (c != null)
                        {
                            available = true;
                            if (Mathf.Abs(c.currNode.indices.x - mC.currNode.indices.x)
                                + Mathf.Abs(c.currNode.indices.y - mC.currNode.indices.y) < shortestDist)
                            {
                                bool hasAdjacent = false;
                                foreach (GameObject g in c.currNode.mAdjacent)
                                {
                                    if (g.GetComponent<Node>().currChar == null) hasAdjacent = true;
                                }
                                if (hasAdjacent)
                                {
                                    shortestDist = Mathf.Abs(c.currNode.indices.x - mC.currNode.indices.x)
                                    + Mathf.Abs(c.currNode.indices.y - mC.currNode.indices.y);
                                    targetCharacter = c;
                                }
                            }
                        }
                    }
                    if (!available) break;
                    foreach (GameObject g in targetCharacter.currNode.mAdjacent)
                    {
                        Node n = g.GetComponent<Node>();
                        if (n.GetUnit() == null)
                        {
                            if (Mathf.Abs(n.indices.x - mC.currNode.indices.x) + Mathf.Abs(n.indices.y - mC.currNode.indices.y) < shortestSideDist)
                            {
                                shortestSideDist = Mathf.Abs(n.indices.x - mC.currNode.indices.x) + Mathf.Abs(n.indices.y - mC.currNode.indices.y);
                                targetNode = n;
                                hasFoundTargetSpace = true;
                            }
                        }
                    }
                }
                this.pathfinder.PathFinder(gameObject.GetComponent<Character>().currNode.gameObject, targetNode.gameObject);
                if (this.pathfinder.mNextNode != null && this.pathfinder.mNextNode.GetComponent<Node>() != null)
                {
                    if (this.pathfinder.mPath.Count > 0)
                    {
                        this.pathfinder.mPrevNode = this.pathfinder.mNextNode;
                        this.pathfinder.mNextNode = this.pathfinder.mPath[pathfinder.mPath.Count - 1];
                        this.pathfinder.mPath.RemoveAt(this.pathfinder.mPath.Count - 1);
                        gameObject.GetComponent<Character>().SetNode(this.pathfinder.mNextNode.GetComponent<Node>());
                        actionTimer = 1.0f;
                    }
                    else
                    {
                        this.pathfinder.PathFinder(gameObject.GetComponent<Character>().currNode.gameObject, oldEndNode.gameObject);
                    }
                }

            }
            else if (this.pathfinder.mState == MeleeState.Attack)
            {
                attackNode(targetAttackNode);
                actionTimer = 1.0f;
            }
        }
    }
    public void attackNode(Node n)
    {
        if (n == null) return;
        if (n.currChar != null)
        {
            GameObject p = Instantiate(mProjectile, this.gameObject.transform.position, Quaternion.identity) as GameObject;
            p.GetComponent<Projectile>().startPosition = this.gameObject.transform.position;
            p.GetComponent<Projectile>().targetNode = n;
            p.GetComponent<Projectile>().targetCharacter = n.currChar;
            if (this.gameObject.GetComponent<Character>().type == "enemy")
            {
                p.GetComponent<Projectile>().color = "green";
            }
        }
    }
}
