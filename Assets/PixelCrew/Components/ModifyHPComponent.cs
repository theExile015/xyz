using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{

    public class ModifyHPComponent : MonoBehaviour
    {
        [SerializeField] private int _value;

        public void ModifyHP(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.ModifyHP(_value);
            }
        }
    }

}
