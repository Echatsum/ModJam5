using UnityEngine;

namespace FifthModJam;
public class CustomItemSocket : OWItemSocket
{
    [SerializeField]
    public int desiredIndex;
    [SerializeField]
    public ItemType desiredType;
    public bool isActive;

    public override void Awake()
    {
        base.Awake();
        _acceptableType = desiredType;
    }

    public override bool PlaceIntoSocket(OWItem item)
    {
        if (base.PlaceIntoSocket(item) && item.GetComponent<CustomItem>().customItemIndex == desiredIndex)
        {
            isActive = true;
            return true;
        }
        isActive = false;
        return false;
    }

    public override OWItem RemoveFromSocket()
    {
        isActive = false;
        OWItem oWItem = base.RemoveFromSocket();
        return oWItem;
    }
}
