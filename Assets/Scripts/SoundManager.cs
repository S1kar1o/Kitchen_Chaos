using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_SOUND_EFECTS_VOLUME= "SoundEffects";
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClipsRefSO audioClipsRefSO;
    private float volume = 1f;
    private void Awake()
    {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_SOUND_EFECTS_VOLUME, 1f);

    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        CuttingCounter.OnAnyCat += CuttingCounter_OnAnyCat;
        PlayerController.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnPlaceSomethingHere += BaseCounter_OnPlaceSomethingHere;
        TrashCounter.OnAneObjectTrash += TrashCounter_OnAneObjectTrash;
    }

    private void TrashCounter_OnAneObjectTrash(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipsRefSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnPlaceSomethingHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipsRefSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
        PlayerController player = sender as PlayerController;
        PlaySound(audioClipsRefSO.foorStep, player.transform.position);
    }

    private void CuttingCounter_OnAnyCat(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipsRefSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipsRefSO.deliveryFail, deliveryCounter.transform.position);
    }
    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipsRefSO.deliverySuccess, deliveryCounter.transform.position);

    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }
    private void PlaySound(AudioClip clip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(clip, position, volumeMultiplier * volume);
    }
    public void PlayFootStepSound(Vector3 playerPosition, float volume)
    {
        PlaySound(audioClipsRefSO.foorStep, playerPosition, volume);
    }
    public void PlayCountDownSound()
    {
        PlaySound(audioClipsRefSO.warning, Vector3.zero);
    }
    public void PlayStoveBurningWarning(Vector3 position)
    {
        PlaySound(audioClipsRefSO.warning, position);
    }
    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1)
        {
            volume = 0;
        }
        PlayerPrefs.SetFloat(PLAYER_SOUND_EFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }
   
    public float GetVolumeMusic()
    {
        return volume;
    }
   
}
