using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace.Utility
{
    public static class ServiceLocator
    {
        private static Dictionary<Type, object> services = new Dictionary<Type, object>();
        private static bool _isInitialized = false;
        private static TaskCompletionSource<bool> _initializationTask = new TaskCompletionSource<bool>();

        /// <summary>
        /// Indicates whether core systems have been initialized
        /// </summary>
        public static bool IsInitialized => _isInitialized;

        /// <summary>
        /// Task that completes when core initialization is done
        /// </summary>
        public static Task InitializationTask => _initializationTask.Task;

        /// <summary>
        /// Registers a service that can be accessed by other objects
        /// </summary>
        public static void RegisterService<T>(T service) where T : class
        {
            services[typeof(T)] = service;
        }

        /// <summary>
        /// Gets a service of the specified type
        /// </summary>
        public static T GetService<T>() where T : class
        {
            if (services.TryGetValue(typeof(T), out var service))
            {
                return service as T;
            }
            return null;
        }

        /// <summary>
        /// Gets a service asynchronously, waiting for it to be registered if needed
        /// </summary>
        /// <param name="timeout">Maximum time to wait in seconds</param>
        public static async Task<T> GetServiceAsync<T>(float timeout = 5f) where T : class
        {
            T service = GetService<T>();
            if (service != null)
                return service;

            float elapsed = 0f;
            float checkInterval = 0.1f;

            while (elapsed < timeout)
            {
                await Task.Delay((int)(checkInterval * 1000));
                elapsed += checkInterval;
                
                service = GetService<T>();
                if (service != null)
                    return service;
            }

            Debug.LogWarning($"Timed out waiting for service of type {typeof(T).Name}");
            return null;
        }

        /// <summary>
        /// Called by the Bootstrapper when core systems are initialized
        /// </summary>
        public static void SetInitialized()
        {
            _isInitialized = true;
            _initializationTask.TrySetResult(true);
        }

        /// <summary>
        /// Resets initialization state (for scene changes or testing)
        /// </summary>
        public static void ResetInitialization()
        {
            _isInitialized = false;
            _initializationTask = new TaskCompletionSource<bool>();
        }

        /// <summary>
        /// Clears all registered services
        /// </summary>
        public static void ClearServices()
        {
            services.Clear();
        }
    }
}