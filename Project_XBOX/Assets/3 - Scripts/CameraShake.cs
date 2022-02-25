using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2D CAMERA SHAKE
public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Camera cam;

    private void Awake()
    {
        Instance = this;
        cam = Camera.main;
    }

    public void Shake(float duration, float intensity) => StartCoroutine(IShake(duration, intensity));
    IEnumerator IShake(float duration, float intensity)
    {
        Quaternion originalRotation = cam.transform.localRotation;

        float cpt = 0;

        while (cpt <= duration)
        {
            float x = Random.Range(-intensity, intensity);
            float y = Random.Range(-intensity, intensity);
            float z = Random.Range(-intensity, intensity);

            cam.transform.localRotation = Quaternion.Euler(x, y, z);

            cpt += Time.deltaTime;
            yield return null;
        }

        cam.transform.localRotation = originalRotation;
    }

}