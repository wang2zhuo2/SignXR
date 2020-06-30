using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class GestureToSound : MonoBehaviour
{
    public HandModelManager handModelManager;
    public Text typeBox;
    public Text chatBox;
    public UnityEngine.UI.Image sentenceIdentifier;
    public UnityEngine.UI.Image countingDownUI;
    public List<Sprite> countingDownSprites;
    public GameObject typeBox_white;
    public GameObject typeBox_blue;

    public Camera camera;

    private bool isCapturing;
    private Sign highlightedSentence;
    private Vector3[] captured_R_Dirs;
    private Vector3[] captured_L_Dirs;

    private ExperienceState experienceState = ExperienceState.Idle;


    private Quaternion currentCamRot = Quaternion.identity;

    GestureData Quote = new GestureData(new Vector3[]{
        new Vector3(0.3201641f, 0.3929585f, 0.8620206f),
        new Vector3(-0.2565548f, 0.4505963f, 0.8550693f),
        new Vector3(-0.371911f, 0.4308072f, 0.8222445f),
        new Vector3(-0.1774591f, -0.9803888f, 0.08571149f),
        new Vector3(-0.2985046f, -0.9527735f, 0.05583981f),
        new Vector3(-0.1811714f, -0.3654756f, 0.9130195f),
    }, new Vector3[]{
        new Vector3(-0.6295634f,0.2523633f,0.7348214f),
        new Vector3(0.02190151f,0.6899171f,0.7235568f),
        new Vector3(0.204675f,0.6320415f,0.7474173f),
        new Vector3(0.431082f,-0.8706462f,0.2369469f),
        new Vector3(0.5873454f,-0.777434f,0.2249929f),
        new Vector3(0.2537988f,-0.1894603f,0.9485205f),
    }, false);

    GestureData Hello = new GestureData(new Vector3[]{
        new Vector3(-0.4729501f,0.8628267f,-0.1784596f),
        new Vector3(-0.5770445f,0.8042979f,0.1418617f),
        new Vector3(-0.5765655f,0.7957888f,0.185181f),
        new Vector3(-0.5241703f,0.7979404f,0.2975507f),
        new Vector3(-0.4808267f,0.7907596f,0.3788202f),
        new Vector3(-0.8882669f,-0.4024375f,0.2214184f),
    },null, false);

    GestureData My = new GestureData(new Vector3[]{
        new Vector3(-0.8882669f,-0.4024375f,0.2214184f),
        new Vector3(-0.9920269f,0.123897f,0.02306735f),
        new Vector3(-0.9903806f,0.1279556f,-0.05266578f),
        new Vector3(-0.9958326f,0.07043716f,-0.05793256f),
        new Vector3(-0.9917232f,0.05746076f,-0.1148201f),
        new Vector3(-0.1655989f,0.2584933f,-0.9517133f),
    }, null,false);

    GestureData Name = new GestureData(new Vector3[]{
        new Vector3(-0.4580174f,0.7288663f,0.5088951f),
        new Vector3(-0.8980643f,0.3769242f,-0.2267355f),
        new Vector3(-0.9014893f,0.4063669f,-0.1489394f),
        new Vector3(0.6158625f,-0.06455632f,-0.7852042f),
        new Vector3(0.7874451f,0.04884087f,-0.6144477f),
        new Vector3(-0.5809613f,0.2333534f,-0.7797629f),
    }, null,false);

    GestureData Sana = new GestureData(new Vector3[]{
        new Vector3(-0.4419136f,0.795024f,0.4155109f),
        new Vector3(0.2537825f,-0.9604024f,0.1149868f),
        new Vector3(0.2279411f,-0.9593278f,-0.1665341f),
        new Vector3(0.1420159f,-0.9731506f,-0.1811332f),
        new Vector3(-0.001346335f,-0.9901798f,-0.1397938f),
        new Vector3(-0.1786944f,-0.2283153f,0.9570479f),
    },null, false);

    GestureData What = new GestureData(new Vector3[]{
        new Vector3(0.2500401f,0.4883281f,0.8360708f),
        new Vector3(-0.1895122f,0.5651773f,0.8029066f),
        new Vector3(-0.2300378f,0.6579321f,0.7170829f),
        new Vector3(-0.2463807f,0.7536974f,0.6092911f),
        new Vector3(-0.2530912f,0.7868088f,0.5629163f),
        new Vector3(-0.08377865f,0.9550385f,-0.2843986f),
    }, null, false);

    GestureData Your = new GestureData(new Vector3[]{
        new Vector3(-0.3350566f,0.8637264f,0.3764488f),
        new Vector3(-0.05029371f,0.9224439f,0.3828409f),
        new Vector3(-0.07525247f,0.8698674f,0.4875107f),
        new Vector3(-0.03021914f,0.8206217f,0.5706731f),
        new Vector3(0.04890499f,0.8411429f,0.5385988f),
        new Vector3(-0.1280358f,-0.4135669f,0.9014263f),
    }, null, false);


    GestureData Nice = new GestureData(new Vector3[]{
        new Vector3(-0.8400488f,0.2993956f,0.4524159f),
        new Vector3(-0.7071092f,0.04842027f,0.7054432f),
        new Vector3(-0.6975498f,-0.09028382f,0.7108247f),
        new Vector3(-0.6538353f,-0.18927f,0.7325814f),
        new Vector3(-0.6589578f,-0.288354f,0.6947132f),
        new Vector3(-0.04791242f,-0.9364421f,0.3475353f),
    }, null, false);


    GestureData Meet = new GestureData(new Vector3[]{
        new Vector3(-0.3363496f,0.260224f,0.9050703f),
        new Vector3(-0.1210194f,0.8725608f,0.4732778f),
        new Vector3(-0.1906672f,-0.9413025f,-0.2785605f),
        new Vector3(-0.06590384f,-0.8482162f,-0.5255341f),
        new Vector3(-0.1937329f,-0.8230978f,-0.5338329f),
        new Vector3(-0.5737526f,-0.6030017f,0.5542535f),
    }, null, false);

    GestureData How = new GestureData(new Vector3[]{
        new Vector3(-0.6209244f,0.4934644f,0.6090535f),
        new Vector3(0.5955777f,0.3273524f,-0.7335707f),
        new Vector3(0.6473337f,0.4079084f,-0.6438721f),
        new Vector3(0.5262411f,0.6195227f,-0.5824627f),
        new Vector3(0.2593566f,0.8550775f,-0.4489709f),
        new Vector3(-0.5432679f,0.5937062f,-0.5936102f),
    }, null, false);


    GestureData You = new GestureData(new Vector3[]{
        new Vector3(-0.3152087f,-0.1814092f,0.9315228f),
        new Vector3(-0.4799011f,0.5733309f,0.6640682f),
        new Vector3(0.375175f,-0.5743068f,-0.7276098f),
        new Vector3(0.2476379f,-0.5636832f,-0.7879959f),
        new Vector3(-0.3907948f,0.07010546f,0.9178045f),
        new Vector3(-0.6089964f,-0.7345235f,0.2993305f),
    }, null, false);

    GestureData Good = new GestureData(new Vector3[]{
        new Vector3(0.4718042f,0.8580487f,0.2028622f),
        new Vector3(0.09950905f,0.9844131f,-0.1450137f),
        new Vector3(0.009058675f,0.9792373f,-0.2025154f),
        new Vector3(-0.1088986f,0.9688807f,-0.222287f),
        new Vector3(-0.1970468f,0.9312972f,-0.3063634f),
        new Vector3(0.0243347f,0.306031f,-0.9517105f),
    }, null, false);

    GestureData MyNameIsSana = new GestureData(null, null, false);
    GestureData WhatIsYourName = new GestureData(null, null, false);
    GestureData HowAreYou = new GestureData(null, null, false);
    GestureData NiceToMeetYou = new GestureData(null, null, false);

    Dictionary<Sign, GestureData> gestureBase = new Dictionary<Sign, GestureData>();

    public List<SentenceData> SentenceBase;
    

    private void Awake()
    {
        //allGestureData.Add(GestureName.Quote, Quote);
        gestureBase.Add(Sign.Hello, Hello); // +distance
        gestureBase.Add(Sign.My, My); //
        gestureBase.Add(Sign.Name, Name); 
        gestureBase.Add(Sign.Sana, Sana); 
        gestureBase.Add(Sign.What, What); 
        gestureBase.Add(Sign.Your, Your); 
        gestureBase.Add(Sign.Nice, Nice); 
        gestureBase.Add(Sign.Meet, Meet); 
        gestureBase.Add(Sign.You, You); //
        gestureBase.Add(Sign.How, How);
        gestureBase.Add(Sign.Good, Good);
        gestureBase.Add(Sign.MyNameIsSana, MyNameIsSana);
        gestureBase.Add(Sign.WhatIsYourName, WhatIsYourName);
        gestureBase.Add(Sign.HowAreYou, HowAreYou);
        gestureBase.Add(Sign.NiceToMeetYou, NiceToMeetYou);
    }


    void Update()
    {

        if (handModelManager.leapProvider)
        {
            Frame frame = handModelManager.leapProvider.CurrentFrame;

            if (Input.GetKeyDown(KeyCode.Space) && experienceState == ExperienceState.Idle)
            {
                typeBox_white.SetActive(false);
                typeBox_blue.SetActive(true);
                experienceState = ExperienceState.Chatting;
            }
            else if (Input.GetKeyDown(KeyCode.Space) && experienceState == ExperienceState.Chatting)
            {
                typeBox_white.SetActive(true);
                typeBox_blue.SetActive(false);
                experienceState = ExperienceState.Idle;
            }
            else if (Input.GetKeyDown(KeyCode.Backspace)/* && experienceState == ExperienceState.Chatting*/)
            {
                RemoveWordsMark(Sign.Hello,Sign.My,Sign.Name,Sign.Sana, Sign.What, Sign.Your,Sign.Nice,Sign.Meet,Sign.You,Sign.How,Sign.Good);
            }
            else if (Input.GetKeyDown(KeyCode.R) && experienceState == ExperienceState.Idle)
            {
                StartCoroutine(CountingDown());
                experienceState = ExperienceState.Recording;
            }

            

            if (frame.Hands.Count > 0)
            {
                var calibration = GetDeltaRotMaterix();
                List<Hand> hands = frame.Hands;
                foreach (var hand in hands)
                {
                    if (hand.IsLeft)
                    {
                        //Debug.Log(experienceState.ToString());
                        if (GetGesture(hand, Quote.l_Dirs,calibration) && experienceState == ExperienceState.Idle)
                        {
                            typeBox_white.SetActive(false);
                            typeBox_blue.SetActive(true);
                            experienceState = ExperienceState.Chatting;
                        }

                        if (Input.GetKeyDown(KeyCode.Alpha2) || isCapturing)
                        {
                            captured_L_Dirs = new Vector3[6];

                            Debug.Log("Capturing left hand...");
                            Capture(hand,out captured_L_Dirs);
                        }
                    }

                    if (hand.IsRight)
                    {
                        if (GetGesture(hand, Quote.r_Dirs,calibration) && experienceState == ExperienceState.Chatting)
                        {
                            typeBox_white.SetActive(true);
                            typeBox_blue.SetActive(false);
                            experienceState = ExperienceState.Idle;
                        }

                        switch (experienceState)
                        {
                            case ExperienceState.Chatting:
                                Translate(hand,calibration);
                                break;
                            case ExperienceState.Recording:
                                break;
                            case ExperienceState.Idle:
                                DetectSentencesAndPlay();
                                break;
                            default:
                                break;
                        }


                        if (Input.GetKeyDown(KeyCode.Alpha1)||isCapturing)
                        {
                            captured_R_Dirs = new Vector3[6];
                            Debug.Log("Capturing left hand...");
                            Capture(hand, out captured_R_Dirs);
                        }
                    }


                }             
            }
        }

        if (isCapturing)
        {
            gestureBase[highlightedSentence] = new GestureData(captured_R_Dirs, captured_L_Dirs, false);
        }

        var typeBoxText = "";
        foreach (var gesture in gestureBase)
        {
            if (gesture.Value.isMarked)
            {
                typeBoxText += gesture.Key.ToString()+", ";
            }
        }
        typeBox.text = typeBoxText;

        isCapturing = false;
        captured_L_Dirs = captured_R_Dirs = null;
    }

    private void Capture(Hand hand, out Vector3[] capturedDirs)
    {
        capturedDirs = new Vector3[6];
        for (int i = 0; i < 6; i++)
        {
            if (i == 5)
            {
                //var idVec = (hand.Fingers[0].Direction + hand.Fingers[1].Direction + hand.Fingers[2].Direction
                //    + hand.Fingers[3].Direction + hand.Fingers[4].Direction).Normalized;

                var palmNormal = hand.PalmNormal.ToVector3();
                capturedDirs[i] = palmNormal;
                Debug.Log("new Vector3" + "(" + palmNormal.x + "f," + palmNormal.y + "f," + palmNormal.z + "f),");

            }
            else
            {
                var fingerDir = hand.Fingers[i].Direction.ToVector3();
                capturedDirs[i] = fingerDir;
                Debug.Log("new Vector3" + "(" + fingerDir.x + "f," + fingerDir.y + "f," + fingerDir.z + "f),");
            }
        }
    }

    IEnumerator CountingDown()
    {
        countingDownUI.enabled = true;
        for (int i = 0; i < 5; i++)
        {
            countingDownUI.sprite = countingDownSprites[i];
            yield return new WaitForSeconds(1);
        }
        countingDownUI.enabled = false;
        isCapturing = true;
        experienceState = ExperienceState.Idle;
    }

    private Quaternion GetDeltaRot()
    {
        if(camera.transform.rotation.eulerAngles.y != 0)
        {
            return Quaternion.Euler(0, camera.transform.rotation.eulerAngles.y, 0);
        }

        return Quaternion.identity;
    }

    private Matrix4x4 GetDeltaRotMaterix()
    {
        return Matrix4x4.Rotate(GetDeltaRot()).transpose;
    }

    private void Translate(Hand hand,Matrix4x4 _calibration)
    {
        foreach (var gesture in gestureBase)
        {
            if(gesture.Value.r_Dirs != null)
            {
                if (GetGesture(hand, gesture.Value.r_Dirs,_calibration) && !gesture.Value.isMarked)
                {
                    Debug.Log(gesture.Key);
                    gesture.Value.isMarked = true;
                }
            }

        }
    }

    private void DetectSentencesAndPlay()
    {

        AudioClip[] sentences = new AudioClip[0];
        Sign lastHotKeySign = Sign.None;

        if (GetWordsMark(Sign.Hello))
        {
            var sentenceTxt = "Hello.";
            chatBox.text = sentenceTxt;
            chatBox.color = Color.white;
            sentences = UpdateContent(sentences, sentenceTxt);
            RemoveWordsMark(Sign.Hello);
            lastHotKeySign = Sign.Hello;
        }

        if (GetWordsMark(Sign.My,Sign.Name,Sign.Sana)||GetWordsMark(Sign.MyNameIsSana))
        {
            var sentenceTxt = "My name is Sana.";
            chatBox.text = sentenceTxt;
            chatBox.color = Color.white;
            sentences = UpdateContent(sentences, sentenceTxt);
            RemoveWordsMark(Sign.My, Sign.Name, Sign.Sana,Sign.MyNameIsSana);
            lastHotKeySign = Sign.MyNameIsSana;
        }

        if (GetWordsMark(Sign.What, Sign.Your, Sign.Name)||GetWordsMark(Sign.WhatIsYourName))
        {
            var sentenceTxt = "What's your name?";
            chatBox.text = sentenceTxt;
            chatBox.color = Color.white;
            sentences = UpdateContent(sentences, sentenceTxt);
            RemoveWordsMark(Sign.What, Sign.Your, Sign.Name,Sign.WhatIsYourName);
            lastHotKeySign = Sign.WhatIsYourName;
        }

        if (GetWordsMark(Sign.Nice, Sign.Meet, Sign.You)||GetWordsMark(Sign.NiceToMeetYou))
        {
            var sentenceTxt = "Nice to meet you.";
            chatBox.text = sentenceTxt;
            chatBox.color = Color.white;
            sentences = UpdateContent(sentences, sentenceTxt);
            RemoveWordsMark(Sign.Nice, Sign.Meet, Sign.You,Sign.NiceToMeetYou);
            lastHotKeySign = Sign.NiceToMeetYou;
        }

        if (GetWordsMark(Sign.How, Sign.You)||GetWordsMark(Sign.HowAreYou))
        {
            var sentenceTxt = "How are you?";
            chatBox.text = sentenceTxt;
            chatBox.color = Color.white;
            sentences = UpdateContent(sentences, sentenceTxt);
            RemoveWordsMark(Sign.How, Sign.You,Sign.HowAreYou);
            lastHotKeySign = Sign.HowAreYou;
        }

        if (GetWordsMark(Sign.Good))
        {
            var sentenceTxt = "Good.";
            chatBox.text = sentenceTxt;
            chatBox.color = Color.white;
            sentences = UpdateContent(sentences, sentenceTxt);
            RemoveWordsMark(Sign.Good);
            lastHotKeySign = Sign.Good;
        }

        if (sentences.Length > 0)
        {
            highlightedSentence = lastHotKeySign;
            chatBox.text += "\n";
            //sentenceIdentifier.transform.localPosition += (chatBox.fontSize + 5*chatBox.lineSpacing) * Vector3.down;
            StartCoroutine(Speak(sentences));
        }
    }

    private bool GetWordsMark(params Sign[] gestureNames)
    {
        for (int i = 0; i < gestureNames.Length; i++)
        {
            if (!gestureBase[gestureNames[i]].isMarked) return false;
        }
        return true;
    }

    private void RemoveWordsMark(params Sign[] gestureNames)
    {
        for (int i = 0; i < gestureNames.Length; i++)
        {
            gestureBase[gestureNames[i]].isMarked = false;
        }
    }

    private AudioClip[] UpdateContent(AudioClip[] sentences, string sentenceTxt)
    {
        var currentCollectedSentences = sentences;
        sentences = new AudioClip[currentCollectedSentences.Length + 1];
        for (int i = 0; i < currentCollectedSentences.Length; i++)
        {
            sentences[i] = currentCollectedSentences[i];
        }
        sentences[sentences.Length - 1] = SentenceBase.Find(s => s.name == sentenceTxt).clip;
        return sentences;
    }

    IEnumerator Speak(AudioClip[] _allSentences)
    {
        var audio = GetComponent<AudioSource>();
        for (int i = 0; i < _allSentences.Length; i++)
        {
            audio.clip = _allSentences[i];
            audio.Play();
            yield return new WaitForSeconds(audio.clip.length);
        }
    }

    private bool GetGesture(Hand _hand, Vector3[] _referenceGesture, Matrix4x4 _calibration)
    {
        var score = 0f;
        for (int i = 0; i < 6; i++)
        {
            if (i < 5)
            {
                score += Vector3.Dot(_calibration.MultiplyPoint3x4(_hand.Fingers[i].Direction.ToVector3()), _referenceGesture[i]);
            }

            if (i == 5)
            {
                score += Vector3.Dot(_calibration.MultiplyPoint3x4(_hand.PalmNormal.ToVector3()), _referenceGesture[i]);
            }
        }
           
        if(score/6 > 0.88f)
            return true;

        return false;
    }

}


public enum Sign
{
    None,
    Quote,
    Hello,
    My,
    Name,
    Sana,
    What,
    Your,
    Nice,
    Meet,
    How,
    You,
    Good,
    MyNameIsSana,
    WhatIsYourName,
    HowAreYou,
    NiceToMeetYou,
}

public enum ExperienceState
{
    Chatting,
    Recording,
    Idle
}

public class GestureData
{
    public Vector3[] r_Dirs;
    public Vector3[] l_Dirs;
    public bool isMarked;
    //public bool needExtraCondition;

    public GestureData(Vector3[] _r_Dirs, Vector3[] _l_Dirs, bool _isMarked)
    {
        r_Dirs = _r_Dirs;
        l_Dirs = _l_Dirs;
        isMarked = _isMarked;
    }

    //public void CheckIfTranslating(Sign _sign, Hand _hand)
    //{
    //    switch (_sign)
    //    {
    //        case Sign.None:
    //            break;
    //        case Sign.Quote:
    //            break;
    //        case Sign.Hello:
    //            needExtraCondition = true;
    //            break;
    //        case Sign.My:
    //            break;
    //        case Sign.Name:
    //            break;
    //        case Sign.Sana:
    //            break;
    //        case Sign.What:
    //            break;
    //        case Sign.Your:
    //            break;
    //        case Sign.Nice:
    //            break;
    //        case Sign.Meet:
    //            break;
    //        case Sign.How:
    //            break;
    //        case Sign.You:
    //            break;
    //        case Sign.Good:
    //            break;
    //        case Sign.MyNameIsSana:
    //            break;
    //        case Sign.WhatIsYourName:
    //            break;
    //        case Sign.HowAreYou:
    //            break;
    //        case Sign.NiceToMeetYou:
    //            break;
    //        default:
    //            break;
    //    }

    //}
}

[System.Serializable]
public class SentenceData
{
    public string name;
    public AudioClip clip;
    public Sign hotKeySign;
}
