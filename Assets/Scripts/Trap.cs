using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private bool trapActivated = false;
    void OnCollisionEnter(Collision other)
    {
        Killable killable = other.gameObject.GetComponent<Killable>();
        GameObject body= other.gameObject;
        Debug.Log("Trap collided!");
        if(!trapActivated && killable!=null){
             Debug.Log("Trap triggered!");
             killable.Die();
             MovePlayerToCenter(body);
             trapActivated=true;
        }
    }

    void MovePlayerToCenter(GameObject player)
    {
        // Calculate the center position of the trap (you might need to adjust this based on your trap's position)
        Vector3 trapCenter = transform.position;

        // Set the player's position to the center of the trap
        player.transform.position = trapCenter;
    }
}
