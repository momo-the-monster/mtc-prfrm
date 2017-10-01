using UnityEngine;
using System.Collections;
using DG.Tweening;

public class RoadBuilder : MonoBehaviour {

    public GameObject prefab;
    public GameObject lastObject;
    public Transform container;
    public KeyCode growKey = KeyCode.G;
    static System.Random rnd;
    public int numberToGrowAtStart = 5;

    void Start () {
        rnd = new System.Random();
        GrowMany(numberToGrowAtStart);
	}

    void GrowMany(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GrowOne();
        }
    }

    void GrowOne()
    {
        GameObject g = Instantiate(prefab);

        // Randomize Scale
        Vector3 scale = Vector3.one;
        //  float[] scales = { 0.25f, 0.5f, 0.75f, 1f };
        float[] scales = { 0.025f, 0.125f, 0.1f, 0.25f, 0.3f };
        scale *= scales[rnd.Next(scales.Length)];
    //    scale.z *= Random.Range(1f, 2.5f);
        //     scale.z *= Random.Range(0.5f, 2f);
        g.transform.localScale = Vector3.zero;
        g.transform.DOScale(scale, 0.2f);
        //g.transform.localScale = scale;

        g.transform.SetParent(container, true);

        // Calculate position
        float maxJumpDiff = 0.6f;
        float lastZScale = lastObject.transform.localScale.z;
        Vector3 position = lastObject.transform.position;
        position.z += (lastZScale / 2f) + (scale.z / 2f);
      //  position.y = (1 - scale.y) / 2; // even y heights
        position.y -= Random.Range(0, scale.y / 2);
      //  float[] distances = { 0f, 0.25f, 0.5f };
   //     position.z += distances[rnd.Next(distances.Length)];
        g.transform.position = position;

        lastObject = g;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(growKey))
            GrowOne();
	}
}
