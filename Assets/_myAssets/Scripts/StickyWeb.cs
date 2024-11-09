using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyWeb : MonoBehaviour
{
    [SerializeField] private GameObject webQuad;

    void Start()
    {
        int numQuads = Random.Range(50, 150); // this is a TON of transparent quads -> very GPU intensive
        SpawnQuads(numQuads);
    }

    // Spawn and randomly place a bunch of quads with cobweb textures to add a more organic feel to our web encasing.
    private void SpawnQuads(int numQuads)
    {
        for (int i = 0; i < numQuads; i++)
        {
            GameObject quad = Instantiate(webQuad, transform.position, Random.rotation);
            quad.transform.parent = transform;
            Vector3 randPoint = Random.onUnitSphere * 0.5f;
            quad.transform.localPosition = randPoint;
        }
    }
}
