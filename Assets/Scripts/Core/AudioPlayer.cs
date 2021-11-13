using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    private static List<AudioClip> m_Clips;
    private static AudioSource m_Source;
    private void Awake()
    {
        m_Source = GetComponent<AudioSource>();
        m_Clips = Resources.LoadAll<AudioClip>("").ToList();
    }

    public static void Play(string clipName)
    {
        var clip = m_Clips.Find(x => string.Equals(x.name, clipName, StringComparison.CurrentCultureIgnoreCase));
        
        if(clip != null)
            m_Source.PlayOneShot(clip);
    }
}
