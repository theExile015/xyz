using UnityEngine;

namespace PixelCrew.Components.Health
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

        public void SetValue(float value)
        {
            _value = (int)value;
        }
    }

}
