using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAI : MonoBehaviour
{
    [SerializeField]
    GameObject debugIcon;

    bool isDead = false;
    bool isMoving = false;
    public float actionTimer = 0.0f;
    public Node oldEndNode = null;
    public Vector3 prevPos;
    public Vector3 currPos;
    Pathfind pathfinder;
    public List<Character> targets;
    // Start is called before the first frame update
    void Start()
    {
        pathfinder = this.gameObject.GetComponent<Pathfind>();
        targets = null;
        prevPos = this.transform.position;
        currPos = this.transform.position;
        debugIcon.transform.SetParent(null);
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
        if (actionTimer > 0.0f)
        {
            this.transform.position = Vector3.Lerp(this.prevPos, this.currPos, 1 - actionTimer);
            return;
        }
        if (this.gameObject.GetComponent<Character>().type == "ally")
        {
            targets = unitManager.GetComponent<UnitManager>().enemies;
            targetType = "enemy";
        }
        else if(this.gameObject.GetComponent<Character>().type == "enemy")
        {
            targets = unitManager.GetComponent<UnitManager>().allies;
            targetType = "ally";
        }
        int validTargetCount = 0;
        foreach (Character c in targets)
        {
            if (c.currNode.nType == NodeType.Board) validTargetCount++;
        }
        if (validTargetCount == 0) return;
        if (gameObject.GetComponent<Character>().currNode != null && gameObject.GetComponent<Character>().currNode.nType != NodeType.Bench && gameObject.GetComponent<Character>().cState == CharacterState.Active && targets.Count > 0)
        {
            Node targetAttackNode = null;
            this.pathfinder.mState = MeleeState.Move;
            foreach(GameObject g in this.gameObject.GetComponent<Character>().currNode.mAdjacent)
            {
                Node n = g.GetComponent<Node>();
                if (n.GetUnit() != null)
                {
                    if(n.GetUnit().type == targetType)
                    {
                        pathfinder.mState = MeleeState.Attack;
                        targetAttackNode = n;
                        break;
                    }
                }
            }

            if(this.pathfinder.mState == MeleeState.Move)
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
                    foreach(Character c in targets)
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
                    foreach(GameObject g in targetCharacter.currNode.mAdjacent)
                    {
                        Node n = g.GetComponent<Node>();
                        if (n.GetUnit() == null)
                        {
                            if(Mathf.Abs(n.indices.x-mC.currNode.indices.x)+Mathf.Abs(n.indices.y-mC.currNode.indices.y) < shortestSideDist)
                            {
                                shortestSideDist = Mathf.Abs(n.indices.x - mC.currNode.indices.x) + Mathf.Abs(n.indices.y - mC.currNode.indices.y);
                                targetNode = n; 
                                hasFoundTargetSpace = true;
                            }
                        }
                    }
                }
                bool lerp = false;
                //this is where i update the pathfinder :) 
                pathfinder.PathFinder(gameObject.GetComponent<Character>().currNode.gameObject, targetNode.gameObject);

                if (this.pathfinder.mNextNode != null && this.pathfinder.mNextNode.GetComponent<Node>().currChar!=null)
                {
                    //if (debugIcon != null)debugIcon.transform.position = this.pathfinder.mNextNode.GetComponent<Node>().transform.position;
                    if(this.pathfinder.mPath.Count > 0)
                    {
                        lerp = true;
                        this.pathfinder.mPrevNode = this.pathfinder.mNextNode;
                        this.pathfinder.mNextNode = this.pathfinder.mPath[pathfinder.mPath.Count - 1];
                        this.gameObject.GetComponent<Character>().SetNode(pathfinder.mNextNode.GetComponent<Node>());
                        this.prevPos = this.pathfinder.mPrevNode.transform.position;
                        this.currPos = this.pathfinder.mNextNode.transform.position;
                        this.pathfinder.mPath.RemoveAt(this.pathfinder.mPath.Count - 1);
                        actionTimer = 1.0f;
                    }
                }
                else if (this.pathfinder.PathFinder(gameObject.GetComponent<Character>().currNode.gameObject, targetNode.gameObject)) {
                    Debug.Log("Tried to find a new path");
                }
                else
                {
                    Debug.Log("Couldn't find anything to move to, defaulting");
                    Node nextNode = gameObject.GetComponent<Character>().currNode.mAdjacent[0].GetComponent<Node>();
                    gameObject.GetComponent<Character>().SetNode(nextNode);
                    actionTimer = 1.0f;
                }

                if (!lerp)
                {
                    this.prevPos = this.currPos;
                }

            }
            else if(this.pathfinder.mState == MeleeState.Attack)
            {
                this.prevPos = this.currPos;
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
            n.currChar.TakeDamage(100);
        }
    }
}
