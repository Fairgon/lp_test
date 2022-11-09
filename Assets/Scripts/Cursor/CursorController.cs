using UnityEngine;
using InputSystem;
using System.Linq;

namespace Game
{
    public enum CursorTypes
    {
        Default, 
        Attack,
        Use,
        Drag
    }

    public class CursorController : MonoBehaviour
    {
        public static CursorController instance = null;

        [Header("General")]
        [SerializeField]
        private InputController _input = null;
        [SerializeField]
        private CursorsData data = null;

        private GameBehaviour behaviour = null;

        private void Awake()
        {
            instance = this;

            Cursor.SetCursor(data.GetCursor(CursorTypes.Default).Texture, Vector2.zero, CursorMode.Auto);
        }

        public static void SetCursor(CursorTypes type, bool ignore = false)
        {
            if (InputController.RightInput && !ignore)
                return;

            Cursor.SetCursor(instance.data.GetCursor(type).Texture, Vector2.zero, CursorMode.Auto);
        }

        private void OnEnable()
        {
            _input.OnDragStart += HandleDragStart;
            _input.OnDragFinish += HandleDragFinish;
            
            _input.OnHitObject += HandleHitObject;
        }

        private void OnDisable()
        {
            _input.OnDragStart -= HandleDragStart;
            _input.OnDragFinish -= HandleDragFinish;
            
            _input.OnHitObject -= HandleHitObject;
        }

        private void HandleDragStart()
        {
            SetCursor(CursorTypes.Drag, true);
        }

        private void HandleDragFinish()
        {
            HandleHitObject(_input.HittedObject);
        }

        private void HandleHitObject(GameObject hittedObject)
        {
            if (hittedObject == null)
            {
                behaviour = null;

                SetCursor(CursorTypes.Default);

                return;
            }

            behaviour = hittedObject.GetComponent<GameBehaviour>();

            if (behaviour == null)
            {
                SetCursor(CursorTypes.Default);

                return;
            }
            
            if (behaviour.Owner == Owner.AI)
                SetCursor(CursorTypes.Attack);
            else if (behaviour.Owner == Owner.Neutral)
                SetCursor(CursorTypes.Use);
        }
    }
}