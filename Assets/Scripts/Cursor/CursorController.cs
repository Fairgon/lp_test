using UnityEngine;
using InputSystem;
using System.Linq;

namespace Game
{
    public enum CursorTypes
    {
        Default, 
        Attack,
        Use
    }

    public class CursorController : MonoBehaviour
    {
        public static CursorController instance = null;

        [Header("General")]
        [SerializeField]
        private InputController _input = null;
        [SerializeField]
        private CursorsData data = null;
        [SerializeField]
        private Transform _spriteTarget = null;

        private void Awake()
        {
            instance = this;

            Cursor.SetCursor(data.GetCursor(CursorTypes.Default).Texture, Vector2.zero, CursorMode.Auto);
        }
        
        public static void SetCursor(CursorTypes type)
        {
            Cursor.SetCursor(instance.data.GetCursor(type).Texture, Vector2.zero, CursorMode.Auto);
        }

        private void OnEnable()
        {
            _input.OnStart += HandleInputStart;
            _input.OnMove += HandleInputMove;
        }

        private void OnDisable()
        {
            _input.OnStart -= HandleInputStart;
            _input.OnMove -= HandleInputMove;
        }

        private void HandleInputStart(InputData data)
        {
            _spriteTarget.position = data.Position + Vector3.up * 0.1f;
        }

        private void HandleInputMove(InputData data)
        {
            _spriteTarget.position = data.Position + Vector3.up * 0.1f;
        }
    }
}