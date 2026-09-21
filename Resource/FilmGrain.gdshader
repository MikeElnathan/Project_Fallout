shader_type canvas_item;

// ------------------------------------------------------------
// Screen
// ------------------------------------------------------------

uniform sampler2D screen_texture :
    hint_screen_texture,
    repeat_disable,
    filter_linear;


// ------------------------------------------------------------
// Film Grain Noise
// ------------------------------------------------------------

uniform sampler2D noise_texture :
    repeat_enable,
    filter_nearest;

uniform float grain_strength : hint_range(0.0, 0.2) = 0.02;

uniform float grain_scale : hint_range(0.1, 20.0) = 1.0;

uniform float grain_speed : hint_range(0.0, 10.0) = 1.0;


// ------------------------------------------------------------
// Fragment
// ------------------------------------------------------------

void fragment()
{
    // Current rendered frame.
    vec4 screen = texture(
        screen_texture,
        SCREEN_UV
    );

    // Animated noise coordinates.
    vec2 noise_uv =
        SCREEN_UV * grain_scale;

    noise_uv += vec2(
        TIME * grain_speed,
        TIME * grain_speed * 0.73
    );

    // Sample noise texture.
    float noise = texture(
        noise_texture,
        noise_uv
    ).r;

    // Convert 0...1 to -1...1.
    noise = noise * 2.0 - 1.0;

    // Apply grain.
    screen.rgb +=
        noise * grain_strength;

    COLOR = screen;
}