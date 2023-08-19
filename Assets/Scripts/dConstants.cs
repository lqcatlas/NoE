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
        public const float CallbackAnimationDelayAfterInit = 4f;
        public const float CallbackAnimationDelayAfterPlay = 0.2f;
        public const float NumberShiftAnimInterval = 0.05f;
        public const int NumberShiftAnimCount = 0;
        public const float SelectorToLevelAnimTransitionPhase1 = 1.2f;
        public const float SelectorToLevelAnimTransitionPhase2 = 0.8f;
        public const float SelectorToLevelAnimTransitionOnHold = 0f;
    }
    public static class Gameplay
    {
        public const int DefaultThemeUnlockTokenRequirement = 6;
    }
    //below is deprecated
    public enum GameTheme { Clock = 1, Coin = 2};
}
