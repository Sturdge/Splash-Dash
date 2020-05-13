/* 
 * Created by:
 * Name: Dominik Waldowski
 * Sid: 1604336
 * Date Created: 29/09/2019
 * Last Modified: 29/09/2019
 */ 
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField]
    private Sprite[] countDownImages;
    [SerializeField]
    private Image countdownVisual;
    [SerializeField]
    private int countDown;
    [SerializeField]
    private float timer;
    [SerializeField]
    [Range(1,3)]
    private int rate;
    [SerializeField]
    private bool isCounting;

    SoundManager soundManager;

    AudioSource audio;

    bool doneOne, doneTwo, doneThree, doneFour, doneFive;

    private void Start()
    {
        isCounting = true;
        countDown = 0;
        countdownVisual.sprite = countDownImages[countDown];
        countdownVisual.gameObject.SetActive(false);
        audio = GetComponent<AudioSource>();
    }

    //handles countdown timer by changing between 4 sprites
    private void Update()
    {
        if (isCounting == true)
        {
            countdownVisual.sprite = countDownImages[countDown];
            timer += Time.deltaTime;
            if (timer >= 1.0f && timer <= 1.99f && doneOne == false)
            {
                countdownVisual.gameObject.SetActive(true);
                countDown = 0;
                audio.pitch = Random.Range(0.8f, 1);
                audio.Play();
                doneOne = true;

            }
            else if (timer >= 2.0f && timer <= 2.99f && doneTwo == false)
            {
                countDown = 1;
                audio.pitch = Random.Range(0.8f, 1);
                audio.Play();
                doneTwo = true;
            }
            else if (timer >= 3.0f && timer <= 3.99f && doneThree == false)
            {
                countDown = 2;
                audio.pitch = Random.Range(0.8f, 1);
                audio.Play();
                doneThree = true;
            }
            else if (timer >= 4.0f && timer <= 5.0f && doneFour == false)
            {
                countDown = 3;
                audio.pitch = Random.Range(0.8f, 1);
                audio.Play();
                doneFour = true;
            }
            else if (timer >= 5.1f && doneFive == false)
            {
                countdownVisual.gameObject.SetActive(false);
                ManageGame.instance.StartTimer();
                isCounting = false;
                soundManager = SoundManager.Instance;
                soundManager.StartSong();
                //audio.Play();
                doneFour = true;
                //soundManager.AudioSource.Play();
            }          
        }
    }

    
}
