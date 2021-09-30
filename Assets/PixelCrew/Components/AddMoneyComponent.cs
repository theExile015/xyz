using UnityEngine;

namespace PixelCrew.Components
{

    public class AddMoneyComponent : MonoBehaviour
    {
        [SerializeField] private Hero _hero;
        [SerializeField] byte _value;

        public void AddSomeMoney()
        {
            if (_value > 0)
            {
                _hero._money += _value;
                Debug.Log("Вы нашли монетку ценностью " +_value + " дублонов. Общее количество дублонов " + _hero._money);
            }
        }

    }
}
