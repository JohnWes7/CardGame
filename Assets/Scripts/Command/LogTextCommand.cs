using System;
using System.Collections.Generic;
using DG.Tweening;
using QFramework;
using UnityEngine;

public class LogDamageTextCommand : AbstractCommand<GameObject>
{
    public int DamageAmount { get; set; }
    public Vector3 Position { get; set; }
    public Color Color { get; set; }
    public float Duration { get; set; } = 1f;
    public float randomRadius { get; set; } = 1f;
    public bool ChangeFontSizeByAmount { get; set; } = true;

    protected override GameObject OnExecute()
    {
        float size = 12f;
        if (ChangeFontSizeByAmount)
        {
            size = Mathf.Clamp(12f + DamageAmount / 10f, 12f, 24f);
        }

        GameObject text = this.GetUtility<LogTextUtility>().LogText(DamageAmount.ToString(), Position + -Vector3.forward, Color, size, Duration);
        
        // 随机半径动画
        var randomV2 = UnityEngine.Random.insideUnitCircle;
        //text.transform.position += new Vector3(randomV2.x, randomV2.y) * randomRadius;
        text.transform.DOMove(Position + new Vector3(randomV2.x, randomV2.y) * randomRadius, 0.8f)
            .SetEase(Ease.OutExpo);

        return text;
    }

    public static Color GetRepairColor()
    {
        return new Color(0.1960784f, 0.8039216f, 0.1960784f, 1f);
    }

    public static Color GetRedColor()
    {
        return new Color(1f, 0.2705882f, 0f, 1f);
    }

    public static Color GetWhiteColor()
    {
        return new Color(1f, 1f, 1f, 1f);
    }
}

public class LogWarringTextOnMousePosCommand : AbstractCommand<GameObject>
{
    public string Text { get; set; }
    public Color Color { get; set; } = Color.white;
    public float Size { get; set; } = 12f;

    protected override GameObject OnExecute()
    {
        Vector3 createPosVector3 = Camera.main.ScreenToWorldPoint(Input.mousePosition) + 2 * Vector3.forward;

        Text = $"<shake a=0.05>{Text}</>";
        GameObject text = this.GetUtility<LogTextUtility>().LogText(Text, createPosVector3, Color, Size, 1f);

        return text;
    }
}