using System.Collections.Generic;
using UnityEngine;

namespace ChampionsOfForest.Effects
{
    public class RealisticBlackHoleEffect : MonoBehaviour
    {
        public Shader shader;
        public static RealisticBlackHoleEffect instance;
        public static void Add(Transform tr)
        {
            instance.BH.Add(tr);
        }


        public float ratio = 0.5f;     // The ratio of the height to the length of the screen to display properly shader

        public List<Transform> BH = new List<Transform>();  // The object whose position is taken as the position of the black hole


        public Camera cam;
        private Material _material; // Material which is located shader
        protected Material material
        {
            get
            {
                if (_material == null)
                {
                    _material = new Material(shader);
                    _material.hideFlags = HideFlags.HideAndDontSave;
                }
                return _material;
            }
        }
        public void Awake()
        {
            cam = Camera.main;
            ratio = 1f / cam.aspect;
            instance = this;
            try
            {

            var bundle = Res.ResourceLoader.GetAssetBundle(2004);
            shader = bundle.LoadAsset<Shader>("BlackHoleShader.shader");
            }
            catch (System.Exception e)
            {

                CotfUtils.Log(e.ToString());
            }
        }


        protected virtual void OnDisable()
        {
            if (_material)
            {
                DestroyImmediate(_material);
            }
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            int a = 0;
            if (shader && material && BH.Count > 0)
            {
                RenderTexture temporary = RenderTexture.GetTemporary(destination.width, destination.height, destination.depth, destination.format, RenderTextureReadWrite.Default);
                for (int i = 0; i < BH.Count; i++)
                {
                    var bh = BH[i];

                    if (bh != null)
                    {
                        var heading = bh.position - cam.transform.position;

                        if (Vector3.Dot(cam.transform.forward, heading) > 0)
                        {
                            // Object is in front.


                            // Find the position of the black hole in screen coordinates
                            Vector2 pos = new Vector2(
                               cam.WorldToScreenPoint(bh.position).x / cam.pixelWidth,
                                (cam.WorldToScreenPoint(bh.position).y / cam.pixelHeight));

                            // Install all the required parameters for the shader
                            material.SetVector("_Position", new Vector2(pos.x, pos.y));
                            material.SetFloat("_Ratio", ratio);
                            material.SetFloat("_Rad", bh.transform.localScale.x * 2);
                            material.SetFloat("_Distance", Vector3.Distance(bh.position, transform.position));
                            // And is applied to the resulting image.
                            if(a>0)
                            Graphics.Blit(source, temporary, material);

                                else
                            Graphics.Blit(source, temporary, material);
                            a++;
                        }
                    }
                    else
                    {
                        BH.RemoveAt(i);
                    }
                }
            }
            if (a == 0)
            {
                Graphics.Blit(source, destination);
            }
        }
    }
}