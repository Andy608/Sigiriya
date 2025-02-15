﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Can be used to create new Sound Effects by designers/sound designers.
[CreateAssetMenu(fileName = "New Sound Effect", menuName = "Sound Effect")]
public class SoundEffect : ScriptableObject
{
    public List<AudioClip> sounds = new List<AudioClip>();
    public EnumSound soundID;

    public AudioClip GetRandomSound()
    {
        return sounds[Random.Range(0, sounds.Count)];
    }
}

public enum EnumSound
{
    BUTTON_PRESS,
    DIALOGUE_ANNOYED,
    DIALOGUE_NEUTRAL,
    DIALOGUE_SMILE,
    DIALOGUE_SURPRISE,
    DIALOGUE_FLUSTERED,
    MUSIC_MAIN,
    MUSIC_MENU,
    TACT_INT_CHOP,
    TACT_INT_POUR,
    TACT_INT_SHAKE,
}
