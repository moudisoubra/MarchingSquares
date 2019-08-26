using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Threading : MonoBehaviour
{
    public Vector3 rotation = Vector3.zero;
    public Vector3 pos = Vector3.zero;
    public Vector3 cube1r;
    public Vector3 cube2r;
    public GameObject cube1;
    public GameObject cube2;

    static void Rotate(ref Vector3 vector3)
    {
        while (true)
        {
            vector3 += Vector3.up;
            print("Rotating");
            Thread.Sleep(2);
        }
    }

    void UpDown(ref Vector3 pos)
    {
        bool up = true;

        while (true)
        {
            if (pos.y >= cube1r.y)
            {
                up = false;
            }
            if (pos.y <= cube2r.y)
            {
                up = true;
            }

            if (up)
            {
                pos += Vector3.up;
                print("First if");
            }
            if (!up)
            {
                pos -= Vector3.up;
                print("Second if");
            }
            Thread.Sleep(50);
        }
    }

    Thread thread;
    Thread thread2;
    void Start()
    {
        cube1 = GameObject.Find("Cube (1)");
        cube2 = GameObject.Find("Cube (2)");

        cube1r = cube1.transform.position;
        cube2r = cube2.transform.position;

        thread = new Thread(() => Rotate(ref rotation));
        thread2 = new Thread(() => UpDown(ref pos));

        thread.IsBackground = false;
        thread2.IsBackground = false;

        thread.Start();
        thread2.Start();
    }


    void Update()
    {
        transform.rotation = Quaternion.Euler(rotation);
        transform.position = pos;
    }

    private void OnApplicationQuit()
    {
        thread.Abort();
        thread2.Abort();
    }
}
