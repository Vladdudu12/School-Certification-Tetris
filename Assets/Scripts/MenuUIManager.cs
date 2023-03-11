using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class MenuUIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _playerName;
    [SerializeField]
    private TextMeshProUGUI _playerLevel;

    [SerializeField]
    private Sprite[] _characterSprites;

    [SerializeField]
    private GameObject _settingsMenu;


    private void Awake()
    {
        int randomIndex = Random.Range(0, _characterSprites.Length);
        GameObject.Find("Character").GetComponent<Image>().sprite = _characterSprites[randomIndex];
    }


    private void Start()
    {
        _playerName.text = "Vlad";
        _playerLevel.text = "1";

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void UpdatePlayerLevel(int level)
    {
        _playerLevel.text = level.ToString();
    }

    private void UpdatePlayerName(string name)
    {
        _playerName.text = name;
    }
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        _settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        _settingsMenu.SetActive(false);
    }




    /*
     public void PVPButton()
     {
         //start matchmaking
     }
    */
}
