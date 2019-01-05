using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererHandler : MonoBehaviour {
    private LinkedList<LineRenderer> renderers = new LinkedList<LineRenderer>();
    private GameObject lineRendObject;

    private float lineWidth;
    public float LineWidth
    {
        get { return lineWidth; }
        set
        {
            foreach (var item in renderers)
            {
                item.startWidth = value;
                item.endWidth = value;
            }
            lineWidth = value;
        }
    }

    private Color lineColor;
    public Color LineColor
    {
        get { return lineColor; }
        set
        {
            foreach (LineRenderer item in renderers)
            {
                item.startColor = value;
                item.endColor = value;
            }
            lineColor = value;
        }
    }


    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update() { }
    public LineRendererHandler Init(float lineWidth, Color lineColor, GameObject rendObject)
    {
        LineWidth = lineWidth;
        LineColor = lineColor;
        lineRendObject = rendObject;
        return this;
    }

    private LineRenderer createRenderer()
    {
        LineRenderer line = Instantiate(lineRendObject).AddComponent<LineRenderer>();

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;

        line.startColor = lineColor;
        line.endColor = lineColor;
        return line;
    }

    //previous line is erased
    public void DrawLine(Vector3[] points) //null for erasing all
    {
        LinkedListNode<LineRenderer> node;
        if (points == null || points.Length < 2) //ne risemo nicesar, vsi renderji izklopljeni
        {
            foreach (LineRenderer item in renderers)
            {
                item.enabled = false;
            }
            return;
        }

        node = renderers.First;
        Vector3 startSection = points[0];
        Vector3 endSection = points[1];
        for (int i = 2; i < points.Length; ++i)
        {
            if (points[i - 1] - points[i - 2] == points[i] - points[i - 1]) //ce je usmeritev enaka, potegnemo samo eno crto
            {
                endSection = points[i];
            }
            else
            {
                if (node == null)
                    node = renderers.AddLast(createRenderer());

                node.Value.enabled = true;
                node.Value.positionCount = 2;
                node.Value.SetPositions(new Vector3[] { startSection, endSection });
                node = node.Next;

                startSection = points[i - 1];
                endSection = points[i]; //nova sekcija
            }
        }
        //zadnja crta
        if (node == null)
            node = renderers.AddLast(createRenderer());

        node.Value.enabled = true;
        node.Value.positionCount = 2;
        node.Value.SetPositions(new Vector3[] { startSection, endSection });
        node = node.Next;

        //morebitne preostale renderje izklopimo
        while (node != null)
        {
            node.Value.enabled = false;
            node = node.Next;
        }
    }



}
