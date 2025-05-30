// Updated AddressablesManager.cs to only support loading by address

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Addler.Runtime.Core.LifetimeBinding;

namespace AssetManagement
{
    [DefaultExecutionOrder(-100)]
    public class AddressablesManager : MonoBehaviour
    {
        public static AddressablesManager Instance { get; private set; }

        private readonly Dictionary<string, AsyncOperationHandle> _downloadHandles = new();

        public static Task Initialized => Instance?._initializationSource?.Task ?? Task.CompletedTask;

        private TaskCompletionSource<bool> _initializationSource = new();

        private async void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            await Addressables.InitializeAsync().Task;
            _initializationSource.TrySetResult(true);
        }

        public async Task DownloadDependenciesAsync(IEnumerable<string> labels)
        {
            var labelList = labels.Select(label => label.Trim()).Where(label => !string.IsNullOrEmpty(label))
                .OrderBy(label => label).ToList();
            if (labelList.Count == 0) return;

            string cacheKey = string.Join(",", labelList);

            if (_downloadHandles.ContainsKey(cacheKey)) return;

            var handle = Addressables.DownloadDependenciesAsync(labelList, Addressables.MergeMode.Intersection);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
                _downloadHandles[cacheKey] = handle;
        }

        public void ReleaseDependencies(IEnumerable<string> labels)
        {
            var labelList = labels.Select(label => label.Trim()).Where(label => !string.IsNullOrEmpty(label))
                .OrderBy(label => label).ToList();
            if (labelList.Count == 0) return;

            string cacheKey = string.Join(",", labelList);

            if (_downloadHandles.TryGetValue(cacheKey, out var handle))
            {
                Addressables.Release(handle);
                _downloadHandles.Remove(cacheKey);
            }
        }

        public async Task<T> LoadByAddressAsync<T>(string address, GameObject bindTo = null)
            where T : UnityEngine.Object
        {
            var handle = Addressables.LoadAssetAsync<T>(address);
            if (bindTo != null) handle.BindTo(bindTo);
            await handle.Task;

            return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
        }

        public async Task<GameObject> LoadAndInstantiateByAddressAsync(string address)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(address);
            await handle.Task;
            if (handle.Status != AsyncOperationStatus.Succeeded) return null;

            var prefab = handle.Result;
            var instance = Instantiate(prefab);
            handle.BindTo(instance);

            return instance;
        }
        public async Task<GameObject> LoadAndInstantiateByAddressAsync(string address, Transform parent)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(address);
            await handle.Task;
            if (handle.Status != AsyncOperationStatus.Succeeded) return null;

            var prefab = handle.Result;
            var instance = Instantiate(prefab, parent);
            handle.BindTo(instance);

            return instance;
        }
    }
}