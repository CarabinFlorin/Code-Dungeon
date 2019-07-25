using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction : MonoBehaviour
{
    public GameObject Chest, Lava, Player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Random.value > 0.5f)
        {
            Player.GetComponent<Player_Controller>().direction = "direction==up";
            Chest.transform.position = new Vector2(8.5f, 1.5f);
            Lava.transform.position = new Vector2(8.5f, -4.5f);
        }
        else
        {
            Player.GetComponent<Player_Controller>().direction = "direction==down";
            Chest.transform.position = new Vector2(8.5f, -4.5f);
            Lava.transform.position = new Vector2(8.5f, 1.5f);

        }

    }
}
