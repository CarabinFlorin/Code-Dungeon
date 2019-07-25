using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour
{
    //Movement
    public float Speed = 2f;
    Vector2 endPos;

    //Array char
    public GameObject Table;
    char[] move = new char[1];
    int i = 0, y, nrLoops;
    bool once = true, compile = false;
    public bool attack_bool = false, stop = false, elseIF = false;

    //direction string
    public string direction;

    //Save color
    Color color_save;
    //Warning UI
    public GameObject StuckUI, For, ForParanthesis , If, IfParanthesis, Else, ElseParanthesis;
    //Create Animator
    private Animator anim;
    //Sound
    private SFX_Script SFX;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize endPos
        endPos = transform.position;
    }

    void Awake()
    {
        //Set the player on starting position
        transform.position = GameObject.Find("Start").transform.position;
        //Set Animator
        anim = GetComponent<Animator>();
        //Set SFX
        SFX = FindObjectOfType<SFX_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        if (compile) {
            //Decides which direction the player will move in
            if (once)
            {
                //Starts coroutine to check if the player is  stuck

                switch (move[i]) {

                    case '0':
                        //Set the point where the player needs to go
                        endPos.y = transform.position.y + short.Parse(Table.transform.GetChild(i).GetChild(1).GetComponent<InputField>().text); //add the number in the Input Field do the position
                        //Change color
                        changeColor();
                        //Animation float
                        SetAnimation(0, 1);
                        break;
                    case '1':
                        endPos.x = transform.position.x + short.Parse(Table.transform.GetChild(i).GetChild(1).GetComponent<InputField>().text);
                        changeColor();
                        SetAnimation(1, 0);
                        break;
                    case '2':
                        endPos.y = transform.position.y - short.Parse(Table.transform.GetChild(i).GetChild(1).GetComponent<InputField>().text);
                        changeColor();
                        SetAnimation(0, -1);
                        break;
                    case '3':
                        endPos.x = transform.position.x - short.Parse(Table.transform.GetChild(i).GetChild(1).GetComponent<InputField>().text);
                        changeColor();
                        SetAnimation(-1, 0);
                        break;

                    //Attack Function
                    case '4':
                        changeColor();
                        StartCoroutine(AttackCoroutine());
                        break;

                    //for
                    case '5':
                        changeColor();
                        y = i;
                        nrLoops = short.Parse(Table.transform.GetChild(i).GetChild(1).GetComponent<InputField>().text);
                        break;
                    case '6':
                        changeColor();
                        if(nrLoops > 1){
                            i = y;
                        }
                        nrLoops--;
                        break;

                    //if
                    case '7':
                        changeColor();
                        if(Table.transform.GetChild(i).GetChild(1).GetComponent<InputField>().text == direction) {
                            elseIF = true;
                        }
                        else {
                            for(int z = i+1; z < Table.transform.childCount; z++)
                                if (move[z] == '8') i = z;
                        }
                        break;
                    case '8':
                        if(!elseIF) changeColor();
                        if (elseIF) {
                            for (int z = i + 1; z < Table.transform.childCount; z++)
                                if (move[z] == '9') i = z;
                        }
                        break;
                    case '9':
                        changeColor();
                        break;
                }
                once = false;
            }

            //Stops the player if he is attacking
            if (!stop)
            {
                //Move the player
                transform.position = Vector2.MoveTowards(transform.position, endPos, Speed * Time.deltaTime);

                //Check if the player has arrived
                if (Vector2.Distance(transform.position, endPos) < .01f && !attack_bool)
                {
                    //Check if the array is over
                    if (i == move.Length - 1)
                    {
                        Stop();
                    }
                    else
                    {
                        //Change color back
                        Table.transform.GetChild(i).GetComponent<Image>().color = color_save;
                        //Move to the next element and start again
                        i++;
                        once = true;
                    }
                }
            }
        }
    }
    
    //Coroutine for the attack animation
    private IEnumerator AttackCoroutine() {
        SFX.Sword.Play();
        attack_bool = true;
        anim.SetBool("Attack_animation", true);
        yield return new WaitForSeconds(.25f);
        anim.SetBool("Attack_animation", false);
        attack_bool = false;
    }

    //Create array
    public void Compile() {
        //Reset if the player is moving
        if (compile) Stop();
        //Set values
        move = new char[Table.transform.childCount];
        for (int i = 0; i < Table.transform.childCount; i++)
            move[i] = Table.transform.GetChild(i).name[0];

        //checked for Errors
            //Check for FOR
            bool checkFor = true, foundFor = false, loneParanthesis = true;
            for (int i = 0; i < move.Length; i++) {
                if (move[i] == '5') {
                    foundFor = true;
                    for (int x = i + 1; i < move.Length; i++) {
                        if (move[i] == '6') {
                            checkFor = true;
                            break;
                        }
                        else {
                            checkFor = false;
                        }
                    }
                    if (!checkFor) {
                        For.SetActive(true);
                    }
                }
                else if (move[i] == '6' && !foundFor)
                {
                   ForParanthesis.SetActive(true);
                   loneParanthesis = false;
                 }
            }

            //Check for IF
            bool checkIF = true, foundIF = false, loneParanthesis2 = true;
            for (int i = 0; i < move.Length; i++)
            {
                if (move[i] == '7')
                {
                    foundIF = true;
                    for (int x = i + 1; i < move.Length; i++)
                    {
                        if (move[i] == '9')
                        {
                            checkIF = true;
                            break;
                        }
                        else
                        {
                            checkIF = false;
                        }
                    }
                    if (!checkIF)
                    {
                        If.SetActive(true);
                    }
                }
                else if (move[i] == '9' && !foundIF) {
                    Debug.Log("closed paranthesis needs if statement");
                    IfParanthesis.SetActive(true);
                    loneParanthesis2 = false;
                }
            }


            //Check for else to have IF
            bool found_IF = false, loneParanthesis4 = true;
            for (int i = 0; i < move.Length; i++)
            {
                if (move[i] == '7')
                {
                    found_IF = true;
                }
                else if (move[i] == '8' && !found_IF)
                {
                    Debug.Log("else needs if statement");
                    Else.SetActive(true);
                    loneParanthesis4 = false;
                }
            }

            //Check for else paranthesis and if statement
            bool checkElse = true;
            for (int i = 0; i < move.Length; i++)
            {
                if (move[i] == '8')
                {
                    for (int x = i + 1; i < move.Length; i++)
                    {
                        if (move[i] == '9')
                        {
                            checkElse = true;
                            break;
                        }
                        else
                        {
                            checkElse = false;
                        }
                    }
                    if (!checkElse)
                    {
                        ElseParanthesis.SetActive(true);
                    }
                }
            }

        //Stop compiler if there are errors
        if (checkFor && loneParanthesis && checkIF && loneParanthesis2 && checkElse && loneParanthesis4) {
            compile = true;
            once = true;
        }
    }

    //Set animation floats
    private void SetAnimation(int x, int y) {
        anim.SetFloat("MoveX", x);
        anim.SetFloat("MoveY", y);
    }

    //Reset the player
    public void Stop() {

        //Change color back
        Table.transform.GetChild(i).GetComponent<Image>().color = color_save;

        //Move the player back to start and and move to the beggining of the array
        transform.position = GameObject.Find("Start").transform.position;
        i = 0;
        compile = false;
        stop = false;
        elseIF = false;
        endPos = transform.position;

        //Reset the animaion
        SetAnimation(0, 0);
        //Reset warning message
        For.SetActive(false);
        ForParanthesis.SetActive(false);
        If.SetActive(false);
        IfParanthesis.SetActive(false);
        Else.SetActive(false);
        ElseParanthesis.SetActive(false);
    }

    //Change color
    private void changeColor() {
        color_save = Table.transform.GetChild(i).GetComponent<Image>().color;
        Table.transform.GetChild(i).GetComponent<Image>().color = new Color(255, 255, 0);
    }

    //Play the hit animation and reset the player
    public IEnumerator Death() {
        anim.SetBool("Hit_animation", true);
        yield return new WaitForSeconds(.05f);
        anim.SetBool("Hit_animation", false);
        yield return new WaitForSeconds(.3f);
        Stop();
    }

}