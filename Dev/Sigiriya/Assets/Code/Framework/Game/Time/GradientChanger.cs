﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradientChanger : ManagerBase<GradientChanger>
{
    [SerializeField] private Image topGradient = null;
    [SerializeField] private Image bottomGradient = null;
    [Space]
    [SerializeField] [Range(0, 1)] private float masterOpacity = 1.0f;
    [Space]
    [SerializeField] private Color topSunrise = Color.white;
    [SerializeField] private Color bottomSunrise = Color.white;
    [SerializeField] [Range(0, 1)] private float sunriseOpacity = 1.0f;
    [Space]
    [SerializeField] private Color topMorning = Color.white;
    [SerializeField] private Color bottomMorning = Color.white;
    [SerializeField] [Range(0, 1)] private float morningOpacity = 1.0f;
    [Space]
    [SerializeField] private Color topMidday = Color.white;
    [SerializeField] private Color bottomMidday = Color.white;
    [SerializeField] [Range(0, 1)] private float middayOpacity = 1.0f;
    [Space]
    [SerializeField] private Color topEvening = Color.white;
    [SerializeField] private Color bottomEvening = Color.white;
    [SerializeField] [Range(0, 1)] private float eveningOpacity = 1.0f;
    [Space]
    [SerializeField] private Color topNight = Color.white;
    [SerializeField] private Color bottomNight = Color.white;
    [SerializeField] [Range(0, 1)] private float nightOpacity = 1.0f;
    [Space]
    [SerializeField] private EnumDisplayTime previewTime = EnumDisplayTime.SUNRISE;
    [Space]
    [SerializeField] [Range(0, 0.99f)] private float transitionAlpha = 0.0f;

    private Dictionary<EnumDisplayTime, ColorData> timeColors = new Dictionary<EnumDisplayTime, ColorData>();

    private Color topStartColor, topEndColor;
    private Color bottomStartColor, bottomEndColor;
    [Range(0, 1)] private float startOpacity, endOpacity;

    private Color topColor, bottomColor;

    private GlobalTimeTracker timeTracker;
    private float scaledAlpha;


    private void OnEnable()
    {
        timeTracker = GlobalTimeTracker.Instance;

        EventAnnouncer.OnTimeTickUpdated += UpdateBackground;
        EventAnnouncer.OnDayIsStarting += UpdateBackground;

        timeColors.Add(EnumDisplayTime.SIZE, new ColorData(topSunrise, bottomSunrise, sunriseOpacity));
        timeColors.Add(EnumDisplayTime.SUNRISE, new ColorData(topSunrise, bottomSunrise, sunriseOpacity));
        timeColors.Add(EnumDisplayTime.MORNING, new ColorData(topMorning, bottomMorning, morningOpacity));
        timeColors.Add(EnumDisplayTime.MIDDAY, new ColorData(topMidday, bottomMidday, middayOpacity));
        timeColors.Add(EnumDisplayTime.EVENING, new ColorData(topEvening, bottomEvening, eveningOpacity));
        timeColors.Add(EnumDisplayTime.NIGHT, new ColorData(topNight, bottomNight, nightOpacity));

        UpdateBackground(timeTracker.GlobalTime);
    }

    private void OnDisable()
    {
        EventAnnouncer.OnTimeTickUpdated -= UpdateBackground;
        EventAnnouncer.OnDayIsStarting -= UpdateBackground;
    }

    private void OnValidate()
    {
        ChangeGradient(previewTime);
    }

    //private void Update()
    //{
        //LerpColors();
    //}

    private void LerpColors(float lerp)
    {
        topColor = Color.Lerp(topStartColor, topEndColor, lerp);
        topColor.a *= Mathf.Lerp(startOpacity, endOpacity, lerp);

        bottomColor = Color.Lerp(bottomStartColor, bottomEndColor, lerp);
        bottomColor.a *= Mathf.Lerp(startOpacity, endOpacity, lerp);

        topColor.a *= masterOpacity;
        bottomColor.a *= masterOpacity;

        topGradient.color = topColor;
        bottomGradient.color = bottomColor;
    }

    private void UpdateBackground()
    {
        float temp = transitionAlpha;
        transitionAlpha = 0.0f;
        UpdateBackground(GlobalTimeTracker.Instance.GlobalTime);
        transitionAlpha = temp;
    }

    private void UpdateBackground(SigiTime globalTime)
    {
        if (timeColors.TryGetValue(timeTracker.ExternalTimeOfDay, out ColorData currentColorData))
        {
            if (timeColors.TryGetValue((EnumDisplayTime)(((int)timeTracker.ExternalTimeOfDay + 1) % (int)EnumDisplayTime.SIZE), out ColorData nextColorData))
            {
                topStartColor = currentColorData.TopColor;
                topEndColor = nextColorData.TopColor;

                bottomStartColor = currentColorData.BottomColor;
                bottomEndColor = nextColorData.BottomColor;

                startOpacity = currentColorData.Opacity;
                endOpacity = nextColorData.Opacity;

                previewTime = timeTracker.ExternalTimeOfDay;
            }
        }

        //Debug.Log(timeTracker.TimeAlpha + " | " + (timeTracker.TimeAlpha - transitionAlpha) / (1.0f - transitionAlpha));

        if (timeTracker.TimeAlpha >= transitionAlpha)
        {
            scaledAlpha = (timeTracker.TimeAlpha - transitionAlpha) / (1.0f - transitionAlpha);
            EventAnnouncer.OnTimeTransitioning?.Invoke(timeTracker.ExternalTimeOfDay, timeTracker.NextExternalTimeOfDay, scaledAlpha);
            LerpColors(scaledAlpha);
        }
    }

    public void ChangeGradient(EnumDisplayTime time)
    {
        topColor = topGradient.color;
        bottomColor = bottomGradient.color;

        switch (time)
        {
            default:
            case EnumDisplayTime.SUNRISE:
                ChangeColor(ref topColor, topSunrise, ref bottomColor, bottomSunrise, sunriseOpacity);
                break;
            case EnumDisplayTime.MORNING:
                ChangeColor(ref topColor, topMorning, ref bottomColor, bottomMorning, morningOpacity);
                break;
            case EnumDisplayTime.MIDDAY:
                ChangeColor(ref topColor, topMidday, ref bottomColor, bottomMidday, middayOpacity);
                break;
            case EnumDisplayTime.EVENING:
                ChangeColor(ref topColor, topEvening, ref bottomColor, bottomEvening, eveningOpacity);
                break;
            case EnumDisplayTime.NIGHT:
                ChangeColor(ref topColor, topNight, ref bottomColor, bottomNight, nightOpacity);
                break;
        }

        topColor.a *= masterOpacity;
        bottomColor.a *= masterOpacity;

        topGradient.color = topColor;
        bottomGradient.color = bottomColor;
    }

    private void ChangeColor(ref Color topColor, Color newTopColor, 
        ref Color bottomColor, Color newBottomColor, float opacity)
    {
        topColor = newTopColor;
        bottomColor = newBottomColor;
        topColor.a *= opacity;
        bottomColor.a *= opacity;
    }

    private struct ColorData
    {
        public Color TopColor;
        public Color BottomColor;
        [Range(0, 1)] public float Opacity;

        public ColorData(Color topColor, Color bottomColor, float opacity)
        {
            TopColor = topColor;
            BottomColor = bottomColor;
            Opacity = opacity;
        }
    }
}
