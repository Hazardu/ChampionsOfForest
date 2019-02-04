using ChampionsOfForest.Player;
using ChampionsOfForest.Res;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheForest.Utils;
using UnityEngine;
namespace ChampionsOfForest.Effects
{
    public class BlackFlame : MonoBehaviour
    {
        public static BlackFlame instance;
        public static float FireDamageBonus =>(10+ ModdedPlayer.instance.SpellDamageBonus) * ModdedPlayer.instance.SpellAMP / 4;

        private static Material mat1;
        private static Material mat2;
        public static GameObject instanceLocalPlayer;
        public static GameObject Create()
        {
            AnimationCurve sizecurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0, 5.129462f, 5.129462f), new Keyframe(0.2449522f, 1, 0, 0), new Keyframe(1, 0, -1.242162f, -1.242162f), });
            if (mat1 == null)
            {
                mat1 = new Material(Shader.Find("Particles/Multiply"));
                mat2 = new Material(Shader.Find("Particles/Additive"));

                mat1.color = Color.white;
                mat2.color = Color.white;

                mat1.mainTexture = Res.ResourceLoader.GetTexture(111);
                mat2.mainTexture = Res.ResourceLoader.GetTexture(111);

                mat1.renderQueue = 3050;
                mat2.renderQueue = 3000;
            }


            GameObject go = new GameObject("__BlackFlames__");

            ParticleSystem ps = go.AddComponent<ParticleSystem>();
            ParticleSystem.MainModule main = ps.main;
            ParticleSystem.EmissionModule emission = ps.emission;
            ParticleSystem.ShapeModule shape = ps.shape;
            ParticleSystem.RotationOverLifetimeModule rot = ps.rotationOverLifetime;
            ParticleSystem.SizeOverLifetimeModule size = ps.sizeOverLifetime;
            ParticleSystemRenderer rend = ps.GetComponent<ParticleSystemRenderer>();

            main.startSize = 0.4f;
            main.startSpeed = -6f;
            main.startLifetime = 0.75f;
            main.startColor = new Color(0, 0, 1, 0.623f);
            main.startRotation = new ParticleSystem.MinMaxCurve(0, 90);

            emission.rateOverTime = 100;

            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 0;
            shape.radius = 0.25f;

            rot.enabled = true;
            rot.z = 180;

            size.enabled = true;
            size.size = new ParticleSystem.MinMaxCurve(1, sizecurve);

            rend.renderMode = ParticleSystemRenderMode.Stretch;
            rend.lengthScale = 2;
            rend.normalDirection = 1;
            rend.material = mat1;

            GameObject go2 = GameObject.Instantiate(go, go.transform);
            ParticleSystem ps2 = go.GetComponent<ParticleSystem>();
            ParticleSystemRenderer rend2 = go.GetComponent<ParticleSystemRenderer>();
            ParticleSystem.MainModule main2 = ps2.main;
            main2.startColor = new Color(0, 0.15f, 1, 0.623f);
            main2.startLifetime = 1;
            main2.startSpeed = -4;

            rend2.material = mat2;



            return go;
        }

        public static bool IsOn = false;
        public static float Cost = 75;

        void Start()
        {
            StartCoroutine(StartCoroutine());
        }

        IEnumerator StartCoroutine()
        {
            while (ModReferences.rightHandTransform == null)
            {
            yield return null;
                Debug.Log("Waiting for setup");
                if (LocalPlayer.Inventory != null)
                {
                    LocalPlayer.Inventory.Equip(80, false);
                }
            }
            yield return null;
            instanceLocalPlayer = Create();
            instanceLocalPlayer.transform.position = ModReferences.rightHandTransform.position;
            instanceLocalPlayer.transform.rotation = ModReferences.rightHandTransform.rotation;
            instanceLocalPlayer.transform.parent = ModReferences.rightHandTransform;
            instanceLocalPlayer.SetActive(false);
            instance = this;

        }

        void Update()
        {
            if (IsOn)
            {
                SpellCaster.RemoveStamina(Cost * Time.deltaTime);
                if (LocalPlayer.Stats.Stamina  < 10)
                {
                    Toggle();
                }

            }
        }

        public static void Toggle()
        {
            IsOn = !IsOn;
            instanceLocalPlayer.SetActive(IsOn);
            if (BoltNetwork.isRunning)
            {
                string s = "SC4;";
                if (IsOn) s += "t;";
                else s += "f;";
                s += ModReferences.ThisPlayerPacked + ";";
                Network.NetworkManager.SendLine(s, Network.NetworkManager.Target.Others);
            }
        }
        public static void ToggleOtherPlayer(ulong packed,bool ison)
        {
            var ents = ModReferences.AllPlayerEntities.Where(ent => ent.networkId.PackedValue == packed).Select(ent => ent.gameObject).ToArray();
            if(ents.Length > 0)
            {
                GameObject go = ents[0];
                Debug.Log(go.name);


            }
        }
    }
}
