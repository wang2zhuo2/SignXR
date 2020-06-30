using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Response : MonoBehaviour
{

    public GameObject frame;
    public Text chatBox;
    int keyStrokeCount = 1;
    //public AudioClip zdf423;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<AudioSource>().clip = zdf423;
        //GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {

            //frame.transform.localPosition += (chatBox.fontSize + chatBox.lineSpacing) * Vector3.down*2;
            //frame.SetActive(true);
            if (keyStrokeCount == 1)
            {
                chatBox.text = "Hi, my name is John, nice to meet you, Sana.\n";
                chatBox.color = Color.blue;
            }
            if (keyStrokeCount == 2)
            {
                chatBox.text = "I am good, you?\n";
                chatBox.color = Color.blue;
            }

            keyStrokeCount = keyStrokeCount+1;

        }



    }
}
