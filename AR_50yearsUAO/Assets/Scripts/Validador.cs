using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class Validador : MonoBehaviour
{
    public TextMeshProUGUI texto;
    void Start()
    {
        StartCoroutine(CheckARCoreCompatibility());
    }

    IEnumerator CheckARCoreCompatibility()
    {
        if (ARSession.state == ARSessionState.None || ARSession.state == ARSessionState.CheckingAvailability)
        {
            yield return ARSession.CheckAvailability();
        }
        if (ARSession.state == ARSessionState.Unsupported)
        {
            Debug.Log("ARCore no es compatible con este dispositivo.");
            texto.text = "ARCore no es compatible con este dispositivo.";
        }
        else if (ARSession.state == ARSessionState.NeedsInstall)
        {
            Debug.Log("ARCore necesita ser instalado.");
            texto.text = "ARCore necesita ser instalado.";
            yield return ARSession.Install();
        }
        else if (ARSession.state != ARSessionState.Ready)
        {
            Debug.Log("ARCore es compatible y está listo para usarse.");
            texto.text = "ARCore es compatible y está listo para usarse.";
        }
        else
        {
            Debug.Log("No se pudo determinar la compatibilidad de ARCore.");
            texto.text = "No se pudo determinar la compatibilidad de ARCore.";
        }
    }
}
