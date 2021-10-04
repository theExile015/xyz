using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class SpawnParticlesComponent : MonoBehaviour
    {
        [SerializeField] private Transform _stepsTarget;
        [SerializeField] private Transform _riseAndFallTarget;
        [SerializeField] private GameObject _stepsPrefub;
        [SerializeField] private GameObject _jumpPrefub;
        [SerializeField] private GameObject _landPrefub;

        [ContextMenu("Spawn")]

        public void Spawn(int _value)
        {
            switch (_value)
            {
                case 0:
                    {
                        var instantiate = Instantiate(_stepsPrefub, _stepsTarget.position, Quaternion.identity);
                        instantiate.transform.localScale = _stepsTarget.lossyScale;
                        break;
                    }
                case 1:
                    {
                        var instantiate = Instantiate(_jumpPrefub, _riseAndFallTarget.position, Quaternion.identity);
                        instantiate.transform.localScale = _stepsTarget.lossyScale;
                        break;
                    }
                case 2:
                    {
                        var instantiate = Instantiate(_landPrefub, _riseAndFallTarget.position, Quaternion.identity);
                        instantiate.transform.localScale = _stepsTarget.lossyScale;
                        break;
                    }
            }
            
        }
    }
}

