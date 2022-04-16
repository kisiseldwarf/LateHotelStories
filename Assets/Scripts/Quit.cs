using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(quit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void quit() {
        Debug.Log("clicked !");
        Application.Quit();

    }
}
