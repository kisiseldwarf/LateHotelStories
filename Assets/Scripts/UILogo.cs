using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogo : MonoBehaviour
{
    public GameObject logo;

    // Start is called before the first frame update
    void Start()
    {
        logo.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        logo.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        logo.gameObject.SetActive(false);
    }

}
