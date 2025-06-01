using UnityEngine;

public enum ItemType
{
    HP, //체력 회복
    MP, //마나 회복
    TimeStop, //날아오는 탄막을 3초간 멈추게 함
    Shield, //3초간 무적 상태가 됨
    SpeedUp //이동 속도가 빨라짐
}

public static class ItemTypeExtensions
{
    public static float GetDuration(this ItemType type)
    {
        switch (type)
        {
            case ItemType.TimeStop:
            case ItemType.Shield:
                return 3f;
            case ItemType.SpeedUp:
                return 5f;
            case ItemType.HP:
            case ItemType.MP:
            default:
                return 0f; // 즉시 효과
        }
    }

    public static float GetCooldown(this ItemType type)
    {
        switch (type)
        {
            case ItemType.TimeStop:
                return 15f;
            case ItemType.Shield:
                return 20f;
            case ItemType.SpeedUp:
                return 10f;
            case ItemType.HP:
            case ItemType.MP:
                return 5f;
            default:
                return 10f;
        }
    }
}
