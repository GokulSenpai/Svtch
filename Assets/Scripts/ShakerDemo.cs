using UnityEngine;
using MilkShake;

public class ShakerDemo : MonoBehaviour
{
    //Inspector field for the Shaker component.
    public Shaker MyShaker;
    //Inspector field for a Shake Preset to use as the shake parameters.
    public ShakePreset ShakePreset;

    private void Start()
    {
        //Shake using the shake preset's parameters.
        MyShaker.Shake(ShakePreset);
    }
}