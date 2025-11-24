
# Darker Weather Nights

Darker Weather Nights is a Synthesis patcher for Skyrim SE/AE/VR that darkens night-time weather by scaling every relevant color field. It reproduces the behavior of [Darker Weather Nights - xEdit Script](https://www.nexusmods.com/skyrimspecialedition/mods/164521).

## Problem it solves

Typically, “darker nights” mods uniformly reduce Image Space brightness, which makes not only the environment, but also light sources unnaturally dim. Some also lower Image Space sunlight scale, which controls moonlit lighting and does not apply to grass - creating a risk of visibly bright grass at night.

## How it works

It scales the RGB values in Night subtrees of the WTHR (Weather) and VOLI (Volumetric Lighting) records using configurable multipliers.

- PNAM - Cloud Colors (night): darker overcast/cloud layers.
- NAM0 - Weather Colors (night): sky upper/lower, horizon ring, ambient, sunlight (moonlight), fog near/far, effect lighting, sky statics, water brightness, moon glare.
- DALC - Directional Ambient Lighting Colors (night): lateral fill (sides), downward sky ambient (Z‑), upward ground bounce (Z+), ambient specular.
- HNAM - Volumetric Lighting (night reference colors).

It does not touch day/dawn/dusk values, image spaces, interior lighting, or non‑weather/volumetric lighting records.

## Settings

`DarkerWeatherNightsSettings.cs` exposes every multiplier via the Synthesis UI.

## Compatibility

- Compatible with most weather mods; tested with Azurite Weathers and Obsidian Weathers.
- Default settings change only vanilla weather records, avoiding custom weathers from mods like LUX.
