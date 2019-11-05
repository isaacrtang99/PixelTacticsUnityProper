using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAI : MonoBehaviour
{
    bool isDead = false;
    bool isMoving = false;
    float moveTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveTimer -= Time.deltaTime;
        GameObject owner = this.gameObject;
        GameObject nodeManager = GameObject.Find("NodeManager");
        List<List<GameObject>> board = nodeManager.GetComponent<NodeManager>().board;
        PathFind pathfinder = this.gameObject.GetComponent<PathFind>();

    }
}
