shader_type spatial;
render_mode unshaded, fog_disabled;


// SCREEN
uniform sampler2D screen_texture
	: source_color, hint_screen_texture, filter_linear;


// MASTER
uniform float intensity
	: hint_range(0.0, 1.0, 0.01) = 1.0;


// CONTRAST / COLOR
uniform float contrast
	: hint_range(0.0, 2.0, 0.01) = 0.30;

uniform float saturation
	: hint_range(0.0, 2.0, 0.01) = 0.10;

uniform float exposure
	: hint_range(-2.0, 2.0, 0.01) = 0.0;


// SPLIT TONING
uniform float shadow_tint_strength
	: hint_range(0.0, 1.0, 0.01) = 0.15;

uniform float highlight_tint_strength
	: hint_range(0.0, 1.0, 0.01) = 0.08;

uniform vec3 shadow_tint = vec3(
	0.82,
	0.90,
	1.00
);

uniform vec3 highlight_tint = vec3(
	1.00,
	0.94,
	0.82
);


// CHROMATIC ABERRATION
uniform float chromatic_aberration
	: hint_range(0.0, 0.02, 0.0001) = 0.0015;


// VIGNETTE
uniform float vignette_strength
	: hint_range(0.0, 1.0, 0.01) = 0.25;

uniform float vignette_roundness
	: hint_range(0.1, 2.0, 0.01) = 0.75;


// SHARPEN
uniform float sharpen_strength
	: hint_range(0.0, 1.0, 0.01) = 0.15;


// FILM DIRT
uniform float dirt_strength
	: hint_range(0.0, 1.0, 0.01) = 0.01;


// FULLSCREEN
void vertex()
{
	POSITION = vec4(
		VERTEX.xy,
		1.0,
		1.0
	);
}


// LUMINANCE
float luminance(vec3 color)
{
	return dot(
		color,
		vec3(
			0.299,
			0.587,
			0.114
		)
	);
}


// SATURATION
vec3 apply_saturation(
	vec3 color,
	float amount
)
{
	float luma =
		luminance(color);

	return mix(
		vec3(luma),
		color,
		1.0 + amount
	);
}


// SHARPEN
vec3 sharpen(
	vec2 uv,
	vec2 pixel_size,
	vec3 center
)
{
	vec3 blur =
		vec3(0.0);


	blur += texture(
		screen_texture,
		uv +
		vec2(
			pixel_size.x,
			0.0
		)
	).rgb;


	blur += texture(
		screen_texture,
		uv -
		vec2(
			pixel_size.x,
			0.0
		)
	).rgb;


	blur += texture(
		screen_texture,
		uv +
		vec2(
			0.0,
			pixel_size.y
		)
	).rgb;


	blur += texture(
		screen_texture,
		uv -
		vec2(
			0.0,
			pixel_size.y
		)
	).rgb;


	blur *= 0.25;


	return center +
		(
			center -
			blur
		) *
		sharpen_strength;
}


// FRAGMENT
void fragment()
{
	vec2 uv =
		SCREEN_UV;

	vec2 pixel_size =
		1.0 / VIEWPORT_SIZE;


	// --------------------------------
	// CHROMATIC ABERRATION
	// --------------------------------

	vec2 centered =
		uv -
		vec2(0.5);


	float distance_from_center =
		length(centered);


	vec2 aberration_offset =
		normalize(
			centered +
			vec2(0.00001)
		) *
		chromatic_aberration *
		distance_from_center;


	float red =
		texture(
			screen_texture,
			uv + aberration_offset
		).r;


	float green =
		texture(
			screen_texture,
			uv
		).g;


	float blue =
		texture(
			screen_texture,
			uv - aberration_offset
		).b;


	vec3 color =
		vec3(
			red,
			green,
			blue
		);


	// --------------------------------
	// EXPOSURE
	// --------------------------------

	color *= pow(
		2.0,
		exposure
	);


	// --------------------------------
	// CONTRAST
	// --------------------------------

	float luma =
		luminance(color);


	color =
		mix(
			vec3(luma),
			color,
			1.0 + contrast
		);


	color =
		clamp(
			color,
			0.0,
			1.0
		);


	// --------------------------------
	// SATURATION
	// --------------------------------

	color =
		apply_saturation(
			color,
			saturation
		);


	// --------------------------------
	// SPLIT TONING
	// --------------------------------

	float shadow_mask =
		1.0 -
		smoothstep(
			0.20,
			0.65,
			luma
		);


	float highlight_mask =
		smoothstep(
			0.45,
			0.90,
			luma
		);


	color =
		mix(
			color,
			color * shadow_tint,
			shadow_mask *
			shadow_tint_strength
		);


	color =
		mix(
			color,
			color * highlight_tint,
			highlight_mask *
			highlight_tint_strength
		);


	// --------------------------------
	// SHARPEN
	// --------------------------------

	color =
		sharpen(
			uv,
			pixel_size,
			color
		);


	// --------------------------------
	// FILM DIRT
	// --------------------------------

	float dirt_a =
		fract(
			sin(
				dot(
					floor(
						uv * 137.0
					),
					vec2(
						12.9898,
						78.233
					)
				)
			) *
			43758.5453
		);


	float dirt_b =
		fract(
			sin(
				dot(
					floor(
						uv * 61.0 +
						vec2(
							31.0,
							17.0
						)
					),
					vec2(
						12.9898,
						78.233
					)
				)
			) *
			43758.5453
		);


	float dirt =
		step(
			0.985,
			dirt_a * dirt_b
		);


	color *=
		1.0 -
		dirt *
		dirt_strength;


	// --------------------------------
	// VIGNETTE
	// --------------------------------

	float vignette =
		1.0 -
		smoothstep(
			0.25,
			0.85,
			length(centered) *
			vignette_roundness
		);


	color *=
		mix(
			1.0 -
			vignette_strength,
			1.0,
			vignette
		);


	// --------------------------------
	// MASTER INTENSITY
	// --------------------------------

	vec3 original =
		texture(
			screen_texture,
			uv
		).rgb;


	color =
		mix(
			original,
			color,
			intensity
		);


	// --------------------------------
	// OUTPUT
	// --------------------------------

	ALBEDO =
		clamp(
			color,
			0.0,
			1.0
		);
}