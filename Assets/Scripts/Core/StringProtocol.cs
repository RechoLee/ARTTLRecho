using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 字符串协议 提供对字符串的编码解码
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
    /// 编码字符串
    /// </summary>
    /// <returns>返回字节数组</returns>
    public override byte[] Encode()
    {
        try
        {
            byte[] b = Encoding.UTF8.GetBytes(this.msgStr);
            return b;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Encode 异常：{e.Message}");
            return new byte[] { };
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