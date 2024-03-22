using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    int score = 0;
    private void OnEnable()
    {
        Debug.Log("Enabled");

        UIDocument uiDocument = GetComponent<UIDocument>();

       
    }
 

    public void ChangeScore()
    {
        Debug.Log("Button Clicked");
        UIDocument uiDocument = GetComponent<UIDocument>();
        Label label = uiDocument.rootVisualElement.Q("ScoreLabel") as Label;
        label.text = "Score: " + score++;
        
    }
}
