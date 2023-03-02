using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace ReadyPlayerMe.Core
{
    public static class NetworkUtil
    {
        public enum NetworkType
        {
            Fast,
            Good,
            Poor
        }
        
        public class BenchmarkResults
        {
            public float Duration;
            public float Speed;
            public NetworkType NetworkType;
        }
        
        public static async Task<BenchmarkResults> Benchmark(string url)
        {
            if (url.Contains(".glb"))
            {
                // url = url.Replace(".glb", ".json");
            }
            
            var startTime = Time.time;
            
            using var request = new UnityWebRequest();
            request.url = url;
            request.downloadHandler = new DownloadHandlerBuffer();
            request.method = UnityWebRequest.kHttpVerbGET;

            Debug.Log(url);
            
            var asyncOperation = request.SendWebRequest();
            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }

            if (request.downloadedBytes == 0 || request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
            {
                throw new Exception(request.error);
            }

            var duration = Time.time - startTime;
            var size = request.downloadHandler.data.Length/(float) 1024;
            var speed = size / duration;
           
            Debug.Log(request.GetResponseHeader("Content-Length"));

            Debug.Log(duration);
            Debug.Log(size);
            Debug.Log(speed + "KB/s");
            
            return new BenchmarkResults
            {
                Duration = duration,
                Speed = speed,
                NetworkType = NetworkType.Fast
            };
        }
    }
}
