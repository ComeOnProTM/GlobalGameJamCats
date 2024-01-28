using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    //private const string LEVELONE = "CasinoScene";

    [Header("References")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject slideOut;
    [SerializeField] private Animator animator;
    [Header("Booleans")]
    [SerializeField] private bool creditsActive;


    // Start is called before the first frame update
    void Start()
    {
        creditsActive = false;
        slideOut.SetActive(false);

        playButton.onClick.AddListener(() => slideOut.SetActive(true));
        creditsButton.onClick.AddListener(() => activateCredits());
        quitButton.onClick.AddListener(() => Application.Quit());

        InputManager.Instance.OnSelectAction += InputManager_OnSelectAction;

        playButton.Select();
    }

    private void InputManager_OnSelectAction(object sender, System.EventArgs e)
    {
        if (creditsActive)
        {
            animator.SetTrigger("creditsExit");
        }
    }

    private void activateCredits()
    {
        credits.SetActive(true);
        creditsActive = true;
    }
}
