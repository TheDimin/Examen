using System;
using System.Collections;
using System.Collections.Generic;
using Examen.Level;
using Examen.Managers;
using UnityEngine;

public class AudioManager : Singelton<AudioManager>
{
    private PlayerController pc;
    private Transform pcTransform;
    private AudioSource audioSource;

    private AudioClip OutSideClip;
    private AudioClip InsideAudioClip;

    public override void Awake()
    {
        pc = ((GameLevel)GameManager.Instance.level).pc;

        pcTransform = pc.transform;

        audioSource = pc.gameObject.AddComponent<AudioSource>();

        OutSideClip = Resources.Load<AudioClip>("Audio/horn002");
        InsideAudioClip = Resources.Load<AudioClip>("Audio/mechanical ring");
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }


    public IEnumerator PlayCode(string code,bool outside)
    {
        while (true)
        {
            foreach (var codeChar in code)
            {
                for (int i = 0; i < Int32.Parse(codeChar.ToString()); i++)
                {
                    AudioClip clip = outside ? OutSideClip : InsideAudioClip;
                    PlaySound(clip);
                    yield return new WaitForSeconds(clip.length);
                }
                yield return new WaitForSeconds(.5f);
            }

            yield return new WaitForSeconds(4);
        }

        // ReSharper disable once IteratorNeverReturns
    }
}
