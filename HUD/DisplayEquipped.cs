﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using Matcha.Game.Tweens;


public class DisplayEquipped : CacheBehaviour
{
    private SpriteRenderer HUDWeapon;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main.GetComponent<Camera>();
        PositionHUDElements();
    }

    void PositionHUDElements()
    {
        transform.position = mainCamera.ScreenToWorldPoint(new Vector3(
            Screen.width / 2,
            Screen.height - HUD_WEAPON_TOP_MARGIN,
            HUD_Z));
    }

    void OnInitEquippedWeapon(GameObject weapon)
    {
        HUDWeapon = spriteRenderer;
        HUDWeapon.sprite = weapon.GetComponent<Weapon>().sprite;
        HUDWeapon.DOKill();
        FadeInWeapon();
    }

    void OnChangeEquippedWeapon(GameObject weapon)
    {
        HUDWeapon = spriteRenderer;
        HUDWeapon.sprite = weapon.GetComponent<Weapon>().sprite;
        HUDWeapon.DOKill();
        MTween.FadeOut(HUDWeapon, 0, 0);
        MTween.FadeIn(HUDWeapon, 1f, 0f, HUD_WEAPON_CHANGE_FADE);
    }

    void FadeInWeapon()
    {
        // fade weapon to zero instantly, then fade up slowly
        MTween.FadeOut(HUDWeapon, 0, 0);
        MTween.FadeIn(HUDWeapon, HUD_FADE_IN_AFTER, HUD_INITIAL_TIME_TO_FADE);
    }

    void OnFadeHud(bool status)
    {
        MTween.FadeOut(HUDWeapon, HUD_FADE_OUT_AFTER, HUD_INITIAL_TIME_TO_FADE);
    }

    void OnScreenSizeChanged(float vExtent, float hExtent)
    {
        PositionHUDElements();
    }

    void OnEnable()
    {
        Messenger.AddListener<GameObject>("init equipped weapon", OnInitEquippedWeapon);
        Messenger.AddListener<GameObject>("change equipped weapon", OnChangeEquippedWeapon);
        Messenger.AddListener<bool>("fade hud", OnFadeHud);
        Messenger.AddListener<float, float>( "screen size changed", OnScreenSizeChanged);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener<GameObject>("init equipped weapon", OnInitEquippedWeapon);
        Messenger.RemoveListener<GameObject>("change equipped weapon", OnChangeEquippedWeapon);
        Messenger.RemoveListener<bool>("fade hud", OnFadeHud);
        Messenger.RemoveListener<float, float>( "screen size changed", OnScreenSizeChanged);
    }
}