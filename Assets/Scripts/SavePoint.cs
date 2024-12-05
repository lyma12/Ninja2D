using System;
using UnityEngine;

public class SavePoint : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            other.GetComponent<Player>().SavePoint();
        }
    }
}