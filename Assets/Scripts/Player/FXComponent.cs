using System.Collections;
using UnityEngine;

namespace Game
{
    public class FXComponent : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private AnimationComponent _animation = null;

        [Header("FX")]
        [SerializeField]
        private GameObject _aura = null;
        
        private void OnEnable()
        {
            _animation.OnLevitating += HandleLevitating;
        }

        private void OnDisable()
        {
            _animation.OnLevitating -= HandleLevitating;
        }

        private void HandleLevitating(bool value)
        {
            if(value == false)
            {
                StartCoroutine(StopParticle(_aura));

                return;
            }

            _aura.SetActive(value);
        }

        private IEnumerator StopParticle(GameObject particle)
        {
            particle.GetComponent<ParticleSystem>().Stop();

            yield return new WaitForSeconds(2f);

            particle.SetActive(false);
        }
    }
}