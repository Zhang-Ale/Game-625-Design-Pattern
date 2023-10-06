using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour, IObserver
{
    [SerializeField] int _destroyCount = 0;
    int _destroyThreshold = 2;
    bool comboReset; 
    Coroutine _currentDestroyResetRoutine = null;
    Coroutine _currentPowerUpResetRoutine = null;
    public AudioSource _audioPlayer1, _audioPlayer2;
    public AudioClip _shootAudioClip;
    public Animator comboAnim;
    public AudioClip _comboAudioClip;
    public AudioClip _collectAudioClip;
    public CameraScript CS;
    public GameObject powerUpIcon; 
    public ParticleSystem splashPart; 
    public Transform enemyDeathPos; 

    public void OnNotify(GameObject GameObj, Action actionType)
    {
        switch (actionType)
        {
            case (Action.OnEnemyDestroy):
                splashPart.transform.position = GameObj.transform.position;
                if (_currentDestroyResetRoutine != null)
                {
                    StopCoroutine(_currentDestroyResetRoutine);
                }
                _destroyCount += 1;
                splashPart.GetComponent<ParticleController>().play = true;

                if (_destroyCount == _destroyThreshold && !comboReset)
                {
                    comboAnim.SetTrigger("Play");
                    comboReset = true; 
                }
                StartCoroutine(ComboResetRoutine());
                _currentDestroyResetRoutine = StartCoroutine(DestroyResetRoutine());
                break; //(exits the switch)
                //default(exits the whole void function)

            case (Action.OnPlayerShoot):
                CS.ShakeCam(0.05f, 0.01f);
                _audioPlayer2.Play();
                break;

            case (Action.OnPowerUpCollect):
                if (_currentPowerUpResetRoutine != null)
                {
                    StopCoroutine(_currentPowerUpResetRoutine);
                }
                _audioPlayer1.Play();
                powerUpIcon.SetActive(true);
                _currentPowerUpResetRoutine = StartCoroutine(PowerUpResetRoutine());
                break;

            default:
                break;
        }
        Debug.Log(actionType.ToString());
    }

    IEnumerator DestroyResetRoutine()
    {
        yield return new WaitForSeconds(4f);
        _destroyCount = 0;
    }

    IEnumerator PowerUpResetRoutine()
    {
        yield return new WaitForSeconds(5f);
        powerUpIcon.SetActive(false);
    }

    IEnumerator ComboResetRoutine()
    {
        yield return new WaitForSeconds(5f);
        comboReset = false; 
    }
}
