using System;

namespace MonoGameRubiksCube
{
    public class EasingFunction
    {
        public readonly string Name;
        public readonly Easing.EasingFn Apply;

        private EasingFunction(string name, Easing.EasingFn apply)
        {
            Name = name;
            Apply = apply;
        }

        //
        public static readonly EasingFunction[] All = 
        {
			new EasingFunction("Linear", Easing.Linear),
			new EasingFunction("QuadraticIn", Easing.QuadraticIn),
			new EasingFunction("QuadraticOut", Easing.QuadraticOut),
			new EasingFunction("QuadraticInOut", Easing.QuadraticInOut),
			new EasingFunction("CubicIn", Easing.CubicIn),
			new EasingFunction("CubicOut", Easing.CubicOut),
			new EasingFunction("CubicInOut", Easing.CubicInOut),
			new EasingFunction("QuartIn", Easing.QuartIn),
			new EasingFunction("QuartOut", Easing.QuartOut),
			new EasingFunction("QuartInOut", Easing.QuartInOut),
			new EasingFunction("QuintIn", Easing.QuintIn),
			new EasingFunction("QuintOut", Easing.QuintOut),
			new EasingFunction("QuintInOut", Easing.QuintInOut),
			new EasingFunction("SinusoidalIn", Easing.SinusoidalIn),
			new EasingFunction("SinusoidalOut", Easing.SinusoidalOut),
			new EasingFunction("SinusoidalInOut", Easing.SinusoidalInOut),
			new EasingFunction("ExponentialIn", Easing.ExponentialIn),
			new EasingFunction("ExponentialOut", Easing.ExponentialOut),
			new EasingFunction("ExponentialInOut", Easing.ExponentialInOut),
			new EasingFunction("CircularIn", Easing.CircularIn),
			new EasingFunction("CircularOut", Easing.CircularOut),
			new EasingFunction("CircularInOut", Easing.CircularInOut)
        };
    }
    public static class Easing
    {
        public delegate double EasingFn(double t, double b, double c, double d);

        public static double Linear(double t, double b, double c, double d)
        {
            return b + c * t / d;
        }

        public static double QuadraticIn(double t, double b, double c, double d)
        {
            t /= d;
            return c * t * t + b;
        }

        public static double QuadraticOut(double t, double b, double c, double d)
        {
            t /= d;
            return -c * t * (t - 2) + b;
        }

        public static double QuadraticInOut(double t, double b, double c, double d)
        {
            t /= (d / 2);
            if (t < 1)
            {
                return b + c / 2 * t * t;
            }
            t--;
            return b - c / 2 * (t * (t - 2) - 1);
        }

        public static double CubicIn(double t, double b, double c, double d)
        {
            t /= d;
            return c * t * t * t + b;
        }

        public static double CubicOut(double t, double b, double c, double d)
        {
            t /= d;
            t--;
            return c * (t * t * t + 1) + b;
        }

        public static double CubicInOut(double t, double b, double c, double d)
        {
            t /= d / 2;
            if (t < 1)
            {
                return c / 2 * t * t * t + b;
            }
            t -= 2;
            return c / 2 * (t * t * t + 2) + b;
        }


        public static double QuartIn(double t, double b, double c, double d)
        {
            t /= d;
            return c * t * t * t * t + b;
        }

        public static double QuartOut(double t, double b, double c, double d)
        {
            t /= d;
            t--;
            return -c * (t * t * t * t - 1) + b;
        }

        public static double QuartInOut(double t, double b, double c, double d)
        {
            t /= d / 2;
            if (t < 1)
            {
                return c / 2 * t * t * t * t + b;
            }
            t -= 2;
            return -c / 2 * (t * t * t * t - 2) + b;
        }

        public static double QuintIn(double t, double b, double c, double d)
        {
            t /= d;
            return c * t * t * t * t * t + b;
        }

        public static double QuintOut(double t, double b, double c, double d)
        {
            t /= d;
            t--;
            return c * (t * t * t * t * t + 1) + b;
        }

        public static double QuintInOut(double t, double b, double c, double d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t * t * t * t + b;
            t -= 2;
            return c / 2 * (t * t * t * t * t + 2) + b;
        }

        public static double SinusoidalIn(double t, double b, double c, double d)
        {
            return -c * Math.Cos(t / d * (Math.PI / 2)) + c + b;
        }
        public static double SinusoidalOut(double t, double b, double c, double d)
        {
            return c * Math.Sin(t / d * (Math.PI / 2)) + b;
        }
        public static double SinusoidalInOut(double t, double b, double c, double d)
        {
            return -c / 2 * (Math.Cos(Math.PI * t / d) - 1) + b;
        }

        public static double ExponentialIn(double t, double b, double c, double d)
        {
            return c * Math.Pow(2, 10 * (t / d - 1)) + b;
        }
        public static double ExponentialOut(double t, double b, double c, double d)
        {
            return c * (-Math.Pow(2, -10 * t / d) + 1) + b;
        }
        public static double ExponentialInOut(double t, double b, double c, double d)
        {
            t /= d / 2;
            if (t < 1)
            {
                return c / 2 * Math.Pow(2, 10 * (t - 1)) + b;
            }
            t--;
            return c / 2 * (-Math.Pow(2, -10 * t) + 2) + b;
        }

        public static double CircularIn(double t, double b, double c, double d)
        {
            t /= d;
            return -c * (Math.Sqrt(1 - t * t) - 1) + b;
        }
        public static double CircularOut(double t, double b, double c, double d)
        {
            t /= d;
            t--;
            return c * Math.Sqrt(1 - t * t) + b;
        }
        public static double CircularInOut(double t, double b, double c, double d)
        {
            t /= d / 2;
            if (t < 1)
            {
                return -c / 2 * (Math.Sqrt(1 - t * t) - 1) + b;
            }
            t -= 2;
            return c / 2 * (Math.Sqrt(1 - t * t) + 1) + b;
        }

        public static readonly EasingFn[] AllEasingFns = new EasingFn[]
        {
            Linear,
            QuadraticIn,
            QuadraticOut,
            QuadraticInOut,
            CubicIn,
            CubicOut,
            CubicInOut,
            QuartIn,
            QuartOut,
            QuartInOut,
            QuintIn,
            QuintOut,
            QuintInOut,
            SinusoidalIn,
            SinusoidalOut,
            SinusoidalInOut,
            ExponentialIn,
            ExponentialOut,
            ExponentialInOut,
            CircularIn,
            CircularOut,
            CircularInOut
        };
    }
}