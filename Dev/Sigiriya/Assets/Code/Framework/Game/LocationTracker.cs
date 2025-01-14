﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Responsible for keeping track of which location in the world we're in.
[RequireComponent(typeof(ParallaxController))]
public class LocationTracker : ManagerBase<LocationTracker>
{
    [SerializeField] private EnumLocation currentLocation = EnumLocation.HOME;
    public EnumLocation CurrentLocation { get { return currentLocation; } private set { currentLocation = value; } }

    public EnumLocation TargetLocation { get; private set; }
    private bool shouldFade;

    public ParallaxController BackgroundController { get; private set; } = null;

    [SerializeField] private RectTransform background = null;
    [SerializeField] private RectTransform midground = null;
    [SerializeField] private RectTransform foreground = null;

    private Dictionary<EnumLocation, LocationController> locationControllers = new Dictionary<EnumLocation, LocationController>();

    [Header("Location Fade")]
    [SerializeField] private Fade locationFadeRef = null;
    [SerializeField] private Button endOfDayButton = null;

    [SerializeField] private Parallax testBackground = null;

    private void OnValidate()
    {
        if (testBackground)
        {
            BackgroundController = GetComponent<ParallaxController>();
            BackgroundController.SetBackground(testBackground);
        }
    }

    private void Awake()
    {
        BackgroundController = GetComponent<ParallaxController>();
        ShowEndOfDayButton(false);
        CurrentLocation = currentLocation;
        TargetLocation = CurrentLocation;

        ChangeLocation(EnumLocation.SIZE, CurrentLocation, false);
    }

    private void OnEnable()
    {
        EventAnnouncer.OnRequestLocationChange += ChangeLocation;
        EventAnnouncer.OnEndFadeIn += GoToNextLocation;
        EventAnnouncer.OnRequestCharacterScheduleUpdate += CharacterScheduleUpdate;

        EventAnnouncer.OnDayIsStarting += StartOfDay;
    }

    private void OnDisable()
    {
        EventAnnouncer.OnRequestLocationChange -= ChangeLocation;
        EventAnnouncer.OnEndFadeIn -= GoToNextLocation;
        EventAnnouncer.OnRequestCharacterScheduleUpdate -= CharacterScheduleUpdate;

        EventAnnouncer.OnDayIsStarting -= StartOfDay;
    }

    public void RegisterLocation(EnumLocation locationKey, LocationController locationValue)
    {
        if (IsLocationRegistered(locationKey))
        {
            Debug.LogWarning("A location has already been registered with key: " + locationKey.ToString());
        }
        else
        {
            Debug.Log("Registering Location: " + locationKey.ToString());
            locationControllers.Add(locationKey, locationValue);
        }
    }

    private void ChangeLocation(EnumLocation prevLocation, EnumLocation targetLocation, bool fade)
    {
        if (locationControllers.ContainsKey(targetLocation))
        {
            TargetLocation = targetLocation;
            shouldFade = fade;

            if (shouldFade)
            {
                locationFadeRef.FadeIn("location_fade");
            }
            else
            {
                GoToNextLocation();
            }
        }
        else
        {
            Debug.Log("Location: " + targetLocation + " is not registered.");
        }
    }

    private void GoToNextLocation(string fadeID)
    {
        if (fadeID.CompareTo("location_fade") == 0)
        {
            GoToNextLocation();
        }
    }

    private void GoToNextLocation()
    {
        GetLocationController(CurrentLocation).gameObject.SetActive(false);
        CurrentLocation = TargetLocation;
        LocationController newLoc = GetLocationController(CurrentLocation);
        newLoc.gameObject.SetActive(true);
        SetObjectOrdering(newLoc);

        EventAnnouncer.OnArrivedAtLocation?.Invoke(CurrentLocation);

        if (shouldFade)
        {
            locationFadeRef.FadeOut("location_fade");
        }
        else
        {
            locationFadeRef.FadeOutNow();
        }
    }

    public bool IsLocationRegistered(EnumLocation location)
    {
        if (locationControllers.ContainsKey(location))
        {
            return true;
        }

        return false;
    }

    public LocationController GetLocationController(EnumLocation location)
    {
        if (locationControllers.TryGetValue(location, out LocationController locationController))
        {
            return locationController;
        }
        else
        {
            Debug.LogWarning("Could not find location registerd with key: " + location.ToString());
            return null;
        }
    }

    private void StartOfDay()
    {
        foreach (LocationController controller in locationControllers.Values)
        {
            controller.StartOfDay();
        }
    }

    private void SetObjectOrdering(LocationController locationController)
    {
        StartCoroutine(SortingOrderChanged(locationController));
    }

    private IEnumerator SortingOrderChanged(LocationController locationController)
    {
        yield return null;

        background.SetParent(locationController.BG);
        midground.SetParent(locationController.MG);
        foreground.SetParent(locationController.FG);

        background.SetAsFirstSibling();
        midground.SetAsFirstSibling();
        foreground.SetAsFirstSibling();
    }

    public void ShowEndOfDayButton(bool showButton)
    {
        endOfDayButton.gameObject.SetActive(showButton);
    }

    public void CharacterScheduleUpdate()
    {
        foreach (KeyValuePair<EnumLocation, LocationController> locationPair in locationControllers)
        {
            locationPair.Value.ResetCharacterSlots();
        }
    }
}

public enum EnumLocation : int
{
    KITCHEN,
    WEWA_MARSH,
    CONSTRUCTION_SITE,
    GATHERING_SPACE,
    FOREST_CLEARING,
    HOME,
    VILLAGE_CENTER,
    POTTING_YARD,
    SPRING,
    SIZE = 9,
    NOT_PRESENT
}
