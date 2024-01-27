using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    // Start is called before the first frame update
    void OnCollisionEnter(Collision other)
    {
        Killable killable = other.gameObject.GetComponent<Killable>();
        if(killable!=null){
             Debug.Log("Trap triggered!");
             killable.Die();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
