using UnityEngine;

public class TestDeath : MonoBehaviour {
    public void MakeVisiblyDead() {
        transform.localScale = new Vector3(transform.localScale.x, 0.25f, transform.localScale.z);
    }
}