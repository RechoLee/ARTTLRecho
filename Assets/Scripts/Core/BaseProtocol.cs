using System;
using System.Collections.Generic;
using System.Text;

public class BaseProtocol
{
    /// <summary>
    /// 解码方法
    /// </summary>
    /// <param name="buff">传入解码的buff</param>
    /// <param name="start">开始的位置</param>
    /// <param name="length">解析的长度</param>
    /// <returns>返回解码后的协议体</returns>
    public virtual BaseProtocol Decode(byte[] buff, int start, int length)
    {
        return new BaseProtocol();
    }

    /// <summary>
    /// 编码方法
    /// </summary>
    /// <returns>返回编码后的字节数组</returns>
    public virtual byte[] Encode()
    {
        return new byte[] { };
    }

    /// <summary>
    /// 获取协议名称
    /// </summary>
    /// <returns>返回协议名称</returns>
    public virtual string GetName()
    {
        return "";
    }

    /// <summary>
    /// 获取协议的内容
    /// </summary>
    /// <returns>返回描叙信息 协议内容</returns>
    public virtual string GetDesc()
    {
        return "";
    }
}