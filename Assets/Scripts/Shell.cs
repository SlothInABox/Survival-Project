using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    private Rigidbody shellRb;
    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;

    private float lifetime = 4;
    private float fadetime = 2;

    // Start is called before the first frame update
    void Start()
    {
        shellRb = GetComponent<Rigidbody>();

        float force = Random.Range(minForce, maxForce);
        shellRb.AddForce(transform.right * force);
        shellRb.AddTorque(Random.insideUnitSphere * force);

        StartCoroutine(Fade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Fade()
    {
        yield return new WaitForSeconds(lifetime);

        float percent = 0;
        float fadeSpeed = 1 / fadetime;
        Material mat = GetComponent<Renderer>().material;
        Color initialColor = mat.color;

        while (percent < 1)
        {
            percent += Time.deltaTime * fadeSpeed;
            mat.color = Color.Lerp(initialColor, Color.clear, percent);
            yield return null;
        }

        Destroy(gameObject);
    }
}
