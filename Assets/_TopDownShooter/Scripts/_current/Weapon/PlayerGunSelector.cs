using System.Collections.Generic;
using _current.Data;
using UnityEngine;

namespace _current.Weapon
{
    [DisallowMultipleComponent]
    public class PlayerGunSelector : MonoBehaviour
    {
        [SerializeField] private GunType _gunType;
        [SerializeField] private Transform _gunParent;
        [SerializeField] private List<GunScriptableObject> _guns;

        [Space] 
        [Header("Runtime Filled")]
        public GunScriptableObject ActiveGun;

        private void Start()
        {
            var gun = _guns.Find(gun => gun.Type == _gunType);

            if (gun == null)
            {
                Debug.LogError($"No GunScriptableObject found for GunType: {gun}");
                return;
            }

            ActiveGun = gun;
            gun.Spawn(_gunParent, this);
        }
    }
}