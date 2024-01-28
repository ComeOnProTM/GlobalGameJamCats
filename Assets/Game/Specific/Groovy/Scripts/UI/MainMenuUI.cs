using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private const string LEVELONE = "CasinoScene";

    [Header("References")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(() => SceneManager.LoadScene(LEVELONE));
        quitButton.onClick.AddListener(() => Application.Quit());

        playButton.Select();
    }
}
