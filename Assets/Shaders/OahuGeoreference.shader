Shader "Custom/OahuGeoreference"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        // DEM Properties
        _DEMTexture ("DEM Texture", 2D) = "black" {}
        _MinElevation ("Min Elevation (meters)", Float) = 0.0
        _MaxElevation ("Max Elevation (meters)", Float) = 1220.0  // Oahu's highest point
        _ElevationScale ("Elevation Scale Factor", Float) = 1.0
        _UseElevationCorrection ("Use Elevation Correction", Range(0,1)) = 1
        _PerspectiveCorrection ("Perspective Correction Strength", Range(0,2)) = 0.5
        
        // Geographic bounds of your Oahu map (from QGIS)
        _MinLat ("Min Latitude", Float) = 21.18
        _MaxLat ("Max Latitude", Float) = 21.7425
        _MinLon ("Min Longitude", Float) = -158.322
        _MaxLon ("Max Longitude", Float) = -157.602
        
        // Projection settings
        _UseMercator ("Use Mercator Projection", Range(0,1)) = 1
        
        // Debug/Visualization options
        _ShowGrid ("Show Coordinate Grid", Range(0,1)) = 0
        _GridColor ("Grid Color", Color) = (1,0,0,0.5)
        _GridSpacing ("Grid Spacing (degrees)", Float) = 0.1
        _GridWidth ("Grid Line Width", Range(0.001, 0.1)) = 0.02
        // DEM
        _ShowElevation ("Show Elevation Overlay", Range(0,1)) = 0
        _ElevationColorLow ("Low Elevation Color", Color) = (0,0,1,0.3)
        _ElevationColorHigh ("High Elevation Color", Color) = (1,1,1,0.7)
        
        // Target coordinate highlighting
        _TargetLat ("Target Latitude", Float) = 21.4667
        _TargetLon ("Target Longitude", Float) = -157.9667
        _HighlightRadius ("Highlight Radius (degrees)", Float) = 0.01
        _HighlightColor ("Highlight Color", Color) = (1,1,0,1)
        _ShowHighlight ("Show Highlight", Range(0,1)) = 0
        
        // Debug: Second target for comparison
        _Target2Lat ("Target 2 Latitude", Float) = 21.2644
        _Target2Lon ("Target 2 Longitude", Float) = -157.8071
        _Target2Color ("Target 2 Color", Color) = (0,1,1,1)
        _ShowTarget2 ("Show Target 2", Range(0,1)) = 0
        
        // Distance measurement
        _ShowDistance ("Show Distance Line", Range(0,1)) = 0
        _DistanceColor ("Distance Line Color", Color) = (0,1,0,1)
        _DistanceWidth ("Distance Line Width", Range(0.001, 0.01)) = 0.002
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float2 geoCoord : TEXCOORD1;  // lat/lon coordinates
                float2 correctedUV : TEXCOORD2;  // elevation-corrected UV
                float elevation : TEXCOORD3;    // elevation at this point
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            // DEM properties
            sampler2D _DEMTexture;
            float4 _DEMTexture_ST;
            float _MinElevation, _MaxElevation, _ElevationScale;
            float _UseElevationCorrection, _PerspectiveCorrection;
            float _ShowElevation;
            fixed4 _ElevationColorLow, _ElevationColorHigh;
            
            float _MinLat, _MaxLat, _MinLon, _MaxLon;
            float _UseMercator;
            float _ShowGrid, _GridSpacing, _GridWidth;
            fixed4 _GridColor;
            float _TargetLat, _TargetLon, _HighlightRadius, _ShowHighlight;
            fixed4 _HighlightColor;
            float _Target2Lat, _Target2Lon, _ShowTarget2;
            fixed4 _Target2Color;
            float _ShowDistance, _DistanceWidth;
            fixed4 _DistanceColor;

            // Sample elevation from DEM texture
            float SampleElevation(float2 uv)
            {
                float elevationSample = tex2D(_DEMTexture, uv).r;
                return lerp(_MinElevation, _MaxElevation, elevationSample) * _ElevationScale;
            }

            // Mercator projection functions
            float LatToMercatorY(float lat)
            {
                float radians = lat * 0.017453292519943295; // degrees to radians
                return log(tan(radians * 0.5 + 0.785398163397448)); // ln(tan(lat/2 + π/4))
            }
            
            float MercatorYToLat(float y)
            {
                return (2.0 * atan(exp(y)) - 1.5707963267948966) * 57.295779513082320877; // radians to degrees
            }
            
            // Haversine formula for great-circle distance (in kilometers)
            float HaversineDistance(float lat1, float lon1, float lat2, float lon2)
            {
                const float R = 6371.0; // Earth's radius in kilometers
                
                // Convert degrees to radians
                float lat1Rad = lat1 * 0.017453292519943295;
                float lon1Rad = lon1 * 0.017453292519943295;
                float lat2Rad = lat2 * 0.017453292519943295;
                float lon2Rad = lon2 * 0.017453292519943295;
                
                // Haversine formula
                float dlat = lat2Rad - lat1Rad;
                float dlon = lon2Rad - lon1Rad;
                
                float a = sin(dlat * 0.5) * sin(dlat * 0.5) + 
                         cos(lat1Rad) * cos(lat2Rad) * sin(dlon * 0.5) * sin(dlon * 0.5);
                float c = 2.0 * atan2(sqrt(a), sqrt(1.0 - a));
                
                return R * c; // Distance in kilometers
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color = v.color * _Color;

                // Pass through original UV for elevation sampling in fragment shader
                o.correctedUV = v.texcoord;
                o.elevation = 0.0; // Will be calculated in fragment shader
                
                float lon = lerp(_MinLon, _MaxLon, v.texcoord.x);
                
                if(_UseMercator > 0.5)
                {
                    // Mercator projection
                    float minMercY = LatToMercatorY(_MinLat);
                    float maxMercY = LatToMercatorY(_MaxLat);
                    // Since origin is northwest, UV.y=0 should map to MaxLat
                    float mercY = lerp(maxMercY, minMercY, 1.0 - v.texcoord.y);
                    float lat = MercatorYToLat(mercY);
                    o.geoCoord = float2(lon, lat);
                }
                else
                {
                    // Linear projection - UV.y=0 maps to MaxLat (north)
                    float lat = lerp(_MaxLat, _MinLat, 1.0 - v.texcoord.y);
                    o.geoCoord = float2(lon, lat);
                }
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Sample the original sprite texture
                // fixed4 texColor = tex2D(_MainTex, i.texcoord) * i.color;
                // fixed4 finalColor = texColor;

                // Sample elevation and calculate corrections in fragment shader
                float2 demUV = TRANSFORM_TEX(i.texcoord, _DEMTexture);
                float elevation = SampleElevation(demUV);
                
                // Calculate elevation-corrected UV coordinates
                float2 correctedUV = i.texcoord;
                
                if(_UseElevationCorrection > 0.5)
                {
                    // Apply perspective correction based on elevation
                    float elevationFactor = elevation / _MaxElevation;
                    float correctionStrength = _PerspectiveCorrection * (1.0 - elevationFactor);
                    
                    // Apply radial correction from center
                    float2 center = float2(0.5, 0.5);
                    float2 direction = correctedUV - center;
                    float distance = length(direction);
                    
                    // Apply correction that's stronger at edges and for lower elevations
                    correctedUV += direction * correctionStrength * distance * 0.1;
                }
                
                // Recalculate geographic coordinates with corrected UV
                float2 geoCoord = i.geoCoord;
                if(_UseElevationCorrection > 0.5)
                {
                    float lon = lerp(_MinLon, _MaxLon, correctedUV.x);
                    
                    if(_UseMercator > 0.5)
                    {
                        float minMercY = LatToMercatorY(_MinLat);
                        float maxMercY = LatToMercatorY(_MaxLat);
                        float mercY = lerp(maxMercY, minMercY, 1.0 - correctedUV.y);
                        float lat = MercatorYToLat(mercY);
                        geoCoord = float2(lon, lat);
                    }
                    else
                    {
                        float lat = lerp(_MaxLat, _MinLat, 1.0 - correctedUV.y);
                        geoCoord = float2(lon, lat);
                    }
                }
                
                // Sample the original sprite texture using corrected UV
                fixed4 texColor = tex2D(_MainTex, correctedUV) * i.color;
                fixed4 finalColor = texColor;
                
                // Elevation visualization overlay
                if(_ShowElevation > 0.5)
                {
                    float elevationNormalized = saturate(elevation / _MaxElevation);
                    fixed4 elevationColor = lerp(_ElevationColorLow, _ElevationColorHigh, elevationNormalized);
                    finalColor = lerp(finalColor, elevationColor, elevationColor.a);
                }
                
                // Grid visualization
                if(_ShowGrid > 0.5)
                {
                    float2 gridPos = i.geoCoord / _GridSpacing;
                    float2 gridFrac = frac(gridPos);
                    
                    // Create grid lines with adjustable width
                    float gridLine = 0.0;
                    
                    if(gridFrac.x < _GridWidth || gridFrac.x > (1.0 - _GridWidth) ||
                       gridFrac.y < _GridWidth || gridFrac.y > (1.0 - _GridWidth))
                    {
                        gridLine = 1.0;
                    }
                    
                    finalColor = lerp(finalColor, _GridColor, gridLine * _GridColor.a);
                }
                
                // Target coordinate highlighting using Haversine distance
                if(_ShowHighlight > 0.5)
                {
                    float distance = HaversineDistance(i.geoCoord.y, i.geoCoord.x, _TargetLat, _TargetLon);
                    float radiusKm = _HighlightRadius * 111.0; // Convert degrees to approximate km
                    
                    if(distance < radiusKm)
                    {
                        float intensity = 1.0 - (distance / radiusKm);
                        finalColor = lerp(finalColor, _HighlightColor, intensity * _HighlightColor.a);
                    }
                }
                
                // Second target using Haversine distance
                if(_ShowTarget2 > 0.5)
                {
                    float distance2 = HaversineDistance(i.geoCoord.y, i.geoCoord.x, _Target2Lat, _Target2Lon);
                    float radiusKm = _HighlightRadius * 111.0; // Convert degrees to approximate km
                    
                    if(distance2 < radiusKm)
                    {
                        float intensity2 = 1.0 - (distance2 / radiusKm);
                        finalColor = lerp(finalColor, _Target2Color, intensity2 * _Target2Color.a);
                    }
                }
                
                // Draw distance line between the two points
                if(_ShowDistance > 0.5)
                {
                    // Calculate distance from current pixel to the great circle line between targets
                    float2 target1 = float2(_TargetLon, _TargetLat);
                    float2 target2 = float2(_Target2Lon, _Target2Lat);
                    
                    // Simple line approximation (for small distances like Oahu, this works well)
                    float2 lineVec = target2 - target1;
                    float2 pointVec = i.geoCoord - target1;
                    
                    float t = saturate(dot(pointVec, lineVec) / dot(lineVec, lineVec));
                    float2 closestPoint = target1 + t * lineVec;
                    
                    float distToLine = HaversineDistance(i.geoCoord.y, i.geoCoord.x, closestPoint.y, closestPoint.x);
                    float lineWidthKm = _DistanceWidth * 111.0; // Convert to km
                    
                    if(distToLine < lineWidthKm)
                    {
                        float intensity = 1.0 - (distToLine / lineWidthKm);
                        finalColor = lerp(finalColor, _DistanceColor, intensity * _DistanceColor.a);
                    }
                }
                
                return finalColor;
            }
            ENDCG
        }
    }
}