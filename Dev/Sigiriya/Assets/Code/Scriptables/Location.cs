﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Can be used to create new Locations by designers or artists.
[CreateAssetMenu(fileName = "New Location", menuName = "Location")]
public class Location : ScriptableObject
{
    //A Location stores:
    //  - The location enum
    //  - A list of images based on flags

    public EnumLocation locationType;

    //I'm using strings for now because that's how the Flag system is set up,
    //but I think it would be cooler to have a bank of either enums or static final strings
    //somewhere that we choose from.
    //[Header("it will randomly chose one of the images to display)")]
    //[Header("(If you have two of the same modifiers,")]
    //[Header("Flags for the images")]
    //[Tooltip("When this flag is active, this image will be displayed as the location background.")]
    //public List<EnumLocationModifier> Keys = new List<EnumLocationModifier>();

    [Header("Images for the flags")]
    [Tooltip("Make sure the image is in the same index spot as the flag.")]
    public List<Parallax> Values = new List<Parallax>();

    public Dictionary<EnumLocationModifier, Parallax> BackgroundData = new Dictionary<EnumLocationModifier, Parallax>();

    public void OnEnable()
    {
        BackgroundData.Clear();

        for (int i = 0; i < Values.Count; ++i)
        {
            BackgroundData.Add(Values[i].locationModifier, Values[i]);
        }
    }
}
