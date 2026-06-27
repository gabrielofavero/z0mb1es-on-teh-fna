using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace ZombiesWP7;

[DebuggerNonUserCode]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[CompilerGenerated]
public class Strings
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			if (object.ReferenceEquals(resourceMan, null))
			{
				ResourceManager resourceManager = new ResourceManager("ZombiesWP7.Strings", typeof(Strings).Assembly);
				resourceMan = resourceManager;
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	public static string Achievements => ResourceManager.GetString("Achievements", resourceCulture);

	public static string AchieveUpsell => ResourceManager.GetString("AchieveUpsell", resourceCulture);

	public static string AchieveUpsellText1 => ResourceManager.GetString("AchieveUpsellText1", resourceCulture);

	public static string AchieveUpsellText2 => ResourceManager.GetString("AchieveUpsellText2", resourceCulture);

	public static string AchieveUpsellText3 => ResourceManager.GetString("AchieveUpsellText3", resourceCulture);

	public static string AchieveUpsellText4 => ResourceManager.GetString("AchieveUpsellText4", resourceCulture);

	public static string AchieveUpsellText5 => ResourceManager.GetString("AchieveUpsellText5", resourceCulture);

	public static string AchieveUpsellText6 => ResourceManager.GetString("AchieveUpsellText6", resourceCulture);

	public static string AchieveUpsellTitle => ResourceManager.GetString("AchieveUpsellTitle", resourceCulture);

	public static string Credits5 => ResourceManager.GetString("Credits5", resourceCulture);

	public static string Language => ResourceManager.GetString("Language", resourceCulture);

	internal Strings()
	{
	}
}
