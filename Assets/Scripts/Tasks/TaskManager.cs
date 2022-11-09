using System.Collections.Generic;
using UnityEngine;
using InputSystem;
using Extensions.EventSystem;
    
namespace Game
{
    public class TaskManager : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private InputController _input = null;

        [Header("Global Events")]
        [SerializeField]
        private GlobalEvent _onCellClicked = null;
        [SerializeField]
        private GlobalEvent _onCreateTask = null;

        private GameBehaviour behaviour = null;
        private SelectionComponent selection = null;

        private Queue<ITask> tasks = new Queue<ITask>();
        private ITask curTask = null;
        private PlayerBehaviour player;

        private void Awake()
        {
            player = FindObjectOfType<PlayerBehaviour>();
        }

        private void OnEnable()
        {
            _input.OnStart += HandleClick;
            _input.OnHitObject += HandleHitObject;

            _onCreateTask.Subscribe(HandleCreateTask);
        }

        private void OnDisable()
        {
            _input.OnStart -= HandleClick;
            _input.OnHitObject -= HandleHitObject;

            _onCreateTask.Unsubscribe(HandleCreateTask);
        }

        private void HandleCreateTask(object sender, object data)
        {
            ITask task = (ITask)data;

            AddTask(task);
        }

        private void FixedUpdate()
        {
            if(tasks.Count > 0 && (curTask == null || curTask.CanBreak))
            {
                if(curTask != null)
                {
                    curTask.Break();
                }

                curTask = tasks.Dequeue();

                curTask.Start();

                curTask.OnFinish += HandleTaskFinsh;
            }
        }

        private void HandleHitObject(GameObject hittedObject)
        {
            if(hittedObject == null)
            {
                if (selection != null)
                    selection.Set(false);

                selection = null;
                behaviour = null;

                return;
            }

            if(selection != null)
                selection.Set(false);

            selection = hittedObject.GetComponent<SelectionComponent>();

            if (selection != null)
                selection.Set(true);

            behaviour = hittedObject.GetComponent<GameBehaviour>();

            if (behaviour == null)
                return;
        }

        private void HandleTaskFinsh(ITask task)
        {
            curTask.OnFinish -= HandleTaskFinsh;

            curTask = null;
        }

        private void HandleClick(InputData data)
        {
            if (behaviour != null)
            {
                ITaskMaker taskMaker = behaviour.GetComponent<ITaskMaker>();

                ITask task = taskMaker.GetTask();

                if (taskMaker != null && task != null)
                {
                    tasks.Enqueue(task);

                    return;
                }

                Cell cell = behaviour.GetComponent<Cell>();

                if (cell == null)
                    return;

                _onCellClicked.Invoke(this, cell);
            }

            MoveTask move = new MoveTask();

            move.Init(player, _input.GeneralData.Position);

            tasks.Enqueue(move);
        }

        public void AddTask(ITask task)
        {
            tasks.Enqueue(task);
        }
    }
}