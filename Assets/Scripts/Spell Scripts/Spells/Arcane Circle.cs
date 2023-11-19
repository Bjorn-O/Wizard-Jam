using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Spell_Scripts.Spells
{
    public class ArcaneCircle : Spell
    {
        [SerializeField] private Transform camPosition;
        [SerializeField] private LayerMask layerMask;

        private bool _shouldCast;
        private bool _isCasting;

        private void Update()
        {
            if (!_isCasting) return;


        }

        public override IEnumerator CastSpell()
        {
            if (_isCasting) yield return null;
            
            RaycastHit hit;
            int timesCast = 0;

            while (timesCast < castAmount) 
            {
                // Instantiate preview

                if (Physics.Raycast(camPosition.position, camPosition.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
                {
                    // Move Preview to out.point
                }

                if (_shouldCast)
                {
                    FireSpellEffect(spellEffect, castAmount);
                }
            }
        }

        protected override void FireSpellEffect(SpellEffect effect, float amount)
        {
            throw new System.NotImplementedException();
        }
    }
}