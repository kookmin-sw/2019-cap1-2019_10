using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneTest : MonoBehaviour
{
    BackendManager backendManager;

    private void Start()
    {
        backendManager = GetComponent<BackendManager>();
    }

    public void OnClicked()
    {
        backendManager.GetAllResult("hi");
    }
}
