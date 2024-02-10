using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private bool trapActivated = false;
    void OnCollisionEnter(Collision other)
    {
        Killable killable = other.gameObject.GetComponent<Killable>();
        Walker walker = other.gameObject.GetComponent<Walker>();
        Debug.Log("Trap collided!");
        if(!trapActivated && killable!=null){
             Debug.Log("Trap triggered!");
             killable.Die();
             MovePlayerToCenter(walker);
             trapActivated=true;
        }
    }

    void MovePlayerToCenter(Walker walker)
    {
        // Calculate the center position of the trap (you might need to adjust this based on your trap's position)
        Vector3 trapCenter = transform.position;

        walker.enabled = false;

        // Set the player's position to the center of the trap
        walker.gameObject.transform.position = new Vector3(trapCenter.x, trapCenter.y + 0.5f, trapCenter.z);
    }
}
