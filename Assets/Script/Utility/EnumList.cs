using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public enum SettingType
    {
        Music,
        Sfx,
        Vibration,
        Quality

    }
    public enum SceneName
    {
        Loading,
        MainMenu,
        Play,
        Level1,
        Level2,
        Level3,
        Level4,
        None
    }

    public enum ScreenType
    {
        MainMenu,
        HUD,
        Settings,
        Custom,
        Shop,
        Leaderboard,
        PopUpPotion,
        PopUpConfirmation,
        PopUpOptions,
        FloorTransition,
        Pause,
        ClickToPlay,
        GameOver,
        PopUpTransation,
        Ads,
        FTUE,
        Exchange,
        PopUpLoot,
        PopUpReward,
        None
    }
    public enum SimpleDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    public enum UIAnimation
    {
        None,
        Slide,
        PopUp
    }

    public enum AudioIdentifier
    {
        None

    }

    public enum PitchType
    {
        defaultPitch,
        tuned,
        random
    }

}
