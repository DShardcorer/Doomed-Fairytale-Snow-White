using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Addler.Runtime.Core.LifetimeBinding;

namespace AssetManagement
{
    [DefaultExecutionOrder(-100)] // Ensure early execution
    public class AddressablesManager : MonoBehaviour
    {
        public static AddressablesManager Instance { get; private set; }

        private readonly Dictionary<string, IList<IResourceLocation>> _locationCache = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public async Task<T> LoadByLabelsAndNameAsync<T>(
            IEnumerable<string> labels,
            string assetName,
            GameObject bindTo
        ) where T : UnityEngine.Object
        {
            var labelList = labels
                .Select(label => label.Trim())
                .Where(label => !string.IsNullOrEmpty(label))
                .OrderBy(label => label)
                .ToList();

            if (labelList.Count == 0)
                throw new ArgumentException("At least one label is required.", nameof(labels));

            string cacheKey = GetCacheKey<T>(labelList);

            if (!_locationCache.TryGetValue(cacheKey, out var locations))
            {
                var labelHandle = Addressables.LoadResourceLocationsAsync(
                    labelList,
                    Addressables.MergeMode.Intersection,
                    typeof(T)
                );

                await labelHandle.Task;

                if (labelHandle.Status != AsyncOperationStatus.Succeeded)
                    return null;

                locations = labelHandle.Result;
                _locationCache[cacheKey] = locations;
            }

            var targetLocation = locations.FirstOrDefault(loc =>
                loc.PrimaryKey.Equals(assetName, StringComparison.OrdinalIgnoreCase) ||
                loc.InternalId.Contains(assetName, StringComparison.OrdinalIgnoreCase));

            if (targetLocation == null)
                return null;

            var handle = Addressables.LoadAssetAsync<T>(targetLocation).BindTo(bindTo);
            await handle.Task;

            return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
        }

        public async Task<GameObject> LoadAndInstantiateByLabelsAndNameAsync(
            IEnumerable<string> labels,
            string assetName
        )
        {
            var labelList = labels
                .Select(label => label.Trim())
                .Where(label => !string.IsNullOrEmpty(label))
                .OrderBy(label => label)
                .ToList();

            if (labelList.Count == 0)
                throw new ArgumentException("At least one label is required.", nameof(labels));

            string cacheKey = GetCacheKey<GameObject>(labelList);

            if (!_locationCache.TryGetValue(cacheKey, out var locations))
            {
                var labelHandle = Addressables.LoadResourceLocationsAsync(
                    labelList,
                    Addressables.MergeMode.Intersection,
                    typeof(GameObject)
                );

                await labelHandle.Task;

                if (labelHandle.Status != AsyncOperationStatus.Succeeded)
                    return null;

                locations = labelHandle.Result;
                _locationCache[cacheKey] = locations;
            }

            var targetLocation = locations.FirstOrDefault(loc =>
                loc.PrimaryKey.Equals(assetName, StringComparison.OrdinalIgnoreCase) ||
                loc.InternalId.Contains(assetName, StringComparison.OrdinalIgnoreCase));

            if (targetLocation == null)
                return null;

            // Load the prefab itself
            var prefabHandle = Addressables.LoadAssetAsync<GameObject>(targetLocation);
            await prefabHandle.Task;

            if (prefabHandle.Status != AsyncOperationStatus.Succeeded)
                return null;

            var prefab = prefabHandle.Result;
            var instance = Instantiate(prefab);

            // Bind the prefab handle to the instance so it gets released on destroy
            prefabHandle.BindTo(instance);

            return instance;
        }


        private static string GetCacheKey<T>(IEnumerable<string> sortedLabels)
        {
            return $"{typeof(T).FullName}|{string.Join(",", sortedLabels)}";
        }
    }
}