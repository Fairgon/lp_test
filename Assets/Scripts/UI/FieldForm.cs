using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Game.UI
{
    public class FieldForm : UIForm
    {
        [Header("General")]
        [SerializeField]
        private Button _build = null;
        [SerializeField]
        private List<Toggle> toggles = null;

        private Field field = null;

        public void Init(Field field)
        {
            this.field = field;
        }

        private void Awake()
        {
            _build.onClick.AddListener(() => Build());
        }

        private void Build()
        {
            List<bool> bildData = new List<bool>(toggles.Select(x => x.isOn));

            field.Build(bildData);

            Close();
        }
    }
}