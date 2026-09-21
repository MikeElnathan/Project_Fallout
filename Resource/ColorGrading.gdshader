shader_type canvas_item;

uniform sampler2D screen_texture :
    hint_screen_texture,
    repeat_disable,
    filter_linear;

// Exposure
uniform float exposure : hint_range(-2.0, 2.0) = 0.0;

// Contrast
uniform float contrast : hint_range(0.0, 2.0) = 1.0;

// Saturation
uniform float saturation : hint_range(0.0, 2.0) = 1.0;

// Overall color tint
uniform vec3 tint : source_color = vec3(1.0);

// Shadow color
uniform vec3 shadow_tint : source_color = vec3(1.0);

// Highlight color
uniform vec3 highlight_tint : source_color = vec3(1.0);


void fragment()
{
    vec3 color = texture(
        screen_texture,
        SCREEN_UV
    ).rgb;


    // --------------------------------------------------------
    // Exposure
    // --------------------------------------------------------

    color *= pow(2.0, exposure);


    // --------------------------------------------------------
    // Contrast
    // --------------------------------------------------------

    color = (color - 0.5) * contrast + 0.5;


    // --------------------------------------------------------
    // Saturation
    // --------------------------------------------------------

    float luminance = dot(
        color,
        vec3(
            0.2126,
            0.7152,
            0.0722
        )
    );

    color = mix(
        vec3(luminance),
        color,
        saturation
    );


    // --------------------------------------------------------
    // Shadow / Highlight Tint
    // --------------------------------------------------------

    float shadow = 1.0 - smoothstep(
        0.0,
        0.5,
        luminance
    );

    float highlight = smoothstep(
        0.5,
        1.0,
        luminance
    );

    color = mix(
        color,
        color * shadow_tint,
        shadow * 0.15
    );

    color = mix(
        color,
        color * highlight_tint,
        highlight * 0.15
    );


    // --------------------------------------------------------
    // Overall Tint
    // --------------------------------------------------------

    color *= tint;


    COLOR = vec4(color, 1.0);
}