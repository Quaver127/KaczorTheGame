using UnityEngine;

public class ColoredHeaderAttribute : PropertyAttribute
{
    public string header;
    public Color textColor = Color.white;
    public Color backgroundColor = Color.clear;
    public float topSpacing;
    public float bottomSpacing;
    public bool useBackground;
    public bool useTextColor;

    /// <summary>
    /// Constructor with text color only (hex).
    /// </summary>
    public ColoredHeaderAttribute(string header, string hexTextColor, float topSpacing = 5f, float bottomSpacing = 5f)
    {
        this.header = header;
        this.textColor = ParseHexColor(hexTextColor);
        this.useTextColor = true;
        this.useBackground = false;
        this.topSpacing = topSpacing;
        this.bottomSpacing = bottomSpacing;
    }

    /// <summary>
    /// Constructor with text and background color (hex).
    /// </summary>
    public ColoredHeaderAttribute(string header, string hexTextColor, string hexBackgroundColor, float topSpacing = 5f, float bottomSpacing = 5f)
    {
        this.header = header;
        this.textColor = ParseHexColor(hexTextColor);
        this.backgroundColor = ParseHexColor(hexBackgroundColor);
        this.useTextColor = true;
        this.useBackground = true;
        this.topSpacing = topSpacing;
        this.bottomSpacing = bottomSpacing;
    }

    /// <summary>
    /// Constructor with background color only (hex).
    /// </summary>
    public ColoredHeaderAttribute(string header, string hexBackgroundColor, bool backgroundOnly, float topSpacing = 5f, float bottomSpacing = 5f)
    {
        this.header = header;
        this.backgroundColor = ParseHexColor(hexBackgroundColor);
        this.useTextColor = false;
        this.useBackground = true;
        this.topSpacing = topSpacing;
        this.bottomSpacing = bottomSpacing;
    }

    /// <summary>
    /// Parses a hex color string (e.g., "#RRGGBB" or "#RRGGBBAA") into a Unity Color.
    /// </summary>
    private Color ParseHexColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out var color))
            return color;

        Debug.LogWarning($"Invalid color hex string: {hex}. Defaulting to Color.clear.");
        return Color.clear;
    }
}