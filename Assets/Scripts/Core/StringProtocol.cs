using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 字符串协议 提供对字符串的编码解码 |长度 字符串|
/// </summary>
public class StringProtocol : BaseProtocol
{
    /// <summary>
    /// 传输的消息字符串
    /// </summary>
    public string msgStr = "";

    /// <summary>
    /// 解码成一个消息字符串
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="start"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public override BaseProtocol Decode(byte[] buff, int start, int length)
    {

        StringProtocol protocol = new StringProtocol();
        try
        {
            protocol.msgStr = Encoding.UTF8.GetString(buff, start, length);
            return protocol;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Decode 异常：{e.Message}");
            return protocol;
        }
    }

    /// <summary>
    /// 编码字符串 成一个字节流
    /// </summary>
    /// <returns>返回字节数组</returns>
    public override byte[] Encode()
    {
        try
        {
            byte[] msgBytes = Encoding.UTF8.GetBytes(this.msgStr);
            byte[] lengthBytes = BitConverter.GetBytes(msgBytes.Length);
            byte[] allBytes = lengthBytes.Concat(msgBytes).ToArray();
            byte[] allLengthBytes = BitConverter.GetBytes(allBytes.Length);
            return allLengthBytes.Concat(allBytes).ToArray();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Encode 异常：{e.Message}");
            return null;
        }

    }

    /// <summary>
    /// 得到字符串中协议的名称 根据","分隔
    /// </summary>
    /// <returns>字符串中协议名称</returns>
    public override string GetName()
    {
        if (string.IsNullOrEmpty(this.msgStr))
            return "";
        else
            return this.msgStr.Split(',')[0];
    }

    /// <summary>
    /// 返回消息的字符串
    /// </summary>
    /// <returns></returns>
    public override string GetDesc()
    {
        return msgStr;
    }
}
