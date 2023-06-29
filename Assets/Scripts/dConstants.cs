using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class dConstants
{
    public static class UI
    {
        public const float StandardizedBtnAnimDuration = 0.3f;
    }
    public static class VFX
    {
        public const float CallbackAnimationDelayAfterInit = 3f;
        public const float CallbackAnimationDelayAfterPlay = 0.2f;
        public const float NumberShiftAnimInterval = 0.05f;
        public const int NumberShiftAnimCount = 0;
    }

    public enum GameTheme { Clock = 1, Coin = 2};
}
