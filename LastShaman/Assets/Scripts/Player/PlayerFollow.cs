using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        //Set the camera position to the player position, keeps player centre frame
        //Might look at tools for this for higher camera control, Kendra Games Tutorial  https://www.youtube.com/watch?v=Z2KmfduirfU&list=PLx7AKmQhxJFaj0IcdjGJzIq5KwrIfB1m9&index=5
        transform.position = player.transform.position;
    }
}
