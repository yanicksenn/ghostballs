using UnityEngine;

public class TestDeath : MonoBehaviour {
    public void MakeVisiblyDead() {
        Deflate();
    }
    public void Deflate() {
        transform.localScale = new Vector3(transform.localScale.x, 0.25f, transform.localScale.z);
    }
    public void Inflate() {
        transform.localScale = new Vector3(transform.localScale.x, 1.0f, transform.localScale.z);
    }
}