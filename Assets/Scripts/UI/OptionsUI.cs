using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Button moveUpButtonSettingButton;
    [SerializeField] private Button moveDownButtonSettingButton;
    [SerializeField] private Button moveLeftButtonSettingButton;
    [SerializeField] private Button moveRightButtonSettingButton;
    [SerializeField] private Button interactSettingButton;
    [SerializeField] private Button alternativeInteractSettingButton;
    [SerializeField] private Button pauseSettingButton;


    [SerializeField] private Button gamePadInteract;
    [SerializeField] private Button gamePadInteractAlternative;
    [SerializeField] private Button gamePadPause;
    [SerializeField] private TextMeshProUGUI gamePadInteractText;
    [SerializeField] private TextMeshProUGUI gamePadInteractAlternativeText;
    [SerializeField] private TextMeshProUGUI gamePadPauseText;


    [SerializeField] private TextMeshProUGUI moveUpButtonSettingText;
    [SerializeField] private TextMeshProUGUI moveDownButtonSettingText;
    [SerializeField] private TextMeshProUGUI moveLeftButtonSettingText;
    [SerializeField] private TextMeshProUGUI moveRightButtonSettingText;
    [SerializeField] private TextMeshProUGUI interactSettingText;
    [SerializeField] private TextMeshProUGUI alternativeInteractSettingText;
    [SerializeField] private TextMeshProUGUI pauseSettingText;
    public static OptionsUI Instance { get; private set; }
    [SerializeField] private Button musicVolume;
    [SerializeField] private Button soundEffectsVolume;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;

    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private Transform pressToRebindTransform;

    private  Action onCloseMenuPause;

    private void Awake()
    {
        Instance = this;
        soundEffectsVolume.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            updateVolumeUI();
        });
        musicVolume.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            updateVolumeUI();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseMenuPause();
        });


        moveUpButtonSettingButton.onClick.AddListener(() => { PressToRebind(InputSystem.Binding.Move_Up); });
        moveDownButtonSettingButton.onClick.AddListener(() => { PressToRebind(InputSystem.Binding.Move_Down); });
        moveLeftButtonSettingButton.onClick.AddListener(() => { PressToRebind(InputSystem.Binding.Move_Left); });
        moveRightButtonSettingButton.onClick.AddListener(() => { PressToRebind(InputSystem.Binding.Move_Right); });
        interactSettingButton.onClick.AddListener(() => { PressToRebind(InputSystem.Binding.Interact); });
        alternativeInteractSettingButton.onClick.AddListener(() => { PressToRebind(InputSystem.Binding.Interact_Alternative); });
        pauseSettingButton.onClick.AddListener(() => { PressToRebind(InputSystem.Binding.Pause); });

        gamePadPause.onClick.AddListener(() => { PressToRebind(InputSystem.Binding.GamePad_Pause); });
        gamePadInteract.onClick.AddListener(() => { PressToRebind(InputSystem.Binding.GamePad_Interact); });
        gamePadInteractAlternative.onClick.AddListener(() => { PressToRebind(InputSystem.Binding.GamePad_Interact_Alternative); });
    }
    private void Start()
    {
        GameManager.Instance.OnGameUnPauseAction += GameManager_OnGameUnPauseAction;
        updateVolumeUI();
        Hide();
        HidePressToRebindKey();

    }

    private void GameManager_OnGameUnPauseAction(object sender, System.EventArgs e)
    {
        Hide();
    }


    private void updateVolumeUI()
    {
        soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolumeMusic() * 10).ToString();
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolumeMusic() * 10).ToString();

        moveUpButtonSettingText.text = InputSystem.Instance.GetBindingText(InputSystem.Binding.Move_Up);
        moveDownButtonSettingText.text = InputSystem.Instance.GetBindingText(InputSystem.Binding.Move_Down);
        moveLeftButtonSettingText.text = InputSystem.Instance.GetBindingText(InputSystem.Binding.Move_Left);
        moveRightButtonSettingText.text = InputSystem.Instance.GetBindingText(InputSystem.Binding.Move_Right);
        interactSettingText.text = InputSystem.Instance.GetBindingText(InputSystem.Binding.Interact);
        alternativeInteractSettingText.text = InputSystem.Instance.GetBindingText(InputSystem.Binding.Interact_Alternative);
        pauseSettingText.text = InputSystem.Instance.GetBindingText(InputSystem.Binding.Pause);

        gamePadInteractText.text = InputSystem.Instance.GetBindingText(InputSystem.Binding.Interact);
        gamePadInteractAlternativeText.text = InputSystem.Instance.GetBindingText(InputSystem.Binding.Interact_Alternative);
        gamePadPauseText.text = InputSystem.Instance.GetBindingText(InputSystem.Binding.Pause);

    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show(Action onCloseMenuPause)
    {
        this.onCloseMenuPause = onCloseMenuPause;
        gameObject.SetActive(true);
        closeButton.Select();

    }
    private void HidePressToRebindKey()
    {
        pressToRebindTransform.gameObject.SetActive(false);
    }
    public void ShowPressToRebindKey()
    {
        pressToRebindTransform.gameObject.SetActive(true);
        soundEffectsVolume.Select();

    }
    public void PressToRebind(InputSystem.Binding binding)
    {
        ShowPressToRebindKey();
        InputSystem.Instance.ReBinding(binding, () =>
        {
            HidePressToRebindKey();
            updateVolumeUI();
        }
        );
    }
}
