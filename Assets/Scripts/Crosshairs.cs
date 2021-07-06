using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshairs : MonoBehaviour
{
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private SpriteRenderer dot;
    [SerializeField] private Color dotHighlightColor;
    private Color originalDotColor;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        originalDotColor = dot.color;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * 40 * Time.deltaTime);
    }

    public void DetectTargets(Ray ray)
    {
        if (Physics.Raycast(ray, 100, targetMask))
        {
            dot.color = dotHighlightColor;
        }
        else
        {
            dot.color = originalDotColor;
        }
    }
}
