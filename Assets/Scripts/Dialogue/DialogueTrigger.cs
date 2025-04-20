using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;
//using Yarn.Unity.ActionAnalyser;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public bool hasTalkedBefore = false;
    public GameObject splitScreen;

    List<string> nodeNames = new List<string>
    {
        "WhatPromotion",
        "Dick",
        "Loathe"
    };

    private void Start()
    {
        splitScreen.SetActive(false);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && hasTalkedBefore == false)
        {
            PlayRandomDialogue();
            //dialogueRunner.StartDialogue("WhatPromotion");
            hasTalkedBefore = true;
            splitScreen.SetActive(true);
        }
        else
        {
            Repeat();
        }
    }

    public void Repeat()
    {
        if (Input.GetKeyDown(KeyCode.E) && hasTalkedBefore == true)
        {
            dialogueRunner.Stop();
            //dialogueRunner.StartDialogue("Repeat");
            PlayRandomDialogue();
            GoToLevelScene();
        }
    }

    public void Skip()
    {
        if (Input.GetKeyDown(KeyCode.E) && dialogueRunner.IsDialogueRunning)
        {
            
        }
    }

    public void PlayRandomDialogue()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(nodeNames.Count);
        dialogueRunner.StartDialogue(nodeNames[index]);
    }

    public void GoToLevelScene()
    {
        SceneManager.LoadScene("PrototypeLevel");
    }
}
