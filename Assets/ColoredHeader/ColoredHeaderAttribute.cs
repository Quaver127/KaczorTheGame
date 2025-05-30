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

   
    public ColoredHeaderAttribute(string header, float r, float g, float b, float topSpacing = 5f, float bottomSpacing = 5f)
    {
        this.header = header;
        this.textColor = new Color(r, g, b);
        this.useTextColor = true;
        this.useBackground = false;
        this.topSpacing = topSpacing;
        this.bottomSpacing = bottomSpacing;
    }

    
    public ColoredHeaderAttribute(string header, float r, float g, float b, float br, float bg, float bb, float topSpacing = 5f, float bottomSpacing = 5f)
    {
        this.header = header;
        this.textColor = new Color(r, g, b);
        this.backgroundColor = new Color(br, bg, bb);
        this.useTextColor = true;
        this.useBackground = true;
        this.topSpacing = topSpacing;
        this.bottomSpacing = bottomSpacing;
    }

    
    public ColoredHeaderAttribute(string header, float br, float bg, float bb, bool backgroundOnly, float topSpacing = 5f, float bottomSpacing = 5f)
    {
        this.header = header;
        this.backgroundColor = new Color(br, bg, bb);
        this.useTextColor = false;
        this.useBackground = true;
        this.topSpacing = topSpacing;
        this.bottomSpacing = bottomSpacing;
    }
}