using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemScript : MonoBehaviour
{
    public enum ApplyOn
    {
        None,
        Patient,
        Ward
    }

    public ApplyOn toBeAppliedOn;
}
