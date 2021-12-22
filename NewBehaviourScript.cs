using Aspose.Words;
using Aspose.Words.Markup;
using Aspose.Words.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;


public class NewBehaviourScript : MonoBehaviour
{

    public void Download()
    {
        StartCoroutine(WriteToBookmark("Report.docx"));
    }



    IEnumerator WriteToBookmark(string fileName)
    {
        System.Uri uri = new System.Uri(Path.Combine(Application.streamingAssetsPath, fileName));
        UnityWebRequest request = UnityWebRequest.Get(uri);
        yield return request.SendWebRequest();

        if (request.isHttpError|| request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Stream stream = new MemoryStream(request.downloadHandler.data);

            Document doc = new Document(stream);
            DocumentBuilder builder = new DocumentBuilder(doc);   

            if (builder.MoveToBookmark("test"))
            {
                builder.Write("11111");
            }

           
            MemoryStream outStream = new MemoryStream();
            doc.Save(outStream, SaveFormat.Docx);
            WebGLDownloadHelper.DownloadDocx(outStream.ToArray(), "NewOne.docx");

#if UNITY_EDITOR
             AssetDatabase.Refresh();
#endif
        }
    }

}
