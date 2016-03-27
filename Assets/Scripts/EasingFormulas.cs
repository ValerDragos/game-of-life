using UnityEngine;
using System.Collections;

public static class EasingFormulas {

	public enum Type {Quadratic, Cubic, Quartic, Quantic, Sinusoidal, Exponential, Circular}

	public static readonly EasingType QuadraticEasing = new QuadraticEasing();
	public static readonly EasingType CubicEasing = new CubicEasing();
	public static readonly EasingType QuarticEasing = new QuarticEasing();
	public static readonly EasingType QuanticEasing = new QuanticEasing();
	public static readonly EasingType SinusoidalEasing = new SinusoidalEasing();
	public static readonly EasingType ExponentialEasing = new ExponentialEasing();
	public static readonly EasingType CircularEasing = new CircularEasing();

	public static EasingType.Formula GetFormula (Type type, EasingType.Easing easing)
	{
		switch (type)
		{
		case Type.Quadratic:
			return QuadraticEasing.GetFormula(easing);
		case Type.Cubic:
			return CubicEasing.GetFormula(easing);
		case Type.Quartic:
			return QuarticEasing.GetFormula(easing);
		case Type.Quantic:
			return QuanticEasing.GetFormula(easing);
		case Type.Sinusoidal:
			return SinusoidalEasing.GetFormula(easing);
		case Type.Exponential:
			return ExponentialEasing.GetFormula(easing);
		case Type.Circular:
			return CircularEasing.GetFormula(easing);
		default:
			return null;
		}
	}
}

public abstract class EasingType
{
	public enum Easing {In, Out, InOut }
	public delegate float Formula (float factor);

	public Formula GetFormula (Easing easing)
	{
		switch (easing)
		{
		case Easing.In:
			return In;
		case Easing.Out:
			return Out;
		case Easing.InOut:
			return InOut;
		default:
			return null;
		}
	}

	public abstract float In (float factor);
	public abstract float Out (float factor);
	public abstract float InOut (float factor);
}

public class QuadraticEasing : EasingType
{
	public override float In (float factor)
	{
		return factor * factor;
	}
	public override float Out (float factor)
	{
		return -factor*(factor-2);
	}
	public override float InOut (float factor)
	{
		//factor /= 2;
		//if (factor < 1) return c/2*t*t + b;
		//factor--;
		//return -c/2 * (t*(t-2) - 1) + b;
		return factor;
	}
}
public class CubicEasing : EasingType
{
	public override float In (float factor)
	{
		return factor;
	}
	public override float Out (float factor)
	{
		return factor;
	}
	public override float InOut (float factor)
	{
		return factor;
	}
}
public class QuarticEasing : EasingType
{
	public override float In (float factor)
	{
		return factor;
	}
	public override float Out (float factor)
	{
		return factor;
	}
	public override float InOut (float factor)
	{
		return factor;
	}
}

public class QuanticEasing : EasingType
{
	public override float In (float factor)
	{
		return factor;
	}
	public override float Out (float factor)
	{
		return factor;
	}
	public override float InOut (float factor)
	{
		return factor;
	}
}
public class SinusoidalEasing : EasingType
{
	public override float In (float factor)
	{
		return factor;
	}
	public override float Out (float factor)
	{
		return factor;
	}
	public override float InOut (float factor)
	{
		return factor;
	}
}
public class ExponentialEasing : EasingType
{
	public override float In (float factor)
	{
		return factor;
	}
	public override float Out (float factor)
	{
		return factor;
	}
	public override float InOut (float factor)
	{
		return factor;
	}
}
public class CircularEasing : EasingType
{
	public override float In (float factor)
	{
		return factor;
	}
	public override float Out (float factor)
	{
		return factor;
	}
	public override float InOut (float factor)
	{
		return factor;
	}
}

