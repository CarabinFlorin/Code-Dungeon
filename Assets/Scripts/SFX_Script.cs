using UnityEngine;

public class SFX_Script : MonoBehaviour
{
    public AudioSource Explosion;
    public AudioSource Hurt;
    public AudioSource Impact;
    public AudioSource Sword;

    public GameObject Image;

    public void Instructions()
    {
        if (Image.active)
        {
            Image.SetActive(false);
        }
        else
        {
            Image.SetActive(true);
        }
    }
}
