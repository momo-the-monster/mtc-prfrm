using UnityEngine;
using System.Collections.Generic;

public class GeneratePalette : MonoBehaviour {

    public Texture2D inputTexture;
    internal Texture2D outputTexture;
    private Renderer renderer;
    public int width = 64;
    public List<Color> colors;
    public bool generate = false;
    public int hueBuckets = 5;
    private Dictionary<int, Color> bucketColors;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
	}

    public void Generate()
    {
        bucketColors = new Dictionary<int, Color>();
        colors = new List<Color>();

        foreach (var pixelColor in outputTexture.GetPixels())
        {
            // Get Hue & Bucket
            Vector3 hsv = RGBToVec3(pixelColor);
            float hue = hsv.x;
            int bucketIndex = Mathf.RoundToInt(Mathf.Lerp(0, hueBuckets + 1, hue));
        //    Debug.LogFormat("Bucket Index for <color={0}>XXX</color> is {1}, hue is {2}", ColorToHex(pixelColor), bucketIndex, hue);
            // If Bucket is empty, add color. Otherwise, average it
            if (bucketColors.ContainsKey(bucketIndex))
            {
                // Average with existing color
                Color existingColor = bucketColors[bucketIndex];
                Color averageColor = Color.Lerp(existingColor, pixelColor, 0.5f);
                bucketColors[bucketIndex] = averageColor;
            }
            else
            {
                bucketColors[bucketIndex] = pixelColor;
            }
        }

        foreach (var item in bucketColors)
        {
            colors.Add(item.Value);
        }
    }

    void Downsample()
    {
        Color32[] result = ScaleUnityTexture.ScalePoint(inputTexture.GetPixels32(0), inputTexture.width, width, width);
        outputTexture = new Texture2D(width, width);
        outputTexture.filterMode = FilterMode.Point;
        outputTexture.SetPixels32(result);
        outputTexture.Apply();
    }
	
	// Update is called once per frame
	void Update () {
        if (generate)
        {
            generate = false;
            Downsample();
            renderer.material.mainTexture = outputTexture;
            Generate();
        }
	}

    // Note that Color32 and Color implictly convert to each other. You may pass a Color object to this method without first casting it.
    string ColorToHex(Color32 color)
    {
        string hex = "#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    public static Vector3 RGBToVec3(Color rgbColor)
    {
        float h = 0;
        float s = 0;
        float v = 0;
        RGBToHSV(rgbColor, out h, out s, out v);
        return new Vector3(h, s, v);
    }

    public static void RGBToHSV(Color rgbColor, out float H, out float S, out float V)
    {
        if (rgbColor.b > rgbColor.g && rgbColor.b > rgbColor.r)
        {
            RGBToHSVHelper(4f, rgbColor.b, rgbColor.r, rgbColor.g, out H, out S, out V);
        }
        else
        {
            if (rgbColor.g > rgbColor.r)
            {
                RGBToHSVHelper(2f, rgbColor.g, rgbColor.b, rgbColor.r, out H, out S, out V);
            }
            else
            {
                RGBToHSVHelper(0f, rgbColor.r, rgbColor.g, rgbColor.b, out H, out S, out V);
            }
        }
    }

    private static void RGBToHSVHelper(float offset, float dominantcolor, float colorone, float colortwo, out float H, out float S, out float V)
    {
        V = dominantcolor;
        if (V != 0f)
        {
            float num = 0f;
            if (colorone > colortwo)
            {
                num = colortwo;
            }
            else
            {
                num = colorone;
            }
            float num2 = V - num;
            if (num2 != 0f)
            {
                S = num2 / V;
                H = offset + (colorone - colortwo) / num2;
            }
            else
            {
                S = 0f;
                H = offset + (colorone - colortwo);
            }
            H /= 6f;
            if (H < 0f)
            {
                H += 1f;
            }
        }
        else
        {
            S = 0f;
            H = 0f;
        }
    }

}
