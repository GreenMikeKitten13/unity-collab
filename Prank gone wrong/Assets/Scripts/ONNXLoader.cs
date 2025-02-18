using System;
using System.IO;
using UnityEngine;
using System.Runtime.InteropServices;
//using Microsoft.ML.OnnxRuntime;

public class ONNXLoader : MonoBehaviour
{
    private InferenceSession session;

    void Start()
    {
        try
        {
            string modelPath = Path.Combine(Application.streamingAssetsPath, "your_model.onnx");

            // Load ONNX model
            session = new InferenceSession(modelPath);
            Debug.Log("ONNX Model Loaded Successfully!");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load ONNX Model: {e.Message}");
        }
    }

    void OnDestroy()
    {
        session?.Dispose();
    }
}
