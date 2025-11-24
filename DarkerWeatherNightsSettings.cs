using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Mutagen.Bethesda.Synthesis.Settings;

namespace DarkerWeatherNights;

public sealed class DarkerWeatherNightsSettings
{
    [Range(0, 1)]
    [DefaultValue(0.50)]
    [SynthesisSettingName("PNAM - Cloud Colors (Night)")]
    [SynthesisTooltip("Cloud layer color scale; lowers the albedo of dynamic clouds at night (align with Sky Statics).")]
    public double PNAMNightMultiplier { get; set; } = 0.50;

    [Range(0, 1)]
    [DefaultValue(0.33)]
    [SynthesisSettingName("NAM0 - Sky-Upper (Night)")]
    [SynthesisTooltip("Darkens the top of the skydome and softens the faint night air-glow.")]
    public double NAM0SkyUpperNightMultiplier { get; set; } = 0.33;

    [Range(0, 1)]
    [DefaultValue(0.33)]
    [SynthesisSettingName("NAM0 - Fog Near (Night)")]
    [SynthesisTooltip("Scales local atmospheric haze when near fog is active (align with Fog Far).")]
    public double NAM0FogNearNightMultiplier { get; set; } = 0.33;

    [Range(0, 1)]
    [DefaultValue(0.25)]
    [SynthesisSettingName("NAM0 - Ambient (Night)")]
    [SynthesisTooltip("Global non-directional base light outdoors; keep in balance with Sunlight.")]
    public double NAM0AmbientNightMultiplier { get; set; } = 0.25;

    [Range(0, 1)]
    [DefaultValue(0.25)]
    [SynthesisSettingName("NAM0 - Sunlight (Night)")]
    [SynthesisTooltip("Night directional light (moonlight); multiplied by the image-space sun scale.")]
    public double NAM0SunlightNightMultiplier { get; set; } = 0.25;

    [Range(0, 1)]
    [DefaultValue(0.33)]
    [SynthesisSettingName("NAM0 - Sky-Lower (Night)")]
    [SynthesisTooltip("Lower sky color near the horizon (align with Horizon and Fog Far).")]
    public double NAM0SkyLowerNightMultiplier { get; set; } = 0.33;

    [Range(0, 1)]
    [DefaultValue(0.33)]
    [SynthesisSettingName("NAM0 - Horizon (Night)")]
    [SynthesisTooltip("Thin horizon ring that sets the skyline seam color (align with Sky Lower/Fog Far).")]
    public double NAM0HorizonNightMultiplier { get; set; } = 0.33;

    [Range(0, 1)]
    [DefaultValue(1.0)]
    [SynthesisSettingName("NAM0 - Effect Lighting (Night)")]
    [SynthesisTooltip("Weather-driven emissive tint for windows, glow-mapped surfaces, and region effects.")]
    public double NAM0EffectLightingNightMultiplier { get; set; } = 1.0;

    [Range(0, 1)]
    [DefaultValue(0.33)]
    [SynthesisSettingName("NAM0 - Fog Far (Night)")]
    [SynthesisTooltip("Distant haze/terrain silhouette tint that also affects water reflections.")]
    public double NAM0FogFarNightMultiplier { get; set; } = 0.33;

    [Range(0, 1)]
    [DefaultValue(0.50)]
    [SynthesisSettingName("NAM0 - Sky Statics (Night)")]
    [SynthesisTooltip("Mountain-hugging statics/fogs around the horizon; keep in sync with PNAM.")]
    public double NAM0SkyStaticsNightMultiplier { get; set; } = 0.50;

    [Range(0, 1)]
    [DefaultValue(0.50)]
    [SynthesisSettingName("NAM0 - Water Multiplier (Night)")]
    [SynthesisTooltip("Night water-surface brightness/reflectance (align with Sky Lower/Horizon/Fog Far).")]
    public double NAM0WaterMultiplierNightMultiplier { get; set; } = 0.50;

    [Range(0, 1)]
    [DefaultValue(0.50)]
    [SynthesisSettingName("NAM0 - Moon Glare (Night)")]
    [SynthesisTooltip("Visible halo and disc glare of Masser/Secunda (does not change moonlight cast).")]
    public double NAM0MoonGlareNightMultiplier { get; set; } = 0.50;

    [Range(0, 1)]
    [DefaultValue(0.50)]
    [SynthesisSettingName("DALC - Directional X/Y (Night)")]
    [SynthesisTooltip("Directional ambient on vertical faces; lowering prevents bright side-lit walls.")]
    public double DALCSidesNightMultiplier { get; set; } = 0.50;

    [Range(0, 1)]
    [DefaultValue(0.50)]
    [SynthesisSettingName("DALC - Directional Z- (Night)")]
    [SynthesisTooltip("Downward sky ambient; lowering darkens upward-facing surfaces.")]
    public double DALCZNegativeNightMultiplier { get; set; } = 0.50;

    [Range(0, 1)]
    [DefaultValue(0.50)]
    [SynthesisSettingName("DALC - Directional Z+ (Night)")]
    [SynthesisTooltip("Upward ground-bounce ambient; lowering deepens undersides and overhangs.")]
    public double DALCZPositiveNightMultiplier { get; set; } = 0.50;

    [Range(0, 1)]
    [DefaultValue(0.50)]
    [SynthesisSettingName("DALC - Specular (Night)")]
    [SynthesisTooltip("Ambient specular component; lowering cuts moonlit glints on wet stone/metal.")]
    public double DALCSpecularNightMultiplier { get; set; } = 0.50;

    [Range(0, 1)]
    [DefaultValue(0.10)]
    [SynthesisSettingName("HNAM - Volumetric Lighting (Night)")]
    [SynthesisTooltip("Colors used by the referenced VOLI record at night.")]
    public double HNAMVolumetricLightingNightMultiplier { get; set; } = 0.10;

    public void Validate()
    {
        ValidateRange(PNAMNightMultiplier, nameof(PNAMNightMultiplier));
        ValidateRange(NAM0SkyUpperNightMultiplier, nameof(NAM0SkyUpperNightMultiplier));
        ValidateRange(NAM0FogNearNightMultiplier, nameof(NAM0FogNearNightMultiplier));
        ValidateRange(NAM0AmbientNightMultiplier, nameof(NAM0AmbientNightMultiplier));
        ValidateRange(NAM0SunlightNightMultiplier, nameof(NAM0SunlightNightMultiplier));
        ValidateRange(NAM0SkyLowerNightMultiplier, nameof(NAM0SkyLowerNightMultiplier));
        ValidateRange(NAM0HorizonNightMultiplier, nameof(NAM0HorizonNightMultiplier));
        ValidateRange(NAM0EffectLightingNightMultiplier, nameof(NAM0EffectLightingNightMultiplier));
        ValidateRange(NAM0FogFarNightMultiplier, nameof(NAM0FogFarNightMultiplier));
        ValidateRange(NAM0SkyStaticsNightMultiplier, nameof(NAM0SkyStaticsNightMultiplier));
        ValidateRange(NAM0WaterMultiplierNightMultiplier, nameof(NAM0WaterMultiplierNightMultiplier));
        ValidateRange(NAM0MoonGlareNightMultiplier, nameof(NAM0MoonGlareNightMultiplier));
        ValidateRange(DALCSidesNightMultiplier, nameof(DALCSidesNightMultiplier));
        ValidateRange(DALCZNegativeNightMultiplier, nameof(DALCZNegativeNightMultiplier));
        ValidateRange(DALCZPositiveNightMultiplier, nameof(DALCZPositiveNightMultiplier));
        ValidateRange(DALCSpecularNightMultiplier, nameof(DALCSpecularNightMultiplier));
        ValidateRange(HNAMVolumetricLightingNightMultiplier, nameof(HNAMVolumetricLightingNightMultiplier));
    }

    private static void ValidateRange(double value, string name)
    {
        if (value < 0d || value > 1d)
        {
            throw new ValidationException($"{name} must be between 0 and 1 (inclusive).");
        }
    }
}
