using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

[RequireComponent(typeof(MeshFilter))]
public class RoadCubeHold : MonoBehaviour
{
    public Vector3 savedScale;
    public List<Mesh> meshes;
    internal MeshFilter meshFilter;
    static System.Random rnd;

    public delegate void IncarnateDelegate(RoadCubeHold cube);
    public static event IncarnateDelegate OnIncarnate;

    // Use this for initialization
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        rnd = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Incarnate(Vector3 scale)
    {
        IncarnateWithMesh(scale, meshes[rnd.Next(meshes.Count)]);
    }

    public void IncarnateWithMesh(Vector3 scale, Mesh mesh)
    {
        if (OnIncarnate != null)
            OnIncarnate(this);

        if (meshFilter == null)
            meshFilter = GetComponent<MeshFilter>();

        meshFilter.mesh = mesh;
        savedScale = scale;
        transform.localScale = Vector3.zero;
        transform.DOScale(savedScale, 0.25f);
    }

    public void Die()
    {
        transform.DOScale(0f, 0.5f).OnComplete(() => Destroy(this.gameObject));
    }
}
