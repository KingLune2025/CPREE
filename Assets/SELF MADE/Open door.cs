using DoorScript;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Opendoor : MonoBehaviour
{
    // Start is called before the first frame update

    public Door door;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space key was pressed");
            door.open = !door.open;
        }
        


    }
}
