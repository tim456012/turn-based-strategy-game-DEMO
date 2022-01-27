using System.Collections;
using UnityEngine;

/// <summary>
/// The components of GameObject. Indicate that it should or should not be placed in the object pools. 
/// </summary>
public class Poolable : MonoBehaviour
{
    public string key;
    public bool isPooled;
}
