﻿using UnityEngine;
using System.Collections.Generic;

public abstract class AIBase : MonoBehaviour
{
    public float actionTimer = 0.0f;
    public Node oldEndNode = null;
    public Vector3 prevPos;
    public Vector3 currPos;
    public List<Character> targets;

    protected Character mCharacter;
    protected bool isDead = false;
    protected bool isMoving = false;
    protected Pathfind pathfinder;
    protected string targetType;
    protected Node targetAttackNode;

    // Start is called before the first frame update
    void Awake()
    {
        this.mCharacter = this.gameObject.GetComponent<Character>();
        this.mCharacter.mAI = this;
        this.pathfinder = this.gameObject.GetComponent<Pathfind>();
        this.targets = null;
        this.prevPos = this.transform.position;
        this.currPos = this.transform.position;

        if (this.mCharacter.type == "ally")
        {
            this.targetType = "enemy";
            this.targets = UnitManager.instance.enemies;
        }
        else if (this.mCharacter.type == "enemy")
        {
            this.targetType = "ally";
            this.targets = UnitManager.instance.allies;
        }
    }

    // Update is called once per frame
    void Update()
    {
        actionTimer -= Time.deltaTime;
        GameObject owner = this.gameObject;
        if (actionTimer > 0.0f)
        {
            this.transform.position = Vector3.Lerp(this.prevPos, this.currPos, 1 - actionTimer);
            return;
        }

        int validTargetCount = 0;
        foreach (Character c in targets)
        {
            if (c.currNode.nType == NodeType.Board) validTargetCount++;
        }
        if (validTargetCount == 0) return;

        if (mCharacter.currNode != null && mCharacter.currNode.nType != NodeType.Bench && mCharacter.cState == CharacterState.Active && targets.Count > 0)
        {
            this.actionTimer = 1f;
            this.pathfinder.mState = this.calculateState();
            if (this.pathfinder.mState == MeleeState.Stay) return;
            if (this.pathfinder.mState == MeleeState.Move)
            {
                isMoving = true;
                bool hasFoundTargetSpace = false;
                Character targetCharacter = null;
                Node targetNode = null;
                int shortestDist = int.MaxValue;
                int shortestSideDist = int.MaxValue;
                Character mC = mCharacter;
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
                                foreach (Node n in c.currNode.mAdjacent)
                                {
                                    if (n.currChar == null) hasAdjacent = true;
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
                    foreach (Node n in targetCharacter.currNode.mAdjacent)
                    {
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

                bool lerp = false;
                this.pathfinder.PathFinder(mCharacter.currNode, targetNode);
                if (this.pathfinder.mNextNode != null && this.pathfinder.mNextNode.currChar != null)
                {
                    if (this.pathfinder.mPath.Count > 0)
                    {
                        lerp = true;
                        this.pathfinder.mPrevNode = this.pathfinder.mNextNode;
                        this.pathfinder.mNextNode = this.pathfinder.mPath[pathfinder.mPath.Count - 1];
                        this.prevPos = this.pathfinder.mPrevNode.transform.position;
                        this.currPos = this.pathfinder.mNextNode.transform.position;
                        this.pathfinder.mPath.RemoveAt(this.pathfinder.mPath.Count - 1);
                        mCharacter.SetNode(this.pathfinder.mNextNode);
                    }
                    else
                    {
                        this.pathfinder.PathFinder(mCharacter.currNode, oldEndNode);
                    }
                }

                if (!lerp)
                {
                    this.prevPos = this.currPos;
                }
            }
            else if (this.pathfinder.mState == MeleeState.Attack)
            {
                this.prevPos = this.currPos;
                attackNode(targetAttackNode);
            }
        }
    }

    protected abstract MeleeState calculateState();
    public abstract void attackNode(Node n);
}
