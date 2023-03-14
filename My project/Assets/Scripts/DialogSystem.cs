using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;                   //UI 를 컨트롤 할 것이라서 추가
using System;                           //Arrary 수정 기능을 사용 하기 위해 추가 

public class DialogSystem : MonoBehaviour
{
    [SerializeField]
    private SpeakerUI[] speakers;
    [SerializeField]
    private DialogData[] dialogs;
    [SerializeField]
    private bool DialogInit = true;     // Init 검사 FLAG 
    [SerializeField]
    private bool dialogsDB = false;     // DB데이터를 읽음 FLAG

    public int currentDialogIndex = -1;
    public int currentSpeakerIndex = 0;
    public float typingSpeed = 0.1f;        //해당 시간동안 멈췄다가 글씨가 나타남 
    private bool isTypingEffect = false;    //재생중인지 판단 FLAG

    private void Awake()
    {
        SetAllClose();
    }

    public bool UpdateDialog(int currentIndex, bool InitType)
    {
        if (DialogInit == true && InitType == true)
        {   //1번만 호출 
            SetAllClose();
            SetNextDialog(currentIndex);
            DialogInit = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isTypingEffect == true)
            {
                isTypingEffect = false;
                StopCoroutine("OnTypingText");
                speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;

                speakers[currentSpeakerIndex].objectArrow.SetActive(true);
                return false;
            }

            if (dialogs[currentDialogIndex].nextindex != -100)
            {
                SetNextDialog(dialogs[currentDialogIndex].nextindex);
            }
            else
            {
                SetAllClose();
                DialogInit = true;
                return true;
            }
        }

        return false;
    }

    private void SetActiveObject(SpeakerUI speaker, bool visible)
    {
        speaker.imageDialog.gameObject.SetActive(visible);
        speaker.textName.gameObject.SetActive(visible);
        speaker.textDialogue.gameObject.SetActive(visible);

        speaker.objectArrow.SetActive(false);

        Color color = speaker.imageDialog.color;
        if (visible)
        {
            color.a = 1;
        }
        else
        {
            color.a = 0.2f;
        }
        speaker.imgCharacter.color = color;

    }

    private void SetAllClose()
    {
        for (int i = 0; i < speakers.Length; ++i)
        {
            SetActiveObject(speakers[i], false);
        }
    }
    private void SetNextDialog(int currentIndex)
    {
        SetAllClose();
        currentDialogIndex = currentIndex;
        currentSpeakerIndex = dialogs[currentDialogIndex].speakerUIindex;
        SetActiveObject(speakers[currentSpeakerIndex], true);

        speakers[currentSpeakerIndex].textName.text = dialogs[currentDialogIndex].name;
        StartCoroutine("OnTypingText");

    }

    private IEnumerator OnTypingText()
    {
        int index = 0;
        isTypingEffect = true;

        if (dialogs[currentDialogIndex].characterPath != "None")
        {
            speakers[currentDialogIndex].imgCharacter =
                (Image)Resources.Load(dialogs[currentDialogIndex].characterPath);
        }

        while (index < dialogs[currentDialogIndex].dialogue.Length + 1)
        {
            speakers[currentSpeakerIndex].textDialogue.text =
                dialogs[currentDialogIndex].dialogue.Substring(0, index);
            index++;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTypingEffect = false;

        speakers[currentSpeakerIndex].objectArrow.SetActive(true);
    }
}

[System.Serializable]
public struct SpeakerUI
{
    public Image imgCharacter;
    public Image imageDialog;
    public Text textName;
    public Text textDialogue;
    public GameObject objectArrow;
}

[System.Serializable]
public struct DialogData
{
    public int index;
    public int speakerUIindex;
    public string name;
    public string dialogue;
    public string characterPath;
    public int tweenType;
    public int nextindex;

    //추가로 원하는대로 데이터를 입력 할 수 있음 
}