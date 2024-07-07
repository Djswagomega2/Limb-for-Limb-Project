using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extensiontest : MonoBehaviour
{
//Plan to have this be a base for the fighters, and have limbs and weapons come in with enumators, functions, inferfaces,etc.
    public BoxCollider2D box;
    public Vector2 size = new Vector2(1, 1);
    public float eTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(ExtendtheCollider());
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            StopCoroutine(ExtendtheCollider());
        }
    }

    public IEnumerator ExtendtheCollider()
    {
        box.size = new Vector2(size.x+=5, 1);
        yield return new WaitForSeconds(eTime);
        box.size = new Vector2(size.x+=5, 1);
        yield return new WaitForSeconds(eTime);
        box.size = new Vector2(size.x+=5, 1);
        yield return new WaitForSeconds(eTime);
        box.size = new Vector2(size.x-=5, 1);
        yield return new WaitForSeconds(eTime);
        box.size = new Vector2(size.x-=5, 1);
        yield return new WaitForSeconds(eTime);
        box.size = new Vector2(size.x-=5, 1);
        yield return new WaitForSeconds(eTime);


        
    }
}
