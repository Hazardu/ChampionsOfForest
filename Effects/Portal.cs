using BuilderCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheForest.Items.World;
using TheForest.Utils;
using UnityEngine;

namespace ChampionsOfForest.Effects
{
    public class Portal : MonoBehaviour
    {
        private static int PortalID=0;
        public static Portal[] portals;
        public static void InitializePortals()
        {
            GameObject p1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            p1.name = "__PORTAL__";
            p1.GetComponent<SphereCollider>().isTrigger = true;
          var pcomponent =  p1.AddComponent<Portal>();
            if (!ModSettings.IsDedicated)
            {
                Camera cam1 = new GameObject("__CAM__").AddComponent<Camera>();
                cam1.transform.parent = p1.transform;
                cam1.fieldOfView = 50;
                var light = p1.AddComponent<Light>();
                light.type = LightType.Point;
                light.range = 20;
                Material mat = new Material(Shader.Find("Unlit/Texture"));
                p1.GetComponent<MeshFilter>().mesh = Res.ResourceLoader.instance.LoadedMeshes[112];


                var MR = p1.GetComponent<MeshRenderer>();
                MR.material = mat;

                pcomponent.cam = cam1;
                pcomponent.rend = MR;
            }
            GameObject p2 = Instantiate(p1);
            var portal2 = p2.GetComponent<Portal>();
            pcomponent.otherPortal = portal2;
            portal2.otherPortal = pcomponent;

            portals = new Portal[]
            {
               pcomponent,portal2
            };

            p1.SetActive(false);
            p2.SetActive(false);
        }
        public static void CreatePortal(Vector3 pos, float Duration,int portalID)
        {
            portals[portalID].gameObject.SetActive(true);
            portals[portalID].transform.position = pos;

            portals[portalID].Duration = Duration;
            portals[portalID].Enable();
            if (portalID == 1) portalID = 0;
            else portalID =1;
        }
        
        public static void SyncTransform(Vector3 pos, float Duration, int portalID)
        {
            string s = String.Concat("SC6;", pos.x, ";", pos.y, ";", pos.z, ";", Duration, ";", portalID, ";");
            Network.NetworkManager.SendLine(s,Network.NetworkManager.Target.Others);
        }

        public static int GetPortalID()
        {
            if (!portals[0].gameObject.activeSelf) return 0;
            if (!portals[1].gameObject.activeSelf) return 1;
            return PortalID;
        }


        public Portal otherPortal;

        public Camera cam;

        public MeshRenderer rend;

        public RenderTexture renderTexture;

        public List<Transform> Excludedtransforms;

        public float Duration;
        // Start is called before the first frame update
        void Start()
        {
            renderTexture = new RenderTexture(512, 512, 16);
            cam.forceIntoRenderTexture = true;
            cam.targetTexture = renderTexture;
            Excludedtransforms = new List<Transform>();
            TimeOfPass = 0;
        }

        void Enable()
        {
            transform.localScale = Vector3.zero;

            StopAllCoroutines();
 StartCoroutine(ScaleIn());
 StartCoroutine(DurationCoroutine());
            Excludedtransforms.Clear();
            TimeOfPass = 0;

        }
        // Update is called once per frame
        void Update()
        {
            if (ModSettings.IsDedicated) return;
            if (!otherPortal.gameObject.activeSelf)
            {
                rend.material.mainTexture = Texture2D.blackTexture;
                return; }

            Vector3 lookAtDir = transform.position - Camera.main.transform.position;
            lookAtDir.Normalize();

            otherPortal.cam.transform.position = otherPortal.transform.position + lookAtDir;
            otherPortal.cam.transform.rotation = Quaternion.LookRotation(lookAtDir, Vector3.up);

            rend.material.mainTexture = otherPortal.cam.activeTexture;
        }

        IEnumerator ScaleIn()
        {
            yield return null;
            while (transform.localScale.x < 2)
            {
                transform.localScale += Vector3.one * Time.deltaTime;
                yield return null;
            }
        }
        IEnumerator DurationCoroutine()
        {
            while (Duration > 0)
            {
                Duration--;
                yield return new  WaitForSeconds(1);
            }
            gameObject.SetActive(false);
        }

            void OnTriggerEnter(Collider other)
        {
            if (!otherPortal.gameObject.activeSelf) { return; }


            if (!ModSettings.IsDedicated && other.transform.root == LocalPlayer.Transform && TimeOfPass+0.6f< Time.time)
            {
                if ((other.transform.root.position - transform.position).sqrMagnitude < 5)
                {
                    if (Excludedtransforms.Contains(LocalPlayer.Transform)) return;
                    otherPortal.TimeOfPass = Time.time;

                    LocalPlayer.Transform.position = otherPortal.transform.position;
                    Vector3 dir = LocalPlayer.Transform.position - transform.position;
                    dir.Normalize();
                    LocalPlayer.Rigidbody.AddForce(dir * 3, ForceMode.VelocityChange);

                }
            }
            else if (other.attachedRigidbody != null && !other.attachedRigidbody.isKinematic)
            {
                if (Excludedtransforms.Contains(other.attachedRigidbody.transform)) return;
                other.attachedRigidbody.transform.position = otherPortal.transform.position;
                Vector3 dir = other.attachedRigidbody.transform.position - transform.position;
                dir.Normalize();
                other.attachedRigidbody.AddForce(dir * 3, ForceMode.VelocityChange);
                otherPortal.Excludedtransforms.Add(other.attachedRigidbody.transform);
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (!otherPortal.gameObject.activeSelf) { return; }

            if (!ModSettings.IsDedicated && other.transform.root == LocalPlayer.Transform)
            {
                return;
            }
            else if(other.attachedRigidbody!= null)
            {
                Excludedtransforms.Remove(other.attachedRigidbody.transform);
            }
        }

        float TimeOfPass = 0;
      
      
    }
}
