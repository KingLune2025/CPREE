using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class measuredistance : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
public override int GetHashCode() => base.GetHashCode();

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public Transform vrheadset;
    public Transform militarydude;
    public Transform distanceText;
    public float triggerDistance = 1.0f;
    public float normalbreath = .2f;

    public measuredistance(float triggerDistance)
    {
        this.triggerDistance = triggerDistance;
    }
}

// Update is called once per frame
void Update()
    { if(vrHeadset == null || militarydude  == null || DistanceText == null)
        Debug.LogError("References are missing")
      { return; }


}
}
