using DG.Tweening;
using UnityEngine;

namespace GuitarHero
{
    public class GuitarHeroNoteObject : MonoBehaviour
    {
        public Renderer renderer;
        public GuitarHeroNote.NoteType noteType = GuitarHeroNote.NoteType.Strum;
        public float time;
        public bool active = true;

        private static readonly int shader_EmissiveIntensity = Shader.PropertyToID("_Emissive_Intensity");
        private static readonly int shader_Alpha = Shader.PropertyToID("_Alpha");

        public void Hit()
        {
            active = false;
            var m = renderer.material;
            m.DOFloat(10.0F, shader_EmissiveIntensity, 0.05F);
            m.DOFloat(0.0F, shader_Alpha, 0.1F).OnComplete(() =>
            {
                gameObject.SetActive(false);
                Destroy(m);
            });
        }

        public void Missed()
        {
            active = false;
            var m = renderer.material;
            m.DOFloat(0.0F, shader_Alpha, 0.1F).OnComplete(() =>
            {
                gameObject.SetActive(false);
                Destroy(m);
            });
        }

        private void OnValidate()
        {
            renderer = GetComponent<Renderer>();
        }
    }
}