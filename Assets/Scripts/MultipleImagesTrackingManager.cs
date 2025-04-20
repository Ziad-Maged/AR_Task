using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleImagesTrackingManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] List<GameObject> trackedPrefabs = new();
    private ARTrackedImageManager trackedImageManager;
    private Dictionary<string, GameObject> arObjects;

    void Start()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        if (trackedImageManager == null)
        {
            Debug.LogError("ARTrackedImageManager component not found on this GameObject.");
            return;
        }
        trackedImageManager.trackablesChanged.AddListener(OnImagesTrackedChanged);
        arObjects = new Dictionary<string, GameObject>();
        SetupSceneEmementes();
    }

    private void OnDestroy()
    {
        trackedImageManager.trackablesChanged.RemoveListener(OnImagesTrackedChanged);
    }

    private void SetupSceneEmementes()
    {
        foreach(var prefab in trackedPrefabs)
        {
            var arObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            arObject.name = prefab.name;
            arObject.gameObject.SetActive(false);
            arObjects.Add(prefab.name, arObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnImagesTrackedChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            if (arObjects.TryGetValue(trackedImage.referenceImage.name, out var arObject))
            {
                UpdateTrackedImages(trackedImage);
            }
        }
        foreach (var trackedImage in eventArgs.updated)
        {
            if (arObjects.TryGetValue(trackedImage.referenceImage.name, out var arObject))
            {
                UpdateTrackedImages(trackedImage);
            }
        }
        foreach (var trackedImage in eventArgs.removed)
        {
            UpdateTrackedImages(trackedImage.Value);
        }
    }

    private void UpdateTrackedImages(ARTrackedImage trackedImage)
    {
        if (trackedImage == null) return;
        if(trackedImage.trackingState is TrackingState.Limited or TrackingState.None)
        {
            arObjects[trackedImage.referenceImage.name].gameObject.SetActive(false);
            return;
        }

        arObjects[trackedImage.referenceImage.name].gameObject.SetActive(true);
        arObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
        arObjects[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
    }
}
