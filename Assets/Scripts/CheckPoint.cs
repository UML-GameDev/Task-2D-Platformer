using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    //TODO-There should be better way than assigning player on every checkpoint
    public PlayerController player;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("CheckPoint Passed");
            //Activate Checkpoint
            player.SetCheckPoint(transform.position);
        }
    }
}
