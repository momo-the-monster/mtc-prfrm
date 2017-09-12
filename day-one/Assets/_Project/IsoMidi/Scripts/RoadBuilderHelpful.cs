using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class RoadBuilderHelpful : MonoBehaviour
{

    public RoadCube prefab;
    public RoadCube lastObject;
    public Transform container;
    public KeyCode growKey = KeyCode.G;
    static System.Random rnd;
    public int numberToGrowAtStart = 5;
    public bool autocull = true;
    public int maxBlocks = 6;
    internal List<RoadCube> blocks;
    public bool doRandomShape = true;
    public List<Mesh> meshes;
    [SerializeField]
    internal Mesh lastMesh;

    void Start()
    {
        rnd = new System.Random();
        lastMesh = GetRandomMesh();
        blocks = new List<RoadCube>();
        GrowMany(numberToGrowAtStart);
    }

    Mesh GetRandomMesh()
    {
        return meshes[rnd.Next(meshes.Count)];
    }

    public void GrowMany(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GrowOne();
        }
    }

    private void Cull(int count)
    {
        for (int i = 0; i < count; i++)
        {
            RoadCube c = blocks[i];
            blocks.Remove(c);
            c.Die();
        }
    }

    public void GrowOne()
    {
        RoadCube c = Instantiate(prefab);

        // Randomize Scale
        Vector3 scale = Vector3.one;
        //  float[] scales = { 0.25f, 0.5f, 0.75f, 1f };
        float[] scales = { 0.0625f, 0.125f, 0.1f, 0.25f};
        scale *= scales[rnd.Next(scales.Length)];

        IncarnateControlled(c, scale);
        c.transform.SetParent(container, true);

        // Calculate position
        float lastZScale = lastObject.savedScale.z;
        float lastYScale = lastObject.savedScale.y;
        Vector3 position = lastObject.transform.position;
        position.z += (lastZScale / 2f) + (scale.z / 2f);
        position.y += (lastYScale / 2f) - (scale.y / 2f);
        position.y -= 0.01f;

        // Set Position
        c.transform.position = position;

        lastObject = c;
        blocks.Add(c);

        if (autocull && blocks.Count > maxBlocks)
            Cull(blocks.Count - maxBlocks);
    }

    public Transform GrowOneWithScale(Vector3 scale)
    {
        RoadCube c = Instantiate(prefab);
        IncarnateControlled(c, scale);
        c.transform.SetParent(container, true);

        // Calculate position
        float lastZScale = lastObject.savedScale.z;
        float lastYScale = lastObject.savedScale.y;
        Vector3 position = lastObject.transform.position;
        position.z += (lastZScale / 2f) + (scale.z / 2f);
        position.y += (lastYScale / 2f) - (scale.y / 2f);
        position.y -= 0.01f;

        // Set Position
        c.transform.position = position;

        lastObject = c;
        blocks.Add(c);

        if (autocull && blocks.Count > maxBlocks)
            Cull(blocks.Count - maxBlocks);

        return c.transform;
    }

    public Transform GrowOneWithScaleAndIndex(Vector3 scale, int index)
    {
        Transform t = GrowOneWithScale(scale);
        var setColor = t.GetComponentsInChildren<SetColorFromPalette>();
        foreach (var item in setColor)
        {
            item.Trigger(index);

        }
        return t;
    }

    internal void IncarnateControlled(RoadCube c, Vector3 scale)
    {
        if (doRandomShape)
        {
            Mesh m = GetRandomMesh();
            c.IncarnateWithMesh(scale, m);
            lastMesh = m;
        }
        else
        {
            c.IncarnateWithMesh(scale, lastMesh);
        }
    }

    public void GrowOneAt(Vector3 position)
    {
        RoadCube c = Instantiate(prefab);

        // Randomize Scale
        Vector3 scale = Vector3.one;
        //  float[] scales = { 0.25f, 0.5f, 0.75f, 1f };
        float[] scales = { 0.0625f, 0.125f, 0.1f, 0.25f, 0.3f, 0.4f };
        scale *= scales[rnd.Next(scales.Length)];

        IncarnateControlled(c, scale);
        c.transform.SetParent(container, true);

        // Set Position
        c.transform.position = position;

        lastObject = c;
        blocks.Add(c);
    }

    public Transform GrowOneAtWithScale(Vector3 position, Vector3 scale)
    {
        // Get Piece
        RoadCube c = Instantiate(prefab);

        // Set Scale
        IncarnateControlled(c, scale);
        c.transform.SetParent(container, true);

        // Set Position
        c.transform.position = position;

        // Update Cache
        lastObject = c;
        blocks.Add(c);

        return c.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(growKey))
            GrowOne();
    }
}
