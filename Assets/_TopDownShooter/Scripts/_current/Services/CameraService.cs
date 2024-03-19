using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace _current.Services
{
    public class CameraService : ICameraService, IInitializable
    {
        private readonly Dictionary<Type, CinemachineExtension> _cinemachineExtensions = new();
        private GameObject _playerVirtualCamera;

        public void Initialize()
        {
            foreach (var extension in _playerVirtualCamera
                         .GetComponents<CinemachineExtension>())
                _cinemachineExtensions[extension.GetType()] = extension;
        }

        public T GetCinemachineExtension<T>() where T : CinemachineExtension
        {
            return (T)_cinemachineExtensions[typeof(T)];
        }
    }

    public interface ICameraService
    {
    }
}