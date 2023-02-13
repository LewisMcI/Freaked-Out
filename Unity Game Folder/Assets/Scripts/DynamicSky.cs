using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DynamicSky : MonoBehaviour
{
    #region fields
    [SerializeField]
    [Range(0.0f, 24.0f)]
    private float timeOfDay;
    [SerializeField]
    private float orbitSpeed, cloudSpeed;
    private float cloudRotation;

    [SerializeField]
    private Light sun, moon;
    [SerializeField]
    private Volume skyVolume;
    private PhysicallyBasedSky sky;
    [SerializeField]
    private AnimationCurve dayNightCurve;
    [SerializeField]
    private Cubemap daySkybox, nightSkybox;

    private bool isNight;
    #endregion

    #region properties

    #endregion

    #region methods
    private void Awake()
    {
        // Get sky
        skyVolume.sharedProfile.TryGet<PhysicallyBasedSky>(out sky);
    }

    private void OnValidate()
    {
        // Get sky
        skyVolume.sharedProfile.TryGet<PhysicallyBasedSky>(out sky);

        UpdateTime();
    }

    private void Update()
    {
        // Increase time of day
        timeOfDay += orbitSpeed * Time.deltaTime;
        // Reset time of day
        if (timeOfDay > 24.0f)
            timeOfDay = 0.0f;

        UpdateTime();
    }

    private void UpdateTime()
    {
        // Calculate time of day
        float alpha = timeOfDay / 24.0f;
        float sunRotation = Mathf.Lerp(-90.0f, 270.0f, alpha);
        float moonRotation = sunRotation - 180.0f;

        // Rotate sun & moon
        sun.transform.rotation = Quaternion.Euler(sunRotation, 270.0f, 0.0f);
        moon.transform.rotation = Quaternion.Euler(moonRotation, 270.0f, 0.0f);

        // Rotate clouds
        cloudRotation += cloudSpeed * Time.deltaTime;
        //sky.spaceRotation.value = new Vector3(0.0f, cloudRotation, 0.0f);

        // Set skybox emission
        sky.spaceEmissionMultiplier.value = dayNightCurve.Evaluate(alpha) * 500.0f;
        // Set light intesity
        sun.transform.GetChild(0).GetComponent<Light>().intensity = dayNightCurve.Evaluate(alpha) * 4000.0f;
        moon.transform.GetChild(0).GetComponent<Light>().intensity = dayNightCurve.Evaluate(alpha) * 50.0f;
        //sun.transform.GetChild(0).GetComponent<Light>().shadowStrength = dayNightCurve.Evaluate(alpha);
        //moon.transform.GetChild(0).GetComponent<Light>().shadowStrength = dayNightCurve.Evaluate(alpha);


        CheckNightDayTransition();
    }

    private void CheckNightDayTransition()
    {
        if (isNight)
        {
            if (moon.transform.rotation.eulerAngles.x > 180)
            {
                StartDay();
            }
        }
        else if (!isNight)
        {
            if (sun.transform.rotation.eulerAngles.x > 180)
            {
                StartNight();
            }
        }
    }

    private void StartDay()
    {
        // Set shadows
        //sun.transform.GetChild(0).GetComponent<Light>().shadows = LightShadows.Soft;
        //moon.transform.GetChild(0).GetComponent<Light>().shadows = LightShadows.None;

        sky.spaceEmissionTexture.value = daySkybox;

        isNight = false;
    }

    private void StartNight()
    {
        // Set shadows
        //sun.transform.GetChild(0).GetComponent<Light>().shadows = LightShadows.None;
        //moon.transform.GetChild(0).GetComponent<Light>().shadows = LightShadows.Soft;

        sky.spaceEmissionTexture.value = nightSkybox;

        isNight = true;
    }
    #endregion
}