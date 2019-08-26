using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindVoxels : MonoBehaviour
{
    public bool find;
    public bool change;
    public List<Voxel> voxels;
    public VoxelMap vmScript;
    public VoxelStencil vsScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (find)
        {
            for (int i = 0; i < (vmScript.voxelResolution * vmScript.voxelResolution); i++)
            {
                Voxel voxel =  FindObjectOfType<Voxel>();
                //voxel.state = !voxel.state;
            }

            find = false;
            change = true;
        }

        if (change)
        { 
            for (int i = 0; i < voxels.Count; i++)
            {
                //voxels[i].state = vsScript.stencil.Apply(x, y, voxels[i].state);
            }

            change = false;
        }

    }
}
