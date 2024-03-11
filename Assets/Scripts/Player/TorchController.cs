using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchController : MonoBehaviour
{
    [SerializeField] InputReader _inputReader;
    [SerializeField] private Light _torch;
    [SerializeField] private Light _sceneLight;

    private void Awake()
    {
        _inputReader.OnTorchToggle += ToggleTorch;
    }

    private void Start()
    {
        _torch.enabled = true;
        _sceneLight.enabled = false;
    }

    private void ToggleTorch()
    {
        _torch.enabled = !_torch.enabled;
        _sceneLight.enabled = !_sceneLight.enabled;
    }
}
