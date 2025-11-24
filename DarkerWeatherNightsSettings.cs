using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Mutagen.Bethesda.Synthesis.Settings;

namespace DarkerWeatherNights;

public sealed class DarkerWeatherNightsSettings
{
    [SynthesisSettingName("Weather Selection")]
    [DefaultValue(WeatherInclusionMode.Vanilla)]
    [SynthesisTooltip("Choose whether to adjust every weather or vanilla only (recommended). Mods like LUX add custom weathers that are not supposed to be altered.")]
    public WeatherInclusionMode WeatherInclusion { get; set; } = WeatherInclusionMode.Vanilla;

    [SynthesisSettingName("PNAM - Cloud Colors (Night)")]
    public PNAMSettings PNAM { get; set; } = new();

    [SynthesisSettingName("NAM0 - Weather Colors (Night)")]
    public Nam0Settings NAM0 { get; set; } = new();

    [SynthesisSettingName("DALC - Directional Ambient Lighting Colors (Night)")]
    public DALCSettings DALC { get; set; } = new();

    [SynthesisSettingName("HNAM - Volumetric Lighting (Night)")]
    public HNAMSettings HNAM { get; set; } = new();

    public void Validate()
    {
        PNAM.Validate(nameof(PNAM));
        NAM0.Validate(nameof(NAM0));
        DALC.Validate(nameof(DALC));
        HNAM.Validate(nameof(HNAM));
    }

    private static void ValidateRange(double value, string name)
    {
        if (value < 0d || value > 1d)
        {
            throw new ValidationException($"{name} must be between 0 and 1 (inclusive).");
        }
    }

    public sealed class PNAMSettings
    {
        [Range(0, 1)]
        [DefaultValue(0.50)]
        [SynthesisSettingName("Cloud Colors")]
        [SynthesisTooltip("Cloud layer color scale; lowers the albedo of dynamic clouds at night (align with Sky Statics).")]
        public double NightMultiplier { get; set; } = 0.50;

        public void Validate(string prefix)
        {
            ValidateRange(NightMultiplier, $"{prefix}.{nameof(NightMultiplier)}");
        }
    }

    public sealed class Nam0Settings
    {
        [Range(0, 1)]
        [DefaultValue(0.50)]
        [SynthesisSettingName("Sky-Upper")]
        [SynthesisTooltip("Darkens the top of the skydome and softens the faint night air-glow.")]
        public double SkyUpperNightMultiplier { get; set; } = 0.50;

        [Range(0, 1)]
        [DefaultValue(0.50)]
        [SynthesisSettingName("Fog Near")]
        [SynthesisTooltip("Scales local atmospheric haze when near fog is active (align with Fog Far).")]
        public double FogNearNightMultiplier { get; set; } = 0.50;

        [Range(0, 1)]
        [DefaultValue(0.50)]
        [SynthesisSettingName("Ambient")]
        [SynthesisTooltip("Global non-directional base light outdoors; keep in balance with Sunlight.")]
        public double AmbientNightMultiplier { get; set; } = 0.50;

        [Range(0, 1)]
        [DefaultValue(0.50)]
        [SynthesisSettingName("Sunlight")]
        [SynthesisTooltip("Night directional light (moonlight); multiplied by the image-space sun scale.")]
        public double SunlightNightMultiplier { get; set; } = 0.50;

        [Range(0, 1)]
        [DefaultValue(0.50)]
        [SynthesisSettingName("Sky-Lower")]
        [SynthesisTooltip("Lower sky color near the horizon (align with Horizon and Fog Far).")]
        public double SkyLowerNightMultiplier { get; set; } = 0.50;

        [Range(0, 1)]
        [DefaultValue(0.50)]
        [SynthesisSettingName("Horizon")]
        [SynthesisTooltip("Thin horizon ring that sets the skyline seam color (align with Sky Lower/Fog Far).")]
        public double HorizonNightMultiplier { get; set; } = 0.50;

        [Range(0, 1)]
        [DefaultValue(1.0)]
        [SynthesisSettingName("Effect Lighting")]
        [SynthesisTooltip("Weather-driven emissive tint for windows, glow-mapped surfaces, and region effects.")]
        public double EffectLightingNightMultiplier { get; set; } = 1.0;

        [Range(0, 1)]
        [DefaultValue(0.50)]
        [SynthesisSettingName("Fog Far")]
        [SynthesisTooltip("Distant haze/terrain silhouette tint that also affects water reflections.")]
        public double FogFarNightMultiplier { get; set; } = 0.50;

        [Range(0, 1)]
        [DefaultValue(0.50)]
        [SynthesisSettingName("Sky Statics")]
        [SynthesisTooltip("Mountain-hugging statics/fogs around the horizon; keep in sync with PNAM.")]
        public double SkyStaticsNightMultiplier { get; set; } = 0.50;

        [Range(0, 1)]
        [DefaultValue(0.50)]
        [SynthesisSettingName("Water Multiplier")]
        [SynthesisTooltip("Night water-surface brightness/reflectance (align with Sky Lower/Horizon/Fog Far).")]
        public double WaterMultiplierNightMultiplier { get; set; } = 0.50;

        [Range(0, 1)]
        [DefaultValue(0.50)]
        [SynthesisSettingName("Moon Glare")]
        [SynthesisTooltip("Visible halo and disc glare of Masser/Secunda (does not change moonlight cast).")]
        public double MoonGlareNightMultiplier { get; set; } = 0.50;

        public void Validate(string prefix)
        {
            ValidateRange(SkyUpperNightMultiplier, $"{prefix}.{nameof(SkyUpperNightMultiplier)}");
            ValidateRange(FogNearNightMultiplier, $"{prefix}.{nameof(FogNearNightMultiplier)}");
            ValidateRange(AmbientNightMultiplier, $"{prefix}.{nameof(AmbientNightMultiplier)}");
            ValidateRange(SunlightNightMultiplier, $"{prefix}.{nameof(SunlightNightMultiplier)}");
            ValidateRange(SkyLowerNightMultiplier, $"{prefix}.{nameof(SkyLowerNightMultiplier)}");
            ValidateRange(HorizonNightMultiplier, $"{prefix}.{nameof(HorizonNightMultiplier)}");
            ValidateRange(EffectLightingNightMultiplier, $"{prefix}.{nameof(EffectLightingNightMultiplier)}");
            ValidateRange(FogFarNightMultiplier, $"{prefix}.{nameof(FogFarNightMultiplier)}");
            ValidateRange(SkyStaticsNightMultiplier, $"{prefix}.{nameof(SkyStaticsNightMultiplier)}");
            ValidateRange(WaterMultiplierNightMultiplier, $"{prefix}.{nameof(WaterMultiplierNightMultiplier)}");
            ValidateRange(MoonGlareNightMultiplier, $"{prefix}.{nameof(MoonGlareNightMultiplier)}");
        }
    }

    public sealed class DALCSettings
    {
        [Range(0, 1)]
        [DefaultValue(0.50)]
        [SynthesisSettingName("Directional X/Y")]
        [SynthesisTooltip("Directional ambient on vertical faces; lowering prevents bright side-lit walls.")]
        public double SidesNightMultiplier { get; set; } = 0.50;

        [Range(0, 1)]
        [DefaultValue(0.50)]
        [SynthesisSettingName("Directional Z-")]
        [SynthesisTooltip("Downward sky ambient; lowering darkens upward-facing surfaces.")]
        public double ZNegativeNightMultiplier { get; set; } = 0.50;

        [Range(0, 1)]
        [DefaultValue(0.50)]
        [SynthesisSettingName("Directional Z+")]
        [SynthesisTooltip("Upward ground-bounce ambient; lowering deepens undersides and overhangs.")]
        public double ZPositiveNightMultiplier { get; set; } = 0.50;

        [Range(0, 1)]
        [DefaultValue(0.50)]
        [SynthesisSettingName("Specular")]
        [SynthesisTooltip("Ambient specular component; lowering cuts moonlit glints on wet stone/metal.")]
        public double SpecularNightMultiplier { get; set; } = 0.50;

        public void Validate(string prefix)
        {
            ValidateRange(SidesNightMultiplier, $"{prefix}.{nameof(SidesNightMultiplier)}");
            ValidateRange(ZNegativeNightMultiplier, $"{prefix}.{nameof(ZNegativeNightMultiplier)}");
            ValidateRange(ZPositiveNightMultiplier, $"{prefix}.{nameof(ZPositiveNightMultiplier)}");
            ValidateRange(SpecularNightMultiplier, $"{prefix}.{nameof(SpecularNightMultiplier)}");
        }
    }

    public sealed class HNAMSettings
    {
        [Range(0, 1)]
        [DefaultValue(0.50)]
        [SynthesisSettingName("Volumetric Lighting Colors")]
        [SynthesisTooltip("Colors used by the referenced VOLI record at night.")]
        public double VolumetricLightingNightMultiplier { get; set; } = 0.50;

        public void Validate(string prefix)
        {
            ValidateRange(VolumetricLightingNightMultiplier, $"{prefix}.{nameof(VolumetricLightingNightMultiplier)}");
        }
    }

    public enum WeatherInclusionMode
    {
        [Description("All weathers")]
        All,

        [Description("Skyrim + DLCs")]
        Vanilla,
    }
}
