shader_type canvas_item;

uniform float speed = 0.15;
uniform float scale = 8.0;
uniform float brightness = 1.5;

uniform float color_speed = 0.3;

uniform vec4 color_a : source_color = vec4(0.1, 0.5, 1.0, 1.0);
uniform vec4 color_b : source_color = vec4(0.1, 1.0, 0.8, 1.0);
uniform vec4 background_color : source_color = vec4(0.01, 0.05, 0.08, 1.0);


void fragment()
{
    vec2 uv = UV;

    // Center coordinates.
    vec2 p = uv - 0.5;

    // Stretch slightly vertically.
    p.y *= 1.2;

    p *= scale;

    float t = TIME * speed;


    // --------------------------------------------------------
    // Caustic waves
    // --------------------------------------------------------

    float wave1 = sin(
        p.x * 2.0 +
        sin(p.y * 1.7 + t) +
        t
    );

    float wave2 = sin(
        p.y * 2.4 +
        sin(p.x * 1.4 - t * 1.2) +
        t * 0.8
    );

    float wave3 = sin(
        (p.x + p.y) * 2.0 +
        sin(p.x * 1.5 + t) +
        t * 1.3
    );

    float wave4 = sin(
        (p.x - p.y) * 2.2 +
        sin(p.y * 1.3 - t) -
        t
    );


    // Combine waves.
    float pattern =
        wave1 +
        wave2 +
        wave3 +
        wave4;

    pattern *= 0.25;


    // Create bright caustic lines.
    pattern = abs(pattern);

    pattern = 1.0 - pattern;

    pattern = pow(
        clamp(pattern, 0.0, 1.0),
        3.0
    );


    // --------------------------------------------------------
    // Edge falloff
    // --------------------------------------------------------

    float distance_from_center = length(uv - 0.5);

    float falloff = 1.0 -
        smoothstep(0.2, 0.75, distance_from_center);

    pattern *= mix(0.7, 1.0, falloff);


    // --------------------------------------------------------
    // Animated colour
    // --------------------------------------------------------

    float color_mix = sin(TIME * color_speed) * 0.5 + 0.5;

    vec3 caustic_color = mix(
        color_a.rgb,
        color_b.rgb,
        color_mix
    );


    // --------------------------------------------------------
    // Final colour
    // --------------------------------------------------------

    vec3 color = mix(
        background_color.rgb,
        caustic_color,
        pattern * brightness
    );

    COLOR = vec4(color, 0.5);
}