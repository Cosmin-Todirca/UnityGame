using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CreditController_TC : MonoBehaviour
{
    // Start is called before the first frame update
    private int countClicker;
    public SpriteRenderer sr;
    Color creditColor;
    private bool unfade,fade,exitScene;
    void Start()
    {
        countClicker = 0;
        sr = GetComponent<SpriteRenderer>();
        creditColor = sr.color;
        creditColor.a = 0;
        sr.color = creditColor;
        unfade = false;
        fade = false;
        exitScene = false;

    }
    private void Update()
    {
        if(unfade)
        {
            creditColor.a += 0.01f;
            sr.color = creditColor;
        }
        if (creditColor.a >= 1)
            unfade = false;

        if(fade)
        {
            creditColor.a -= 0.01f;
            sr.color = creditColor;
        }
        if (creditColor.a <= 0)
            fade = false;

        if(exitScene)
            SceneManager.LoadScene(0);

    }

    private void OnMouseDown()
    {
        countClicker++;
        if(countClicker==1)
        {
            unfade = true;
        }
        if (countClicker == 2)
        {
            fade=true;
        }
        if (countClicker > 2)
        {
            exitScene = true;
        }
    }
}
