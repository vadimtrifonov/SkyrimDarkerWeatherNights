using System;
using System.Collections.Generic;
using System.Drawing;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Order;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;

namespace DarkerWeatherNights;

internal sealed class WeatherProcessor
{
    private readonly IPatcherState<ISkyrimMod, ISkyrimModGetter> _state;
    private readonly DarkerWeatherNightsSettings _settings;
    private readonly HashSet<FormKey> _scaledVolumetricLights = new();

    private int _colorChanges;
    private int _weatherChanges;

    public WeatherProcessor(
        IPatcherState<ISkyrimMod, ISkyrimModGetter> state,
        DarkerWeatherNightsSettings settings)
    {
        _state = state;
        _settings = settings;
    }

    public void Run()
    {
        foreach (var weatherGetter in _state.LoadOrder.PriorityOrder.Weather().WinningOverrides())
        {
            Weather? patchWeather = null;
            Weather EnsurePatchWeather()
            {
                patchWeather ??= _state.PatchMod.Weathers.GetOrAddAsOverride(weatherGetter);
                return patchWeather;
            }

            var beforeColors = _colorChanges;
            var weatherRecordChanged = false;
            weatherRecordChanged |= ScaleCloudLayers(weatherGetter, EnsurePatchWeather);
            weatherRecordChanged |= ScaleNam0WeatherColors(weatherGetter, EnsurePatchWeather);
            weatherRecordChanged |= ScaleDirectionalAmbientLighting(weatherGetter, EnsurePatchWeather);
            var volumetricChanged = ScaleVolumetricLighting(weatherGetter);

            if (weatherRecordChanged || volumetricChanged || beforeColors != _colorChanges)
            {
                _weatherChanges++;
            }
        }

        Console.WriteLine(
            $"Darker Weather Nights: Weather records changed: {_weatherChanges}. Color fields adjusted: {_colorChanges}.");
    }

    private bool ScaleCloudLayers(IWeatherGetter source, Func<Weather> ensureDestination)
    {
        var srcClouds = source.Clouds;
        var changed = false;

        for (var i = 0; i < srcClouds.Count; i++)
        {
            var srcLayer = srcClouds[i];
            if (srcLayer?.Colors == null)
            {
                continue;
            }

            var index = i;
            changed |= ScaleWeatherColorNight(
                srcLayer.Colors,
                () =>
                {
                    var dstClouds = ensureDestination().Clouds;
                    if (index >= dstClouds.Length)
                    {
                        return null;
                    }

                    return dstClouds[index]?.Colors;
                },
                _settings.PNAM.NightMultiplier);
        }

        return changed;
    }

    private bool ScaleNam0WeatherColors(IWeatherGetter source, Func<Weather> ensureDestination)
    {
        var changed = false;

        changed |= ScaleWeatherColorNight(
            source.SkyUpperColor,
            () => ensureDestination().SkyUpperColor,
            _settings.NAM0.SkyUpperNightMultiplier);
        changed |= ScaleWeatherColorNight(
            source.FogNearColor,
            () => ensureDestination().FogNearColor,
            _settings.NAM0.FogNearNightMultiplier);
        changed |= ScaleWeatherColorNight(
            source.AmbientColor,
            () => ensureDestination().AmbientColor,
            _settings.NAM0.AmbientNightMultiplier);
        changed |= ScaleWeatherColorNight(
            source.SunlightColor,
            () => ensureDestination().SunlightColor,
            _settings.NAM0.SunlightNightMultiplier);
        changed |= ScaleWeatherColorNight(
            source.SkyLowerColor,
            () => ensureDestination().SkyLowerColor,
            _settings.NAM0.SkyLowerNightMultiplier);
        changed |= ScaleWeatherColorNight(
            source.HorizonColor,
            () => ensureDestination().HorizonColor,
            _settings.NAM0.HorizonNightMultiplier);
        changed |= ScaleWeatherColorNight(
            source.EffectLightingColor,
            () => ensureDestination().EffectLightingColor,
            _settings.NAM0.EffectLightingNightMultiplier);
        changed |= ScaleWeatherColorNight(
            source.FogFarColor,
            () => ensureDestination().FogFarColor,
            _settings.NAM0.FogFarNightMultiplier);
        changed |= ScaleWeatherColorNight(
            source.SkyStaticsColor,
            () => ensureDestination().SkyStaticsColor,
            _settings.NAM0.SkyStaticsNightMultiplier);
        changed |= ScaleWeatherColorNight(
            source.WaterMultiplierColor,
            () => ensureDestination().WaterMultiplierColor,
            _settings.NAM0.WaterMultiplierNightMultiplier);
        changed |= ScaleWeatherColorNight(
            source.MoonGlareColor,
            () => ensureDestination().MoonGlareColor,
            _settings.NAM0.MoonGlareNightMultiplier);

        return changed;
    }

    private bool ScaleDirectionalAmbientLighting(IWeatherGetter source, Func<Weather> ensureDestination)
    {
        var srcColors = source.DirectionalAmbientLightingColors;
        var srcNight = srcColors?.Night;
        if (srcNight == null)
        {
            return false;
        }

        var changed = false;
        changed |= ScaleColorByFactor(
            srcNight.DirectionalXPlus,
            _settings.DALC.SidesNightMultiplier,
            color =>
            {
                var dstNight = ensureDestination().DirectionalAmbientLightingColors?.Night;
                if (dstNight == null)
                {
                    return false;
                }

                dstNight.DirectionalXPlus = color;
                return true;
            });
        changed |= ScaleColorByFactor(
            srcNight.DirectionalXMinus,
            _settings.DALC.SidesNightMultiplier,
            color =>
            {
                var dstNight = ensureDestination().DirectionalAmbientLightingColors?.Night;
                if (dstNight == null)
                {
                    return false;
                }

                dstNight.DirectionalXMinus = color;
                return true;
            });
        changed |= ScaleColorByFactor(
            srcNight.DirectionalYPlus,
            _settings.DALC.SidesNightMultiplier,
            color =>
            {
                var dstNight = ensureDestination().DirectionalAmbientLightingColors?.Night;
                if (dstNight == null)
                {
                    return false;
                }

                dstNight.DirectionalYPlus = color;
                return true;
            });
        changed |= ScaleColorByFactor(
            srcNight.DirectionalYMinus,
            _settings.DALC.SidesNightMultiplier,
            color =>
            {
                var dstNight = ensureDestination().DirectionalAmbientLightingColors?.Night;
                if (dstNight == null)
                {
                    return false;
                }

                dstNight.DirectionalYMinus = color;
                return true;
            });
        changed |= ScaleColorByFactor(
            srcNight.DirectionalZMinus,
            _settings.DALC.ZNegativeNightMultiplier,
            color =>
            {
                var dstNight = ensureDestination().DirectionalAmbientLightingColors?.Night;
                if (dstNight == null)
                {
                    return false;
                }

                dstNight.DirectionalZMinus = color;
                return true;
            });
        changed |= ScaleColorByFactor(
            srcNight.DirectionalZPlus,
            _settings.DALC.ZPositiveNightMultiplier,
            color =>
            {
                var dstNight = ensureDestination().DirectionalAmbientLightingColors?.Night;
                if (dstNight == null)
                {
                    return false;
                }

                dstNight.DirectionalZPlus = color;
                return true;
            });
        changed |= ScaleColorByFactor(
            srcNight.Specular,
            _settings.DALC.SpecularNightMultiplier,
            color =>
            {
                var dstNight = ensureDestination().DirectionalAmbientLightingColors?.Night;
                if (dstNight == null)
                {
                    return false;
                }

                dstNight.Specular = color;
                return true;
            });

        return changed;
    }

    private bool ScaleVolumetricLighting(IWeatherGetter source)
    {
        var nightLink = source.VolumetricLighting?.Night;
        if (nightLink == null)
        {
            return false;
        }

        if (_scaledVolumetricLights.Contains(nightLink.FormKey))
        {
            return false;
        }

        var baseline = nightLink.TryResolve(_state.LinkCache);
        if (baseline == null)
        {
            return false;
        }

        if (!TryGetVolumetricColorChannels(baseline, out var r, out var g, out var b))
        {
            return false;
        }

        if (IsAchromatic(r, g, b))
        {
            return false;
        }

        var scaledR = ScaleVolumetricChannel(r, _settings.HNAM.VolumetricLightingNightMultiplier);
        var scaledG = ScaleVolumetricChannel(g, _settings.HNAM.VolumetricLightingNightMultiplier);
        var scaledB = ScaleVolumetricChannel(b, _settings.HNAM.VolumetricLightingNightMultiplier);

        if (AreVolumetricChannelsEqual(scaledR, r) &&
            AreVolumetricChannelsEqual(scaledG, g) &&
            AreVolumetricChannelsEqual(scaledB, b))
        {
            return false;
        }

        var overrideRecord = _state.PatchMod.VolumetricLightings.GetOrAddAsOverride(baseline);
        overrideRecord.ColorR = scaledR;
        overrideRecord.ColorG = scaledG;
        overrideRecord.ColorB = scaledB;

        _scaledVolumetricLights.Add(nightLink.FormKey);
        _colorChanges++;
        return true;
    }

    private bool ScaleWeatherColorNight(
        IWeatherColorGetter? source,
        Func<WeatherColor?> getDestination,
        double multiplier)
    {
        if (source == null)
        {
            return false;
        }

        return ScaleColorByFactor(
            source.Night,
            multiplier,
            color =>
            {
                var destination = getDestination();
                if (destination == null)
                {
                    return false;
                }

                destination.Night = color;
                return true;
            });
    }

    private bool ScaleColorByFactor(Color baseline, double multiplier, Func<Color, bool> apply)
    {
        if (!HasColorData(baseline))
        {
            return false;
        }

        var scaled = ScaleColor(baseline, multiplier);
        if (ColorsEqual(scaled, baseline))
        {
            return false;
        }

        if (!apply(scaled))
        {
            return false;
        }

        _colorChanges++;
        return true;
    }

    private static Color ScaleColor(Color baseline, double multiplier)
    {
        var r = ScaleChannel(baseline.R, multiplier);
        var g = ScaleChannel(baseline.G, multiplier);
        var b = ScaleChannel(baseline.B, multiplier);
        return Color.FromArgb(baseline.A, r, g, b);
    }

    private static byte ScaleChannel(byte value, double multiplier)
    {
        var scaled = value * multiplier;
        return ClampByte(scaled);
    }

    private static byte ClampByte(double value)
    {
        return (byte)Math.Clamp((int)Math.Round(value, MidpointRounding.AwayFromZero), 0, 255);
    }

    private static bool HasColorData(Color color)
    {
        if (color.IsEmpty)
        {
            return false;
        }

        var isBlack = color.R == 0 && color.G == 0 && color.B == 0;
        var isWhite = color.R == 255 && color.G == 255 && color.B == 255;
        return !(isBlack || isWhite);
    }

    private static bool ColorsEqual(Color left, Color right)
    {
        return left.ToArgb() == right.ToArgb();
    }

    private static bool TryGetVolumetricColorChannels(
        IVolumetricLightingGetter source,
        out float r,
        out float g,
        out float b)
    {
        if (!source.ColorR.HasValue || !source.ColorG.HasValue || !source.ColorB.HasValue)
        {
            r = g = b = 0f;
            return false;
        }

        r = source.ColorR.Value;
        g = source.ColorG.Value;
        b = source.ColorB.Value;
        return true;
    }

    private static bool IsAchromatic(float r, float g, float b)
    {
        return (IsApproximately(r, 0f) && IsApproximately(g, 0f) && IsApproximately(b, 0f)) ||
               (IsApproximately(r, 255f) && IsApproximately(g, 255f) && IsApproximately(b, 255f));
    }

    private static bool IsApproximately(float value, float target)
    {
        return Math.Abs(value - target) <= 0.01f;
    }

    private static float ScaleVolumetricChannel(float value, double multiplier)
    {
        var scaled = value * multiplier;
        return (float)Math.Clamp(scaled, 0f, 255f);
    }

    private static bool AreVolumetricChannelsEqual(float left, float right)
    {
        return IsApproximately(left, right);
    }
}
