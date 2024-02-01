using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanRemove : MonoBehaviour
{
    public ItemData ItemData;
    public void DestroyGO()
    {
        Destroy(this.gameObject);
    }
}
