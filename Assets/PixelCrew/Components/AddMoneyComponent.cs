using UnityEngine;

namespace PixelCrew.Components
{

    public class AddMoneyComponent : MonoBehaviour
    {
        [SerializeField] private byte _value;
        private Hero _hero;


        private void Start()
        {
            _hero = FindObjectOfType<Hero>();
        }

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
