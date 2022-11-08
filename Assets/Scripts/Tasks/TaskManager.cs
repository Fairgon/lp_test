using System.Collections.Generic;
using UnityEngine;
using InputSystem;
using System.Linq;

namespace Game
{
    public class TaskManager : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private InputController _input = null;

        private GameBehaviour behaviour = null;
        private SelectionComponent selection = null;
        private SelectionComponent newSelection = null;

        private Queue<ITask> tasks = new Queue<ITask>();
        private ITask curTask = null;

        private void OnEnable()
        {
            _input.OnStart += HandleClick;
        }

        private void OnDisable()
        {
            _input.OnStart -= HandleClick;
        }

        private void FixedUpdate()
        {
            if(tasks.Count > 0 && curTask == null)
            {
                curTask = tasks.Dequeue();

                curTask.Start();

                curTask.OnFinish += HandleTaskFinsh;
            }

            if (_input.GeneralData == null)
                return;

            for(int i = 0; i < _input.GeneralData.Objects.Count; ++i)
            {
                behaviour = _input.GeneralData.Objects[i].GetComponent<GameBehaviour>();
                newSelection = _input.GeneralData.Objects[i].GetComponent<SelectionComponent>();

                if(newSelection != null && newSelection != selection)
                {
                    if(selection != null)
                        selection.Set(false);

                    selection = newSelection;

                    selection.Set(true);
                }

                if (behaviour == null)
                    continue;

                if(behaviour.Owner == Owner.AI)
                {
                    Attack();

                    break;
                }

                if (behaviour.Owner == Owner.Neutral)
                {
                    Use();

                    break;
                }
            }

            if(newSelection == null && selection != null)
            {
                selection.Set(false);

                selection = null;
            }

            if(behaviour == null)
            {
                CursorController.SetCursor(CursorTypes.Default);
            }
        }

        private void HandleTaskFinsh(ITask task)
        {
            curTask.OnFinish -= HandleTaskFinsh;

            curTask = null;
        }

        private void HandleClick(InputData data)
        {
            if (behaviour == null)
                return;

            ITaskMaker taskMaker = behaviour.GetComponent<ITaskMaker>();

            tasks.Enqueue(taskMaker.GetTask());
        }

        private void Attack()
        {
            CursorController.SetCursor(CursorTypes.Attack);
        }

        private void Use()
        {
            CursorController.SetCursor(CursorTypes.Use);
        }
    }
}