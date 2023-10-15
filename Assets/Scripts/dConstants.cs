using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class dConstants
{
    public static class UI
    {
        public const float StandardizedBtnAnimDuration = 0.3f;
        public static Color DefaultColor_Black = new Color(.15f, .15f, .15f);
        public static Color DefaultColor_1st = new Color(1f, 1f, 1f);
        public static Color DefaultColor_2nd = new Color(.7f, .7f, .7f, 1f);
        public static Color DefaultColor_3rd = new Color(.4f, .4f, .4f, 1f);
        public static Color DefaultColor_4th = new Color(.25f, .25f, .25f, 1f);
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
        public enum GamePhase { Title = 0, Selector = 1, Level = 2 };
    }
    //below is deprecated
    public enum GameTheme { Clock = 1, Coin = 2};
}
