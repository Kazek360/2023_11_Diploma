using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IImmaterial
{
    public void SetPartnerEntity(IHittable partner);


    public void OnPartnerDeath();
}
