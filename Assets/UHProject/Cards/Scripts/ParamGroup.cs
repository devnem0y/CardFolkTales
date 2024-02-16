using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UralHedgehog;

public class ParamGroup : MonoBehaviour
{
    [SerializeField] private List<Marker> _markers;
    
    public void SetVisible(MarkerType type, bool visible)
    {
        GetMarker(type).gameObject.SetActive(visible);
    }

    public Marker GetMarker(MarkerType type)
    {
        return _markers.FirstOrDefault(marker => marker.Type == type);
    }
}