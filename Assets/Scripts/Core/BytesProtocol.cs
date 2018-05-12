using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 字节流协议 根据顺序解析需要的数据
/// </summary>
public class BytesProtocol : BaseProtocol
{
    /// <summary>
    /// 字节数组
    /// </summary>
    public byte[] bytes;

    /// <summary>
    /// 截取对应的字节流数据
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="start"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public override BaseProtocol Decode(byte[] buff, int start, int length)
    {
        BytesProtocol protocol = new BytesProtocol();

        try
        {
            protocol.bytes = new byte[length];
            Array.Copy(buff, start, protocol.bytes, 0, length);
            return protocol;
        }
        catch (Exception e)
        {
            Console.WriteLine($"decode 异常：{e.Message}");
            return protocol;
        }
    }

    /// <summary>
    /// 编码 封装成一个首部加长度的消息体
    /// </summary>
    /// <returns>返回自身的bytes</returns>
    public override byte[] Encode()
    {
        try
        {
            //byte[] msgLengthBytes = BitConverter.GetBytes(this.bytes.Length);
            //byte[] msgBytes = msgLengthBytes.Concat(this.bytes).ToArray();
            //byte[] allLength = BitConverter.GetBytes(msgBytes.Length);
            //return allLength.Concat(msgBytes).ToArray();

            byte[] allLenBytes = BitConverter.GetBytes(this.bytes.Length+sizeof(int));
            return allLenBytes.Concat(this.bytes).ToArray();

        }
        catch (Exception e)
        {
            Console.WriteLine($"encode failed :{e.Message}");
            return null;
        }
    }

    /// <summary>
    /// 获取协议体中的协议名称 一般第一个字符串
    /// </summary>
    /// <returns>返回第一个字符串</returns>
    public override string GetName()
    {
        //TODO:
        return GetString(0);
    }

    /// <summary>
    /// 返回整条协议体的字节数据 arcII 码形式
    /// </summary>
    /// <returns></returns>
    public override string GetDesc()
    {
        string str = "";
        if (this.bytes == null)
            return str;

        for (int i = 0; i < this.bytes.Length; i++)
        {
            int b = (int)this.bytes[i];
            str += b.ToString() + " ";
        }
        return str;
    }


    /// <summary>
    /// 向字节流数组中追加数据
    /// </summary>
    /// <param name="str"></param>
    public void AddString(string str)
    {
        try
        {
            byte[] strBytes = Encoding.UTF8.GetBytes(str);
            byte[] lenBytes = BitConverter.GetBytes(strBytes.Length);

            if (this.bytes == null)
            {
                this.bytes = lenBytes.Concat(strBytes).ToArray();
            }
            else
            {
                this.bytes = this.bytes.Concat(lenBytes).Concat(strBytes).ToArray();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    /// <summary>
    /// 向字节流中添加Int数据
    /// </summary>
    /// <param name="num"></param>
    public void AddInt(int number)
    {
        try
        {
            byte[] numBytes = BitConverter.GetBytes(number);
            byte[] lenBytes = BitConverter.GetBytes(numBytes.Length);
            if (this.bytes == null)
            {
                this.bytes = lenBytes.Concat(numBytes).ToArray();
            }
            else
            {
                this.bytes = this.bytes.Concat(lenBytes).Concat(numBytes).ToArray();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }

    /// <summary>
    /// 添加Float数据
    /// </summary>
    /// <param name="number"></param>
    public void AddFloat(float number)
    {
        try
        {
            byte[] numBytes = BitConverter.GetBytes(number);
            byte[] lenBytes = BitConverter.GetBytes(numBytes.Length);
            if (this.bytes == null)
            {
                this.bytes = lenBytes.Concat(numBytes).ToArray();
            }
            else
            {
                this.bytes = this.bytes.Concat(lenBytes).Concat(numBytes).ToArray();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }

    /// <summary>
    /// 从字节数组中读取字符串
    /// </summary>
    /// <returns></returns>
    public string GetString(int start, ref int end)
    {
        try
        {
            if (this.bytes == null)
                return null;

            //不够一条消息长度字节的长度
            if (this.bytes.Length < start + sizeof(Int32))
                return null;

            Int32 strLength = BitConverter.ToInt32(this.bytes, start);
            //不够一条完整消息长度
            if (this.bytes.Length < start + sizeof(Int32) + strLength)
            {
                return null;
            }

            string msgStr = Encoding.UTF8.GetString(
                this.bytes, start + sizeof(Int32), strLength);

            end = start + sizeof(Int32) + strLength;
            return msgStr;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

    }

    /// <summary>
    /// 用于获取知道指定开始位置的字符串
    /// </summary>
    /// <param name="start"></param>
    /// <returns></returns>
    public string GetString(int start)
    {
        int end = 0;
        return GetString(start, ref end);
    }


    /// <summary>
    /// 获取Int数据
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public int? GetInt(int start, ref int end)
    {
        try
        {
            if (this.bytes == null)
                return null;
            //if (this.bytes.Length < start + sizeof(Int32))
            //    return null;
            if (this.bytes.Length < start + sizeof(Int32) + sizeof(int))
                return null;
            end = start + sizeof(Int32) + sizeof(int);
            return BitConverter.ToInt32(this.bytes, start + sizeof(Int32));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public int? GetInt(int start)
    {
        int end = 0;
        return GetInt(start, ref end);
    }

    /// <summary>
    /// 获取Float数据
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public float? GetFloat(int start, ref int end)
    {
        if (this.bytes == null)
        {
            return null;
        }
        //if(this.bytes.Length<start+sizeof(Int32))
        //{
        //    return null;
        //}
        if (this.bytes.Length < start + sizeof(Int32) + sizeof(float))
        {
            return null;
        }
        end = start + sizeof(Int32) + sizeof(float);

        try
        {
            return BitConverter.ToSingle(this.bytes, start + sizeof(Int32));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
}
