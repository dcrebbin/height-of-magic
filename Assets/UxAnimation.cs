using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class UxAnimation : MonoBehaviour
{

    public PostProcessVolume postProcessVolume;
    public DepthOfField depthOfField;

    // Start is called before the first frame update
    void Start()
    {

        // Add debug checks
        if (postProcessVolume == null)
        {
            Debug.LogError("PostProcessVolume component not found!");
            return;
        }

        if (postProcessVolume.profile == null)
        {
            Debug.LogError("No profile assigned to PostProcessVolume!");
            return;
        }

        if (postProcessVolume.profile.TryGetSettings<DepthOfField>(out var dof))
        {
            dof.focalLength.value = 215f;
            StartCoroutine(ReduceFocalLength(dof));
        }
        else
        {
            Debug.LogError("Depth of Field not found in post process profile! Make sure it's enabled in the profile.");
        }
    }

    IEnumerator ReduceFocalLength(DepthOfField dof)
    {
        while (dof.focalLength.value > 0)
        {
            Debug.Log("Reducing focal length: " + dof.focalLength.value);
            dof.focalLength.value -= Time.deltaTime * 140; // Adjust the speed as needed
            yield return null;
        }
    }
}
