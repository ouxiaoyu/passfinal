using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text;

public class MovableCube : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text bestText;
    GameObject[] shapeArray = new GameObject[25];
    GameObject[] fixedCubeArray = new GameObject[25];

    Boolean turnRight = false;
    Boolean turnLeft = false;
    Vector3 rotateCenter;
    Vector3[] offset = new Vector3[4] { 
        new Vector3(-5, 5, 0), 
        new Vector3(5, 5, 0),
        new Vector3(-5, -5, 0), 
        new Vector3(5, -5, 0)};
    Vector3[] cubeOffset = new Vector3[8] { 
        new Vector3(-10, 10, 0), 
        new Vector3(0, 10, 0),
        new Vector3(10, 10, 0),
        new Vector3(-10, 0, 0),
        new Vector3(10, 0, 0),
        new Vector3(-10, -10, 0),
        new Vector3(0, -10, 0),
        new Vector3(10, -10, 0)};
    Boolean[] checkExist;
    int rotateAngle;
    int checkAngle = 0;
    public Button leftButton;
    public Button rightButton;
    int score = 0;
    Boolean isUpdate = true;

    int[] updateArray = new int[25];
    static int[] previousArray = new int[25];
    static Boolean isFailed = false;
    int wallColor;
    int num_color = 2;

    Color[] colorArray = new Color[] {
                            Color.red,
                            new Color(255 / 255.0F, 162 / 255.0F, 32 / 255.0F, 255 / 255.0F),
                            Color.green,
                            new Color(0  / 255.0F, 233 / 255.0F, 190 / 255.0F, 255 / 255.0F),
                            new Color(28 / 255.0F, 145 / 255.0F, 239 / 255.0F, 255 / 255.0F),
                            new Color(169 / 255.0F, 78 / 255.0F, 255 / 255.0F, 255 / 255.0F),
                            new Color(255 / 255.0F, 32 / 255.0F, 215 / 255.0F, 255 / 255.0F),
    };

    void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
            bestText.text = "Best: " + LoadFile(Application.persistentDataPath, "Best.txt");
#endif

#if UNITY_STANDALONE_WIN
            bestText.text = "Best: " + File.ReadAllText("Assets//Resources//Best.txt");
#endif

//            bestText.text = "Best: " + LoadFile(Application.persistentDataPath, "Best.txt");
//           bestText.text = "Best: " + File.ReadAllText("Assets//Resources//Best.txt");
         
    }

    String LoadFile(string path, string name)
    {
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path + "//" + name);
        }
        catch (Exception e)
        {
            return "0";
        }
        string info;
        info = sr.ReadToEnd();
        sr.Close();
        sr.Dispose();
        return info;
    }

    void WriteFile(string path, string name,String info)
    {
        StreamWriter sw = new StreamWriter(path + "//" + name, false);
        sw.Write(info);
        sw.Close();
        sw.Dispose();
    }


    void FixedUpdate()
    {
        UpdateShape();
        if (ColorChanger.isUpdate)
        {
            Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.tag = "MovableCube";
        }
        else
        {
            if (gameObject.GetComponent<Rigidbody>()== null)
            {
                gameObject.AddComponent<Rigidbody>();
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().angularDrag = 0;
            }
            
        }

        float interval = Time.fixedDeltaTime;
        //UnityEngine.Debug.Log(Time.fixedDeltaTime);
        if (turnRight)
        {
            gameObject.transform.RotateAround(rotateCenter, Vector3.forward, -10 * 50 * interval );
            checkAngle+=10;
        }
        else if (turnLeft)
        {

            gameObject.transform.RotateAround(rotateCenter, Vector3.forward, 10 * 50 * interval );
            checkAngle+=10;
        }
        if (checkAngle == rotateAngle)
        {
            turnRight = false;
            turnLeft = false;
            checkAngle = 0;
            leftButton.interactable = true;
            rightButton.interactable = true;
        }
    }

    /*        void Update()
        {
            float interval = Time.deltaTime;

            if (turnRight)
            {

                gameObject.transform.RotateAround(rotateCenter, Vector3.forward, -1 * interval);
                checkAngle++;
            }
            else if (turnLeft)
            {

                gameObject.transform.RotateAround(rotateCenter, Vector3.forward, 1 * interval);
                checkAngle++;
            }
            if (checkAngle == rotateAngle)
            {
                turnRight = false;
                turnLeft = false;
                checkAngle = 0;
                leftButton.interactable = true;
                rightButton.interactable = true;
            }

        }*/
    void OnTriggerEnter(Collider collider)
    {
        var name = collider.name;
        UnityEngine.Debug.Log("Name is " + name);
        
        if (collider.tag == "Finish" || (collider.tag == "Clear" && GameObject.Find("Shape").tag != gameObject.tag))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            isFailed = true;

            #if UNITY_ANDROID || UNITY_IOS
                if (score > int.Parse(LoadFile(Application.persistentDataPath, "Best.txt")))
                    WriteFile(Application.persistentDataPath, "Best.txt", score.ToString());
            #endif

            #if UNITY_STANDALONE_WIN
                if (score > int.Parse(File.ReadAllText("Assets//Resources//Best.txt")))
                    File.WriteAllText("Assets//Resources//Best.txt", score.ToString(), Encoding.Default);
                bestText.text = "Best: " + File.ReadAllText("Assets//Resources//Best.txt");
            #endif
        }
        else if (collider.tag == "Clear" && GameObject.Find("Shape").tag == gameObject.tag)
        {
            GameObject.Find("Wall").transform.localPosition = new Vector3(0, 0, 442);
            AddScoreText();
            isUpdate = true;
        }
        else if (collider.tag == "Red")
        {

            gameObject.GetComponent<Renderer>().material.color = colorArray[0];
            gameObject.tag = "Red";
        }
        else if (collider.tag == "Orange")
        {
            gameObject.GetComponent<Renderer>().material.color = colorArray[1];
            gameObject.tag = "Orange";

        }
        else if (collider.tag == "Green")
        {
            gameObject.GetComponent<Renderer>().material.color = colorArray[2];
            gameObject.tag = "Green";

        }
        else if (collider.tag == "LightBlue")
        {
            gameObject.GetComponent<Renderer>().material.color = colorArray[3];
            gameObject.tag = "LightBlue";
        }
        else if (collider.tag == "Blue")
        {
            gameObject.GetComponent<Renderer>().material.color = colorArray[4];
            gameObject.tag = "Blue";
        }
        else if (collider.tag == "Purple")
        {
            gameObject.GetComponent<Renderer>().material.color = colorArray[5];
            gameObject.tag = "Purple";
        }
        else if (collider.tag == "Pink")
        {
            gameObject.GetComponent<Renderer>().material.color = colorArray[6];
            gameObject.tag = "Pink";
        }

    }

    Boolean[] GetNear()
    { 
        Boolean[] c = new Boolean[cubeOffset.Length];
        for (int i = 0; i < cubeOffset.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(gameObject.transform.position + cubeOffset[i] + new Vector3(0, 0, -10), gameObject.transform.position + cubeOffset[i], out hit, 10))
            {
                c[i] = true;
            }
            else
            {
                c[i] = false;
            }
        }

        if (gameObject.transform.position.y <= 14)
        {
            c[5] = true;
            c[6] = true;
            c[7] = true;
        }

        return c;
    }

    int GetCenter(int[] a)
    {
        if (a[0] != 0)
        {
            return 0;
        }
        if (a[1] != 0)
        {
            return 1;
        }
        if (a[2] != 0)
        {
            return 2;
        }
        if (a[3] != 0)
        {
            return 3;
        }
        return -1;
    }

    int[] GetAngle(Boolean isRight)
    {
        int[] a = new int[4];

        if (isRight)
        {
            if (checkExist[1] && !checkExist[0] && !checkExist[3])
            {
                a[0] = 180;
            }
            else if (checkExist[1] && checkExist[0] && !checkExist[3])
            {
                a[0] = 90;
            }
            else
            {
                a[0] = 0;
            }

            if (checkExist[4] && !checkExist[2] && !checkExist[1])
            {
                a[1] = 180;
            }
            else if (checkExist[4] && checkExist[2] && !checkExist[1])
            {
                a[1] = 90;
            }
            else
            {
                a[1] = 0;
            }

            if (checkExist[3] && !checkExist[5] && !checkExist[6])
            {
                a[2] = 180;
            }
            else if (checkExist[3] && checkExist[5] && !checkExist[6])
            {
                a[2] = 90;
            }
            else
            {
                a[2] = 0;
            }

            if (checkExist[6] && !checkExist[7] && !checkExist[4])
            {
                a[3] = 180;
            }
            else if (checkExist[6] && checkExist[7] && !checkExist[4])
            {
                a[3] = 90;
            }
            else
            {
                a[3] = 0;
            }

        }
        else if (!isRight)
        {
            if (checkExist[3] && !checkExist[0] && !checkExist[1])
            {
                a[0] = 180;
            }
            else if (checkExist[3] && checkExist[0] && !checkExist[1])
            {
                a[0] = 90;
            }
            else
            {
                a[0] = 0;
            }

            if (checkExist[1] && !checkExist[2] && !checkExist[4])
            {
                a[1] = 180;
            }
            else if (checkExist[1] && checkExist[2] && !checkExist[4])
            {
                a[1] = 90;
            }
            else
            {
                a[1] = 0;
            }

            if (checkExist[6] && !checkExist[5] && !checkExist[3])
            {
                a[2] = 180;
            }
            else if (checkExist[6] && checkExist[5] && !checkExist[3])
            {
                a[2] = 90;
            }
            else
            {
                a[2] = 0;
            }

            if (checkExist[4] && !checkExist[7] && !checkExist[6])
            {
                a[3] = 180;
            }
            else if (checkExist[4] && checkExist[7] && !checkExist[6])
            {
                a[3] = 90;
            }
            else
            {
                a[3] = 0;
            }

        }
        else
        {
            a[0] = 0;
            a[1] = 0;
            a[2] = 0;
            a[3] = 0;
        }
        return a;
    }

    public void RotateRight() 
    {        
        if (turnLeft)
            return;
        leftButton.interactable = false;
        rightButton.interactable = false;
        checkExist = GetNear();
        rotateCenter = gameObject.transform.position + offset[GetCenter(GetAngle(true))];
        rotateAngle = GetAngle(true)[GetCenter(GetAngle(true))];

        if (GetCenter(GetAngle(true))==-1 || Math.Abs(GetNextXPoition(GetCenter(GetAngle(true)), rotateAngle, true))> 25 || GetNextYPoition(GetCenter(GetAngle(true)), rotateAngle, true) < 10 || GetNextYPoition(GetCenter(GetAngle(true)), rotateAngle, true) > 60 )
        {
            leftButton.interactable = true;
            rightButton.interactable = true;
            return;
        } 
        turnRight = true;
        
    }

    public void RotateLeft()
    {
        if (turnRight)
            return;
        leftButton.interactable = false;
        rightButton.interactable = false;
        checkExist = GetNear();
        
        rotateCenter = gameObject.transform.position + offset[GetCenter(GetAngle(false))];
        rotateAngle = GetAngle(false)[GetCenter(GetAngle(false))];

        if (GetCenter(GetAngle(false)) == -1 || Math.Abs(GetNextXPoition(GetCenter(GetAngle(false)), rotateAngle, false)) > 25 || GetNextYPoition(GetCenter(GetAngle(false)), rotateAngle, false) < 10 || GetNextYPoition(GetCenter(GetAngle(false)), rotateAngle, false) > 60 )
        {
            leftButton.interactable = true;
            rightButton.interactable = true;
            return;
        }
        turnLeft = true;
    }

    float GetNextXPoition(int center,int angle,Boolean isRight)
    {
        if (isRight) 
        {
            switch (center)
            {
                case 0: if (angle == 0) return gameObject.transform.position.x; else return gameObject.transform.position.x - 10; break;
                case 1: if (angle == 180) return gameObject.transform.position.x + 10; else return gameObject.transform.position.x; break;
                case 2: if (angle == 180) return gameObject.transform.position.x - 10; else return gameObject.transform.position.x; break;
                case 3: if (angle == 0) return gameObject.transform.position.x; else return gameObject.transform.position.x + 10; break;
                default: return 0; break;
            }
        }
        else
        {
            switch (center)
            {
                case 0: if (angle == 180) return gameObject.transform.position.x-10; else return gameObject.transform.position.x; break;
                case 1: if (angle == 0) return gameObject.transform.position.x; else return gameObject.transform.position.x + 10; break;
                case 2: if (angle == 0) return gameObject.transform.position.x; else return gameObject.transform.position.x - 10; break;
                case 3: if (angle == 180) return gameObject.transform.position.x + 10; else return gameObject.transform.position.x; break;
                default: return 0; break;
            }
        }
    }

    float GetNextYPoition(int center, int angle, Boolean isRight)
    {
        if (isRight)
        {
            switch (center)
            {
                case 0: if (angle == 180) return gameObject.transform.position.y + 10; else return gameObject.transform.position.y ; break;
                case 1: if (angle == 0) return gameObject.transform.position.y; else return gameObject.transform.position.y + 10; break;
                case 2: if (angle == 0) return gameObject.transform.position.y; else return gameObject.transform.position.y - 10; break;
                case 3: if (angle == 180) return gameObject.transform.position.y - 10; else return gameObject.transform.position.y; break;
                default: return 0; break;
            }
        }
        else
        {
            switch (center)
            {
                case 0: if (angle == 0) return gameObject.transform.position.y; else return gameObject.transform.position.y + 10; break;
                case 1: if (angle == 180) return gameObject.transform.position.y + 10; else return gameObject.transform.position.y; break;
                case 2: if (angle == 180) return gameObject.transform.position.y - 10; else return gameObject.transform.position.y; break;
                case 3: if (angle == 0) return gameObject.transform.position.y; else return gameObject.transform.position.y - 10; break;
                default: return 0; break;
            }
        }
    }

    void AddScoreText()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();
    }

    void UpdateShape()
    {
        if (isUpdate)
        {
            //wallColor = UnityEngine.Random.Range(0, 7);
            wallColor = 0;
            if (isFailed && previousArray!=null) 
            {
                Array.Copy(previousArray, updateArray, previousArray.Length);
                isFailed = false;
                UnityEngine.Debug.Log("isFail");
            }
            else
            {
                //Array.Copy(CreateShape(num_color, wallColor), updateArray, updateArray.Length);
                Array.Copy(CreateShape(), updateArray, updateArray.Length);

            }
            //fixedcube
            FixedCube.array = updateArray;
            FixedCube.isUpdate = true;
            //shape
            Shape.array = updateArray;
            Shape.isUpdate = true;
            Shape.c = colorArray[wallColor];
            switch (wallColor)
            {
                case 0: Shape.name = "Red"; break;
                case 1: Shape.name = "Orange"; break;
                case 2: Shape.name = "Green"; break;
                case 3: Shape.name = "LightBlue"; break;
                case 4: Shape.name = "Blue"; break;
                case 5: Shape.name = "Purple"; break;
                case 6: Shape.name = "Pink"; break;
                default: Shape.name = "Red"; break;
            }
            //color
            ColorChanger.array = updateArray;
            ColorChanger.isUpdate = true;
            //movable cube
            SetMovableCube(updateArray);
            gameObject.GetComponent<Renderer>().material.color = new Color(0 / 255.0F, 0 / 255.0F, 0 / 255.0F, 255 / 255.0F);

            isUpdate = false;
            Array.Copy(updateArray,previousArray, updateArray.Length); 
            
        }
    }

    public void SetMovableCube(int[] array) {
        for (int i = 0; i < array.Length; i++)
        {
            switch (array[i])
            {
                case 2:
                    gameObject.transform.localPosition = new Vector3(-20 + 10 * (i % 5), 55 - 10 * (i / 5), 115);
                    break;
                case 3:
                    break;
                defalt: break;
            }
        }
    }

    //public int[] CreateShape(int num_color, int wallColor)
    public int[] CreateShape()
    {
        int[] shape = new int[25];
        int[] dir = { -1, 1, -5, 5 };
        int num_one = UnityEngine.Random.Range(0, 5) + 4;
        int[] index = new int[num_one + 4];
        int center = UnityEngine.Random.Range(0, 25);
        index[0] = center;
        shape[center] = 1;
        int it = 1;
        while (it < index.Length)
        {
            int prev;
            if (it < num_one)
            {
                prev = index[UnityEngine.Random.Range(0, it)];
            }
            else
            {
                prev = index[UnityEngine.Random.Range(0, num_one)];
            }

            int direction = dir[UnityEngine.Random.Range(0, 4)];
            int next = prev + direction;
            Boolean isEdge = (direction == 1 || direction == -1) && ((prev + next) % 10 == 9);
            if (next >= 0 && next < 25 && !isEdge && shape[next] == 0)
            {
                index[it] = next;
                if (it < num_one)
                {
                    shape[next] = 1;
                }
                else if (it == index.Length - 4)
                {
                    shape[next] = 2;
                }
                else if (it == index.Length - 3)
                {
                    shape[next] = 4;
                }
                else
                {
                    shape[next] = 3;
                }
                it++;
            }

        }
        int index_two = index[index.Length - 4];
        int index_three_a = index[index.Length - 1];
        int index_three_b = index[index.Length - 2];

        Boolean[] visit = new Boolean[25];
        for (int i = 0; i < 25; i++)
        {
            visit[i] = false;
        }
        Boolean isThreeFound = false;
        Boolean isFourFound = false;
        Queue<int> nodeQueue = new Queue<int>();
        int node = index_two;
        nodeQueue.Enqueue(node);
        while (nodeQueue != null && nodeQueue.Count > 0)
        {
            node = nodeQueue.Dequeue();
            List<int> children = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                int direction = dir[i];
                int next = node + direction;
                Boolean isEdge = (direction == 1 || direction == -1) && ((node + next) % 10 == 9);
                if (next >= 0 && next < 25 && !isEdge)
                {
                    if (!visit[next] && shape[next] != 1)
                    {
                        children.Add(next);
                    }
                }
            }
            if (children != null && children != null && children.Count > 0)
            {
                foreach (int child in children)
                {
                    visit[child] = true;
                    if (shape[child] == 3)
                    {
                        isThreeFound = true;
                    }
                    if (shape[child] == 4)
                    {
                        isFourFound = true;
                    }
                    if (isThreeFound && isFourFound)
                    {
                        return shape;
                    }
                    nodeQueue.Enqueue(child);
                }
            }
        }

        return CreateShape();
        /*        int[] shape = new int[25]{0,0,0,0,0,
                                0,0,0,0,0,
                                0,3,3,0,0,
                                0,10,7,8,0,
                                9,1,1,1,2};
                shape[UnityEngine.Random.Range(0, 5)] = 1;
                return shape;*/

    }

}
