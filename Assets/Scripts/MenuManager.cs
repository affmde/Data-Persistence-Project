using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    private void Awake(){
        if( Instance != null){
            Destroy(gameObject);
            return;
        }

        Instance= this;
        DontDestroyOnLoad(gameObject);
    }
    public string inputText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadNameInput(string name){
        inputText= name;
    }

    
}
