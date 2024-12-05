using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    [Range(0,1)] private float smooth;

    private void FixedUpdate() {
        if(player != null){
            Vector3 p = new Vector3(player.transform.position.x, player.transform.position.y, -10);
            transform.position = transform.localPosition = Vector3.Lerp(transform.position, p, smooth);
        }
    }
}
