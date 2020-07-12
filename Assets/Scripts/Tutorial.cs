using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Tutorial : MonoBehaviour
{
    public Animator animator1;
    public Animator animator2; 
    public Animator animator3;
    public Animator final;
    public Animator finalButton1;
    public Animator finalButton2;
    public GameObject graybg;
    public Animator clear;
    Boolean isRight = false;
    Boolean isLeft = false;
    Boolean isWall = false;
    public GameObject wall;
    public GameObject right;
    public GameObject left;
    public GameObject red;
    float speed = 200;
    int round = 0;
    int count = 0;
    int checkAngle = 0;
    Vector3 center = new Vector3(0, 0, 0);
    void Start()
    {
        animator1.SetBool("isAppear", true);
        left.GetComponent<Button>().interactable = false;
        red.SetActive(false);
    }
    void Update()
    {
        if (isRight)
        {            
            if (checkAngle == 0)
            {
                center = gameObject.transform.localPosition + new Vector3(5, 5, 0);
            }
            if (checkAngle < 180)
            {
                gameObject.transform.RotateAround(center, Vector3.forward, -15);
                checkAngle += 15;
            }           
        }
        if (isLeft)
        {
            if (checkAngle == 0)
            {
                center = gameObject.transform.localPosition + new Vector3(-5, -5, 0);
            }
            gameObject.transform.RotateAround(center, Vector3.forward, 15);
            checkAngle += 15;
            if(checkAngle == 180)
            {
                left.GetComponent<Button>().interactable = false; 
                right.GetComponent<Button>().interactable = true;       
            }                
        }
        if(checkAngle == 180)
        {
            checkAngle = 0;
            isRight = false;
            isLeft = false;
        }
        if (isWall)
        {
            wall.transform.localPosition = Vector3.MoveTowards(wall.transform.localPosition, new Vector3(0, 0, 0), speed * Time.deltaTime);
        }
        if (wall.transform.localPosition.z < 115)
        {
            isWall = false;
            wall.transform.localPosition = new Vector3(0, 0, 675);
            clear.Play("Pop", 0, 0f);
            if(round == 0)
            {
                Invoke("AnimatorPlay", 0.5f);
                Invoke("ActiveLeft", 1f);
                gameObject.transform.localPosition = new Vector3(-10,35,115);
                red.SetActive(true);

                //wall change color
                foreach (Renderer child in wall.GetComponentsInChildren<Renderer>(true))
                {
                    child.material.color = new Color(239 / 255.0F, 27 / 255.0F, 57 / 255.0F, 255 / 255.0F);
                }
            }
            if (round == 1)
            {
                Invoke("FinalPlay", 1f);

            }
            

            round++;
        }
        if (gameObject.tag == "Red" && count==1)
        {
            left.GetComponent<Button>().interactable = false;
            right.GetComponent<Button>().interactable = true;

        }
    }

    void AnimatorPlay()
    {
        animator2.SetBool("isAppear", true);
    }
    void FinalPlay()
    {
        graybg.SetActive(true);
        final.SetBool("isAppear", true);
        finalButton1.SetBool("isAppear", true);
        finalButton2.SetBool("isAppear", true);
        animator3.SetBool("isAppear", true);
    }
    void ActiveLeft()
    {
        left.GetComponent<Button>().interactable = true;
    }

    public void RightButton()
    {
        count++;
        isRight = true;
        if (round == 0)
        {
            animator1.SetBool("isAppear", false);
            isWall = true;
            right.GetComponent<Button>().interactable = false;
        }
        if (count >= 3)
        {
            animator2.SetBool("isAppear", false);
            right.GetComponent<Button>().interactable = false;
            isWall = true;
        }
    }
        public void leftButton()
    {
        isLeft = true;
    }

    void OnTriggerEnter(Collider collider)
    {
        var name = collider.name;
        UnityEngine.Debug.Log("Name is " + name);

        if (collider.tag == "Red")
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(239 / 255.0F, 27 / 255.0F, 57 / 255.0F, 255 / 255.0F);
            gameObject.tag = "Red";
        }
        else if (collider.tag == "SpeedDown")
        {
            speed = 100;
            Invoke("ResetSpeed", 0.5f);
        }
    }

    void ResetSpeed()
    {
        speed = 200;
    }
    public void PlayButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void BackButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}
