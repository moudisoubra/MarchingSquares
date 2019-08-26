using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public int index;
    public float speed;

    public Grid gridScript;
    public Pathfinding pfScript;

    // Start is called before the first frame update
    void Awake()
    {
        index = 0;
        gridScript = FindObjectOfType<Grid>();
        pfScript = FindObjectOfType<Pathfinding>();
    }

    // Update is called once per frame
    void Update()
    {
            if (Vector3.Distance(transform.position, gridScript.FinalPath[index].worldPosition) < 0.5f)
            {
                index++;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, gridScript.FinalPath[index].worldPosition, Time.deltaTime * speed);
            }
    }
}
