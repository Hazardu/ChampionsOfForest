using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BuilderCore;
namespace ChampionsOfForest.Effects
{
    public class Multishot : MonoBehaviour
    {
        public Transform root;
        public Transform child1;
        public Transform child2;
        public Transform child3;

        public static GameObject localPlayerInstance;
        public static bool IsOn;

        public static GameObject Create(Transform root, Transform hand)
        {
            GameObject go = new GameObject("__SPELL EFFECT 1__");

            GameObject child = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Material mat = new Material(Shader.Find("Particles/Additive"));
            mat.SetColor("_TintColor", new Color(0.0f, 0.3f, 0.2f, 0.2f));
            mat.mainTexture = Res.ResourceLoader.GetTexture(126);
            child.GetComponent<Renderer>().material = mat;

            var light = go.AddComponent<Light>();
            light.shadowStrength = 1;
            light.shadows = LightShadows.Hard;
            light.type = LightType.Point;
            light.range = 14;
            light.color = new Color(0, 1f, 0.66f);
            light.intensity = 0.6f;


            go.transform.parent = hand;
          var c = go.AddComponent<Multishot>();
            c.root = root;
            go.transform.position = hand.position;
            go.transform.localScale = Vector3.one * 0.6f;
            child.transform.parent = go.transform;
            child.transform.localPosition = Vector3.forward;
            c.child1 = child.transform;
            c.child2 = Instantiate(child, go.transform.position + Vector3.forward * 2, Quaternion.identity, go.transform).transform;
            c.child3 = Instantiate(child, go.transform.position + Vector3.forward * 3, Quaternion.identity, go.transform).transform;
            c.child2.localScale *= 2;
            c.child3.localScale *= 4;



            return go;

        }

        void Update()
        {
            transform.rotation = root.transform.rotation;
            child1.Rotate(Vector3.forward * 90*Time.deltaTime);
            child2.Rotate(-Vector3.forward * 45 * Time.deltaTime);
            child3.Rotate(Vector3.forward * 20 * Time.deltaTime);
        }
    }
}
