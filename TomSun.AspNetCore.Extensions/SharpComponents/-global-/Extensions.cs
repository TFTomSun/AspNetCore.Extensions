using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TomSun.AspNetCore.Extensions.ExternalFramework;
using TomSun.AspNetCore.Extensions.SharpComponents;

public static class Extensions
{
    public static string GetHtmlString(this IHtmlContent htmlContent)
    {
        using (var stringWriter = new StringWriter())
        {
            htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);
            return stringWriter.ToString();
        }
    }
    public  static string BuildUrl(this string entry, params (string Name, string Value)[] parameters)
    {
        var url = entry + "?";

        foreach (var parameter in parameters)
        {
            url += $"{parameter.Name}={parameter.Value}&";
        }
        var result = url.TrimEnd('&');
        return result;
    }

    private static int BUFFER_SIZE = 64 * 1024; //64kB

    public static byte[] Compress(this byte[] inputData)
    {
        if (inputData == null)
            throw new ArgumentNullException("inputData must be non-null");

        using (var compressIntoMs = new MemoryStream())
        {
            using (var gzs = new BufferedStream(new GZipStream(compressIntoMs,
                CompressionMode.Compress), BUFFER_SIZE))
            {
                gzs.Write(inputData, 0, inputData.Length);
            }
            return compressIntoMs.ToArray();
        }
    }

    public static byte[] Decompress(this byte[] inputData)
    {
        if (inputData == null)
            throw new ArgumentNullException("inputData must be non-null");

        using (var compressedMs = new MemoryStream(inputData))
        {
            using (var decompressedMs = new MemoryStream())
            {
                using (var gzs = new BufferedStream(new GZipStream(compressedMs,
                    CompressionMode.Decompress), BUFFER_SIZE))
                {
                    gzs.CopyTo(decompressedMs);
                }
                return decompressedMs.ToArray();
            }
        }
    }

    public static IComponentRenderer<SharpViewComponent<TParameter>> WithParameter<TParameter>(
        this IComponentRendererBuilder<SharpViewComponent<TParameter>> builder, TParameter parameter)
    {
        builder.Parameter = parameter;
        return (IComponentRenderer<SharpViewComponent<TParameter>>) builder;
    }

    public static TAttribute GetCustomAttribute<TAttribute>(this Type type)
        where TAttribute : Attribute
    {
        return (TAttribute) type.GetCustomAttribute(typeof(TAttribute));
    }

    public static async Task<IHtmlContent> Sync<TParameter>(
        this IComponentRenderer<SharpViewComponent<TParameter>> builder)
    {
       var parameter =   SharpViewComponent.GetInvokeParameter(
            builder.ComponentInfo.InvokeMethodParameterName.Value,
            builder.Parameter);
      
        return await Api.Global.ViewComponentHelper().InvokeAsync(
            builder.ComponentInfo.Type, parameter); //new { parameter = builder.Parameter });
    }

  
    public static async Task<IHtmlContent> Async<TParameter>(
        this IComponentRenderer<SharpViewComponent<TParameter>> builder)
    {
        var asyncRendererType = builder.ComponentInfo.AsyncRendererComponentType;
        var asyncRendererParameter = builder.InvokeAsyncParameter;

        return await Api.Global.ViewComponentHelper().InvokeAsync(
            asyncRendererType, new {parameter = asyncRendererParameter });
    }

    public static object DeserializeFromBase64(this BinaryFormatter serializer, string base64EncodedData, bool isCompressed, bool isUrlEncoded)
    {
        if (isUrlEncoded)
        {
            base64EncodedData = System.Net.WebUtility.UrlDecode(base64EncodedData);
        }
        var decodedBytes = Convert.FromBase64String(base64EncodedData);
        if (isCompressed)
        {
            decodedBytes = decodedBytes.Decompress();
        }
        var result =  serializer.DeserializeFromBase64(decodedBytes);
        return result;
    }

    public static object DeserializeFromBase64(this BinaryFormatter serializer, byte[] data)
    {
        using (var memoryStream = new MemoryStream(data))
        {
            return serializer.Deserialize(memoryStream);
        }
    }

    public static string SerializeToBase64(this BinaryFormatter serializer, object graph, bool compress, bool urlEncode)
    {
        var bytes = serializer.Serialize(graph);
        if (compress)
        {
            bytes = bytes.Compress();
        }
        var result =  Convert.ToBase64String(bytes);
        if (urlEncode)
        {
            result = WebUtility.UrlEncode(result);
        }
        return result;
    }

    public static byte[] Serialize(this BinaryFormatter serializer, object graph)
    {
        using (var memoryStream = new MemoryStream())
        {
            serializer.Serialize(memoryStream, graph);
            return memoryStream.GetBuffer();
        }
    }
}

