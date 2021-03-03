using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDlaPlansz : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var pc = GetComponent<PolygonCollider2D>();

        int ind = 1;
        var path = pc.GetPath(ind);
        float x = Camera.main.orthographicSize * Camera.main.aspect;
        for(int i = 0; i<path.Length; i++)
        {
            path[i].x = Mathf.Sign(path[i].x)*x;
        }
        pc.SetPath(ind, path);
    }

}
