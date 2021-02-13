using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button monkeyButton;
    public Button elephantButton;
    public Button birdButton;

    public GameObject monkeyText;
    public GameObject elephantText;
    public GameObject birdText;


    private void Update()
    {

    }


    private float currCountdownValueMonkey;
    float monkeyFillAmount;
    public void OnClick_MonkeyButton()
    {
        if (CharacterController.Instance.currentCharacter != "Monkey")
        {
            CharacterController.Instance.currentCharacter = "Monkey";
            StartCoroutine(StartCountdownMonkey(10));
            monkeyButton.GetComponent<Button>().interactable = false;
            monkeyText.GetComponent<Text>().text = currCountdownValueMonkey.ToString();
            monkeyText.SetActive(true);
            monkeyButton.GetComponent<Image>().fillAmount = 0;
        }
    }
    public IEnumerator StartCountdownMonkey(float countdownValueMonkey)
    {
        currCountdownValueMonkey = countdownValueMonkey;
        while (currCountdownValueMonkey >= 0)
        {
            monkeyText.GetComponent<Text>().text = currCountdownValueMonkey.ToString();
            yield return new WaitForSeconds(1.0f);
            currCountdownValueMonkey--;
            monkeyFillAmount++;

            if (monkeyFillAmount != countdownValueMonkey)
            {
                monkeyButton.GetComponent<Image>().fillAmount = monkeyFillAmount / countdownValueMonkey;
            }

            if (currCountdownValueMonkey == 0)
            {
                monkeyButton.GetComponent<Button>().interactable = true;
                monkeyText.SetActive(false);
            }
        }
    }


    private float currCountdownValuElephant;
    float elephantFillAmount;
    public void OnClick_ElephantButton()
    {
        if (CharacterController.Instance.currentCharacter != "Elephant")
        {
            CharacterController.Instance.currentCharacter = "Elephant";
            StartCoroutine(StartCountdownElephant(10));
            elephantButton.GetComponent<Button>().interactable = false;
            elephantText.GetComponent<Text>().text = currCountdownValuElephant.ToString();
            elephantText.SetActive(true);
            elephantButton.GetComponent<Image>().fillAmount = 0;
        }
    }
    public IEnumerator StartCountdownElephant(float countdownValueElephant)
    {
        currCountdownValuElephant = countdownValueElephant;
        while (currCountdownValuElephant >= 0)
        {
            elephantText.GetComponent<Text>().text = currCountdownValuElephant.ToString();
            yield return new WaitForSeconds(1.0f);
            currCountdownValuElephant--;
            elephantFillAmount++;

            if (elephantFillAmount != countdownValueElephant)
            {
                elephantButton.GetComponent<Image>().fillAmount = elephantFillAmount / countdownValueElephant;
            }

            if (currCountdownValuElephant == 0)
            {
                elephantButton.GetComponent<Button>().interactable = true;
                elephantText.SetActive(false);
            }
        }
    }


    private float currCountdownValuBird;
    float birdFillAmount;
    public void OnClick_BirdButton()
    {
        if (CharacterController.Instance.currentCharacter != "Bird")
        {
            CharacterController.Instance.currentCharacter = "Bird";
            StartCoroutine(StartCountdownBird(10));
            birdButton.GetComponent<Button>().interactable = false;
            birdText.GetComponent<Text>().text = currCountdownValuBird.ToString();
            birdText.SetActive(true);
            birdButton.GetComponent<Image>().fillAmount = 0;
        }
    }
    public IEnumerator StartCountdownBird(float countdownValueBird)
    {
        currCountdownValuBird = countdownValueBird;
        while (currCountdownValuBird >= 0)
        {
            birdText.GetComponent<Text>().text = currCountdownValuBird.ToString();
            yield return new WaitForSeconds(1.0f);
            currCountdownValuBird--;
            birdFillAmount++;

            if (birdFillAmount != countdownValueBird)
            {
                birdButton.GetComponent<Image>().fillAmount = birdFillAmount / countdownValueBird;
            }

            if (currCountdownValuBird == 0)
            {
                birdButton.GetComponent<Button>().interactable = true;
                birdText.SetActive(false);
            }
        }
    }
}
