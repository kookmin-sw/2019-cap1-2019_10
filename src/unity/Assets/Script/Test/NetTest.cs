using System.Collections;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class NetTest : MonoBehaviour
{
    private string pubkey;
    public string url;

    class CertPublicKey : CertificateHandler
    {
        public string PUB_KEY;

        // Encoded RSAPublicKey
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            X509Certificate2 certificate = new X509Certificate2(certificateData);
            string pk = certificate.GetPublicKeyString();

            if (pk.ToLower().Equals(PUB_KEY.ToLower()))
                return true;
            else
                return false;
        }
    }

    private void Start()
    {
        //인증에 필요한 PublicKey 생성
        TextAsset tx = Resources.Load<TextAsset>("certificateFile");
        byte[] by = Encoding.UTF8.GetBytes(tx.ToString());

        X509Certificate2 certificate = new X509Certificate2(by);
        pubkey = certificate.GetPublicKeyString();

        StartCoroutine(Post(url, by));
    }

    private IEnumerator Post(string url, byte[] data)
    {
        //byte로 데이터를 전송 해야하는데 UnityWebRequest.POST는 string만 가능 하여 Put으로 넣은뒤 POST로 변경 해서 전송

        UnityWebRequest request = UnityWebRequest.Put(url, data);
        {
            request.method = "POST";
            request.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };
            //request.SetRequestHeader("Content-Type", "application/json"); //json전송이 필요하다면

            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error + " / " + request.responseCode);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
}