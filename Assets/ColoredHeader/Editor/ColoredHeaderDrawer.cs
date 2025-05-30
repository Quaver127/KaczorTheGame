using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ColoredHeaderAttribute))]
public class ColoredHeaderDrawer : DecoratorDrawer
{
    public override void OnGUI(Rect position)
    {
        ColoredHeaderAttribute attr = (ColoredHeaderAttribute)attribute;
        position.y += attr.topSpacing;

        if (attr.useBackground)
        {
            EditorGUI.DrawRect(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), attr.backgroundColor);
        }

        var style = new GUIStyle(EditorStyles.boldLabel);
        if (attr.useTextColor)
            style.normal.textColor = attr.textColor;

        EditorGUI.LabelField(position, attr.header, style);
    }

    public override float GetHeight()
    {
        ColoredHeaderAttribute attr = (ColoredHeaderAttribute)attribute;
        return attr.topSpacing + EditorGUIUtility.singleLineHeight + attr.bottomSpacing;
    }
}
