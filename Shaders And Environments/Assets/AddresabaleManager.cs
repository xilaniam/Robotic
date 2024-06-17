using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddresabaleManager : MonoBehaviour
{
    [SerializeField] private AssetReferenceGameObject _firefliesAssetReference;
    GameObject _instanceReference;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            _firefliesAssetReference.InstantiateAsync().Completed += AddresabaleManager_Completed; ;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            _firefliesAssetReference.ReleaseInstance(_instanceReference);
        }
    }

    private void AddresabaleManager_Completed(AsyncOperationHandle<GameObject> obj)
    {
        if(obj.Status == AsyncOperationStatus.Succeeded)
        {
            _instanceReference = obj.Result;
        }
        
    }
}
