using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SnapshotMode : MonoBehaviour
{

    private Shader noneShader;
    private Shader neonShader;
    private Shader bloomShader;

    public GameObject sky;
    public GameObject ground;

    private List<SnapshotFilter> filters = new List<SnapshotFilter>();

    private int filterIndex = 0;
    private Boolean isClick = true;

    private void Awake()
    {
        // Find all shader files.
        //outlineShader = Shader.Find("Snapshot/EdgeDetect");
        noneShader = Shader.Find("Snapshot/Base");
        neonShader = Shader.Find("Snapshot/Neon");
        bloomShader = Shader.Find("Snapshot/Bloom");

        // Create all filters.
        //filters.Add(new BaseFilter("Outlines", Color.white, outlineShader));
        filters.Add(new BaseFilter("None", Color.white, noneShader));
        filters.Add(new NeonFilter("Neon", Color.cyan, bloomShader, 
            new BaseFilter("", Color.white, neonShader)));

        

    }

    private void Update()
    {
/*        if (Input.GetMouseButtonDown(0))
        {
            if (--filterIndex < 0)
            {
                filterIndex = filters.Count - 1;
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (++filterIndex >= filters.Count)
            {
                filterIndex = 0;
            }
        }*/

    }

    public void BGChanger()
    {
        if (--filterIndex < 0)
        {
            filterIndex = filters.Count - 1;
        }

        sky.SetActive(!isClick);
        ground.SetActive(!isClick);
        isClick = !isClick;
    }

    // Delegate OnRenderImage() to a SnapshotFilter object.
    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        filters[filterIndex].OnRenderImage(src, dst);
    }
}
