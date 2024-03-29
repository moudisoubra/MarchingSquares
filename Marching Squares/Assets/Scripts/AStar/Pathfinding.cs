﻿using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinding : MonoBehaviour
{
    Grid GridReference;//For referencing the grid class
    public Transform StartPosition;//Starting position to pathfind from
    public Transform TargetPosition;//Starting position to pathfind to

    public Vector3 startVector;
    public Vector3 targetVector;

    public DateTime before;

    public bool walk;
    public bool startThread;
    public bool startNormal;
    public Thread findPathThread;

    private void Awake()//When the program starts
    {
        startVector = StartPosition.position;
        targetVector = TargetPosition.position;

        GridReference = GetComponent<Grid>();//Get a reference to the game manager

    }
    private void OnApplicationQuit()
    {
        findPathThread.Abort();
    }
    private void Update()//Every frame
    {
        startVector = StartPosition.position;
        targetVector = TargetPosition.position;

        if (startThread)
        {
            before = DateTime.Now;
            findPathThread = new Thread(() => FindPath(startVector, targetVector));
            findPathThread.IsBackground = false;
            findPathThread.Start();


            //Debug.Log("Duration in milliseconds MultiThreaded Mode: " + duration.Milliseconds);
            startThread = false;
        }

        if (startNormal)
        {
            DateTime before = DateTime.Now;
            FindPath(startVector, targetVector);

            //Debug.Log("Duration in milliseconds Normal Mode: " + duration.Milliseconds);
            startNormal = false;
        }


        //FindPath(StartPosition.position, TargetPosition.position);//Find a path to the goal
    }

    void FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
    {


        Debug.Log("Thread Started");
        walk = false;
        Node StartNode = GridReference.NodeFromWorldPoint(a_StartPos);//Gets the node closest to the starting position
        Node TargetNode = GridReference.NodeFromWorldPoint(a_TargetPos);//Gets the node closest to the target position

        List<Node> OpenList = new List<Node>();//List of nodes for the open list
        List<Node> ClosedList = new List<Node>();//List of nodes for the closed list

        OpenList.Add(StartNode);//Add the starting node to the open list to begin the program

        while (OpenList.Count > 0)//Whilst there is something in the open list
        {
            Node CurrentNode = OpenList[0];//Create a node and set it to the first item in the open list
            for (int i = 1; i < OpenList.Count; i++)//Loop through the open list starting from the second object
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].hCost < CurrentNode.hCost)//If the f cost of that object is less than or equal to the f cost of the current node
                {
                    CurrentNode = OpenList[i];//Set the current node to that object
                }
            }
            OpenList.Remove(CurrentNode);//Remove that from the open list
            ClosedList.Add(CurrentNode);//And add it to the closed list

            if (CurrentNode == TargetNode)//If the current node is the same as the target node
            {
                GetFinalPath(StartNode, TargetNode);//Calculate the final path
            }

            foreach (Node NeighborNode in GridReference.GetNeighboringNodes(CurrentNode))//Loop through each neighbor of the current node
            {
                if (!NeighborNode.blocked || ClosedList.Contains(NeighborNode))//If the neighbor is a wall or has already been checked
                {
                    continue;//Skip it
                }
                int MoveCost = CurrentNode.gCost + GetManhattenDistance(CurrentNode, NeighborNode);//Get the F cost of that neighbor

                if (MoveCost < NeighborNode.gCost || !OpenList.Contains(NeighborNode))//If the f cost is greater than the g cost or it is not in the open list
                {
                    NeighborNode.gCost = MoveCost;//Set the g cost to the f cost
                    NeighborNode.hCost = GetManhattenDistance(NeighborNode, TargetNode);//Set the h cost
                    NeighborNode.ParentNode = CurrentNode;//Set the parent of the node for retracing steps

                    if (!OpenList.Contains(NeighborNode))//If the neighbor is not in the openlist
                    {
                        OpenList.Add(NeighborNode);//Add it to the list
                    }
                }
            }

        }

        DateTime after = DateTime.Now;

        TimeSpan duration = after.Subtract(before);

        Debug.Log("Duration in milliseconds Normal Mode: " + duration.Milliseconds);

    }



    void GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {
        Debug.Log("Getting final path");
        List<Node> FinalPath = new List<Node>();//List to hold the path sequentially 
        Node CurrentNode = a_EndNode;//Node to store the current node being checked

        while (CurrentNode != a_StartingNode)//While loop to work through each node going through the parents to the beginning of the path
        {
            FinalPath.Add(CurrentNode);//Add that node to the final path
            CurrentNode = CurrentNode.ParentNode;//Move onto its parent node
        }

        FinalPath.Reverse();//Reverse the path to get the correct order

        GridReference.FinalPath = FinalPath;//Set the final path
        walk = true;
    }

    int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.xPosition - a_nodeB.xPosition);//x1-x2
        int iy = Mathf.Abs(a_nodeA.yPosition - a_nodeB.yPosition);//y1-y2

        return ix + iy;//Return the sum
    }

}
